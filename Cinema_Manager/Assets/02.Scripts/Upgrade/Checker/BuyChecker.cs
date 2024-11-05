using System.Collections;
using TMPro;
using UnityEngine;

public class BuyChecker : CheckerArea, IOpenTarget {
    [SerializeField] private TextMeshPro _priceTxt;
    [SerializeField] private GameObject _openTarget;
    [SerializeField] private int _exp;
    
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

    private void Awake() {
        CalculateWeght();
        UpdatePriceText(_price);
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
            if (_price <= 0) {
                EndCal();
                break;
            }

            if (_price - _minusCoin < 0) {
                _minusCoin = 1; // 여기 나중에 수정 필요함
            }
            _price -= _minusCoin;
            CoinManager.Instance.Coin -= _minusCoin;
            UpdatePriceText(_price);
            yield return waitTime;
        }
    }
    private void UpdatePriceText(int coin) {
        _priceTxt.text = CoinManager.Instance.CalculatePriceText(coin);
    }
    private void EndCal() {
        _isCalaulate = false;
        ActiveObj(false, true);
        OpenITarget.ActiveObj(true);
        LevelEvents.ChangePriceEvent?.Invoke(this, _price);
        LevelManager.Instance.GetExp(_exp);
    }
    public void ActiveObj(bool active, bool isAction = false) {
        _isOpen = active;
        gameObject.SetActive(active);
        if(isAction) {
            LevelEvents.ChangeCheckerActiveEvent?.Invoke(this, !active);
        }
    }
}
