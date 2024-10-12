using TMPro;
using UnityEngine;

public class TriggerChecker : MonoBehaviour {
    [SerializeField] private int _calculateWeight = 1; // 돈빠지는 속도
    [SerializeField] private int _price;

    [SerializeField] private TextMeshPro _priceTxt;

    private int minusCoin;

    private void Awake() {
        _priceTxt.text = _price.ToString();
        minusCoin = 1 * _calculateWeight;
    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            EnterInteraction();
        }
    }
    public void EnterInteraction() {
        if(_price <= 0) {
            return;
        }
        _price -= minusCoin;
        CoinManager.Instance.Coin -= minusCoin;
        Debug.Log(CoinManager.Instance.Coin);
        UpdatePriceText(_price);
    }
    private void UpdatePriceText(int price) {
        _priceTxt.text = price.ToString();
    }
}
