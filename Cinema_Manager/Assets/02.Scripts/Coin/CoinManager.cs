using System;
using UnityEngine;

public class CoinManager : MonoSingleton<CoinManager> {
    private int _coin = 0;

    public int Coin {
        get {
            return _coin;
        }
        set {
            if(_coin - value < 0) {
                _coin = value;
                Debug.LogWarning("현재 코인이 -임");
            }
            else {
                _coin = value;
            }
            changeCoinEvent?.Invoke(_coin);
        }

    }

    public Action<int> changeCoinEvent; // ui변경을 넣거나

    public void ResetCoins() {
        _coin = 0;
    }
}
