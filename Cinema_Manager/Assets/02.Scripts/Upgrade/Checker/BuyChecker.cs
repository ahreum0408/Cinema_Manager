using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BuyChecker : CheckerArea {
    [SerializeField] private TextMeshPro _priceTxt;
    private int currentCoin => CoinManager.Instance.Coin;

    private void Awake() {
        CalculateWeght();

        _priceTxt.text = CoinManager.Instance.CalculatePriceText(_price);
    }

    protected IEnumerator CalculateCoin() {
        while (_isCalaulate) {
            WaitForSeconds waitTime = new WaitForSeconds(0.05f);
            if (_price <= 0 || currentCoin <= 0) {
                _isCalaulate = false;
                yield return null;
            }
            else if (_price - _minusCoin < 0) {
                _minusCoin = 1; // 여기 나중에 수정 필요함
            }
            else {
                _price -= _minusCoin;
                CoinManager.Instance.Coin -= _minusCoin;
                UpdatePriceText(_price);
                yield return waitTime;
            }
        }
    }
    private void UpdatePriceText(int coin) {
        _priceTxt.text = CoinManager.Instance.CalculatePriceText(coin);
    }

    public override void EnterInteraction() {
        _isCalaulate = true;
        StartCoroutine(CalculateCoin());
    }

    public override void ExitInteraction() {
        _isCalaulate = false;
        StopCoroutine(CalculateCoin());
    }
}
