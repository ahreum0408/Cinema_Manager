using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using static AyunDefine;

public class BuyChecker : CheckerArea, IOpenTarget {
    [SerializeField] private TextMeshPro _priceTxt;
    [SerializeField] public GameObject _openTarget;
    [SerializeField] private int _exp;
    [SerializeField] private TargetType _targetType;
    
    public int Price { 
        get { 
            return _price; 
        }

        set { 
            _price = value;
            UpdatePriceText(_price);
        } 
    }
    public GameObject OpenGTarget => _openTarget;
    public IOpenTarget OpenITarget => _openTarget.GetComponent<IOpenTarget>();
    private int currentCoin => CoinManager.Instance.Coin;

    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public TargetType Type { get => _targetType; set => _targetType = value; }

    private void Awake() {
        CalculateWeght();
        //UpdatePriceText(_price);
    }

    public override void EnterInteraction(AgentController agent) {
        _isCalaulate = true;
        StartCoroutine(CalculateCoin());
    }
    public override void ExitInteraction(AgentController agent) {
        _isCalaulate = false;
        LevelEvents.ChangePriceEvent?.Invoke(this, _price);
        StopCoroutine(CalculateCoin());
    }

    protected IEnumerator CalculateCoin() {
        while (_isCalaulate) {
            WaitForSeconds waitTime = new WaitForSeconds(0.05f);
            if (currentCoin <= 0) {
                _isCalaulate = false;
                break;
            }
            if (_price - _minusCoin < 0) {
                _minusCoin = 1; // 여기 나중에 수정 필요함
            }
            _price -= _minusCoin;
            CoinManager.Instance.Coin -= _minusCoin;
            UpdatePriceText(_price);
            if (_price <= 0) {
                EndCal();
                break;
            }
            yield return waitTime;
        }
    }
    private void EndCal() {
        _isCalaulate = false;
        LevelEvents.ChangePriceEvent?.Invoke(this, _price);
        LevelManager.Instance.GetExp(_exp);
        ActiveObj(false); // 나 끄고
        OpenITarget.ActiveObj(true, true); // 타겟 키고

        // Sound
        SoundManager.Instance.Play(AudioClips.BuyObject, 1f);
    }
    private void UpdatePriceText(int coin) {
        _priceTxt.text = CoinManager.Instance.CalculatePriceText(coin);
        LevelEvents.PriceChangingEvent?.Invoke(transform);

        // Sound
        SoundManager.Instance.Play(AudioClips.Stack, 1);
    }
    public void ActiveObj(bool active, bool onTarget = false) {
        if(onTarget) { // 맨처음 로드 될 때만 사용
            OpenITarget.ActiveObj(!active);
        }
        _isOpen = active;
        gameObject.SetActive(active);
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
