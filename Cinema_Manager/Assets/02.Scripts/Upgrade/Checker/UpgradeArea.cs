using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeArea : MonoBehaviour, IIneractionable {
    [SerializeField] private int _calculateWeight = 1; // 돈빠지는 속도
    [SerializeField] private int _price;

    [SerializeField] private TextMeshPro _priceTxt;

    private int _minusCoin;
    private bool _isCalaulate;

    private char kilo = 'K';
    private char mega = 'M';

    private void Awake() {
        _priceTxt.text = CalculatePriceText(_price);
        _minusCoin = 1 * _calculateWeight;
    }
    public void EnterInteraction() {
        _isCalaulate = true;
        StartCoroutine(CalculateCoin());
    }

    private IEnumerator CalculateCoin() {
        WaitForSeconds waitTime = new WaitForSeconds(0.05f);
        while (_isCalaulate) {
            if (_price <= 0) {
                _isCalaulate = false;
                yield return null;
            }
            if(_price - _minusCoin < 0) {
                _minusCoin = 0;
            }
            _price -= _minusCoin;
            CoinManager.Instance.Coin -= _minusCoin;
            UpdatePriceText(_price);
            yield return waitTime;
        }
    }

    public void ExitInteraction() {
        _isCalaulate = false;
        StopCoroutine(CalculateCoin());
    }
    private string CalculatePriceText(int price) {
        int m = 1000000;
        int k = 1000;
        string calP = "";

        if (price / m > 0) { // 백만
            calP = (price / m).ToString() + mega;
        }
        else if(price / k > 0) { // 천
            calP = (price / k).ToString() + kilo;
        }

        return calP;
    }
    private void UpdatePriceText(int price) {
        _priceTxt.text = CalculatePriceText(price);
    }
}
