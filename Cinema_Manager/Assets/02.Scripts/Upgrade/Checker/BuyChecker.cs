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
    [SerializeField] private float _waitTime = 0.8f;

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

    private bool _isFirst;
    public bool IsFirst { get => _isFirst; set => _isFirst = value; }

    private float time;

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
        StartCoroutine(WaitEnter());
    }
    public override void ExitInteraction(AgentController agent)
    {
        StopCoroutine(CalculateCoin());
        _isCalaulate = false;
        LevelEvents.ChangePriceEvent?.Invoke(this, _price);
    }
    private IEnumerator WaitEnter() {
        _isCalaulate = true;
        yield return new WaitForSeconds(_waitTime);

        if (_isCalaulate) {
            StartCoroutine(CalculateCoin());
        }
    }
    protected IEnumerator CalculateCoin()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.05f);
        while (_isCalaulate)
        {
            if (CurrentCoin <= 0)
            {
                _isCalaulate = false;
                break;
            }

            if (_minusCoin > Mathf.Min(_price, CurrentCoin)) {
                _minusCoin = Mathf.Min(_price, CurrentCoin);
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
