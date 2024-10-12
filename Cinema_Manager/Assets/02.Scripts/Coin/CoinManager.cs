using System;
using UnityEngine;

public class CoinManager : MonoSingleton<CoinManager> {
    public int _coin = 0;
    private int _gam = 0;

    public int Coin {
        get {
            return _coin;
        }
        set {
            _coin = value;
            MainEvents.ChangeCoinEvent?.Invoke(_coin);

            if(_coin < 0) {
                //_coin = 0;
                Debug.LogWarning("[주의] 현재 코인이 -임");
            }
        }
    }
    public int Gam {
        get {
            return _gam;
        }
        set {
            _gam = value;
            MainEvents.ChangeGamEvent?.Invoke(_gam);
            
            if (_gam < 0) {
               // _gam = 0;
                Debug.LogWarning("[주의] 현재 잼이 -임");
            }
        }
    }

    public void ResetGoods() {
        _coin = 0;
        _gam = 0;
    }
}
