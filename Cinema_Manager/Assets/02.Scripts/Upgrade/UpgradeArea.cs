using TMPro;
using UnityEngine;

public class UpgradeArea : MonoBehaviour, IIneractionable {
    [SerializeField] private int _calculateWeight = 1; // 돈빠지는 속도
    [SerializeField] private int _price;

    [SerializeField] private TextMeshPro _priceTxt;

    private bool isCal = false; // 돈을 계산하고 있는 중인가

    private void Awake() {
        _priceTxt.text = _price.ToString();
    }
    public void EnterInteraction() {
        int coin = 2 * _calculateWeight;
        if(CoinManager.Instance.Coin - coin < 0) {
            coin = 1;
        }
        CoinManager.Instance.Coin -= coin;
        UpdatePriceText(CoinManager.Instance.Coin);
    }
    public void ExitInteraction() {
    }
    private void CalMinusCoinSpeed() {

    }
    private void UpdatePriceText(int price) {
        _priceTxt.text = price.ToString();
    }
}
