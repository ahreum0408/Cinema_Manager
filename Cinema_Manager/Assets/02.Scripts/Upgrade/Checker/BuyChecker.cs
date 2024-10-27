using System.Collections;
using TMPro;
using UnityEngine;

public class BuyChecker : CheckerArea {
    [SerializeField] private TextMeshPro _priceTxt;
    [SerializeField] private GameObject openTarget;
    
    public int Price { 
        get { 
            return _price; 
        }

        set { 
            _price = value;
            UpdatePriceText(_price);
        } 
    }
    //public bool IsOpen => openTarget.activeInHierarchy == true ? true : false;
    private int currentCoin => CoinManager.Instance.Coin;

    private void Awake() {
        CalculateWeght();
        SetActiveMap(false);
        _priceTxt.text = CoinManager.Instance.CalculatePriceText(_price);
    }

    public override void EnterInteraction() {
        _isCalaulate = true;
        StartCoroutine(CalculateCoin());
    }
    public override void ExitInteraction() {
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
                _isCalaulate = false;
                LevelEvents.ChangePriceEvent?.Invoke(this, _price);
                SetActiveMap(true);
                gameObject.SetActive(false);
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
    public void SetActiveMap(bool active) {
        if (openTarget != null) {
            openTarget.SetActive(active);
        }
    }
}
