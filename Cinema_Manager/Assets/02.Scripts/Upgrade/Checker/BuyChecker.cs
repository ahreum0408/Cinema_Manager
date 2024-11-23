using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using static AyunDefine;

public class BuyChecker : CheckerArea, IOpenTarget
{
    [SerializeField] private TextMeshPro _priceTxt;
    [SerializeField] public GameObject _openTarget;
    [SerializeField] private int _exp;
    [SerializeField] private TargetType _targetType;

    public int Exp => _exp;
    public int Price
    {
        get
        {
            return _price;
        }

        set
        {
            _price = value;
            UpdatePriceText(_price);
        }
    }
    public GameObject OpenGTarget => _openTarget;
    public IOpenTarget OpenITarget => _openTarget.GetComponent<IOpenTarget>();
    private int CurrentCoin => CoinManager.Instance.Coin;

    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public TargetType Type { get => _targetType; set => _targetType = value; }

    private void Awake()
    {
        CalculateWeght();
    }
    private void Start()
    {
        UpdatePriceText(_price, true);
    }

    public override void EnterInteraction(AgentController agent)
    {
        _isCalaulate = true;
        StartCoroutine(CalculateCoin());
    }
    public override void ExitInteraction(AgentController agent)
    {
        _isCalaulate = false;
        LevelEvents.ChangePriceEvent?.Invoke(this, _price);
        StopCoroutine(CalculateCoin());
    }

    protected IEnumerator CalculateCoin()
    {
        while (_isCalaulate)
        {
            WaitForSeconds waitTime = new WaitForSeconds(0.05f);
            if (CurrentCoin <= 0)
            {
                _isCalaulate = false;
                break;
            }
            if (_price - _minusCoin < 0) // 100 -> 90
            {
                _minusCoin = 1; // ¿©±â ³ªÁß¿¡ ¼öÁ¤ ÇÊ¿äÇÔ
            }
            _price -= _minusCoin; // chcker µ· »©°í
            CoinManager.Instance.Coin -= _minusCoin; // ½ÇÁ÷ÀûÀÎ µ· »©°í
            UpdatePriceText(_price);
            if (_price <= 0)
            {
                EndCal();
                break;
            }
            yield return waitTime;
        }
    }
    private void EndCal()
    {
        _isCalaulate = false;
        LevelEvents.ChangePriceEvent?.Invoke(this, _price);
        LevelManager.Instance.GetExp(_exp);
        ActiveObj(false); // ³ª ²ô°í
        OpenITarget.ActiveObj(true, true); // Å¸°Ù Å°°í

        // Sound
        SoundManager.Instance.Play(AudioClips.BuyObject, 1f);
        VibrationManager.Instance.Vibrate(100, 36);
    }
    private void UpdatePriceText(int coin, bool load = false)
    {
        _priceTxt.text = CoinManager.Instance.CalculatePriceText(coin);
        if (!load)
        {
            LevelEvents.PriceChangingEvent?.Invoke(transform);
            VibrationManager.Instance.Vibrate(5, 24);
        }

        // Sound
        SoundManager.Instance.Play(AudioClips.Stack, 1);
    }
    public void ActiveObj(bool active, bool firstLoad = false)
    {
        _isOpen = active;
        gameObject.SetActive(active);
        if (!firstLoad)
        {
            ScaleSetting();
        }
        LevelEvents.ChangeCheckerActiveEvent?.Invoke(this, active);
    }

    public void ScaleSetting()
    {
        float time = 0.5f;
        Vector3 originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, time).SetEase(Ease.OutBack);
    }
}
