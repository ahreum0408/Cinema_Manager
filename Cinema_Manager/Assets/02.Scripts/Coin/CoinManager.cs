using UnityEngine;

public class CoinManager : MonoSingleton<CoinManager>
{
    public int _coin = 0;
    private int _gam = 0;

    private char kilo = 'K';

    private GameData _gameData;

    public int Coin
    {
        get
        {
            return _coin;
        }
        set
        {
            _coin = value;
            _gameData.coin = _coin;
            string coin = CalculatePriceText(_coin);
            MainEvents.ChangeCoinEvent?.Invoke(coin);
            MainEvents.GameDataUpdatEvent?.Invoke(_gameData);

            if (_coin < 0)
            {
                _coin = 0;
                Debug.LogWarning("[주의] 현재 코인이 -임");
            }
        }
    }
    public int Gam
    {
        get
        {
            return _gam;
        }
        set
        {
            _coin = value;
            _gameData.gam = _gam;
            string gam = CalculatePriceText(_coin);
            MainEvents.ChangeGamEvent?.Invoke(gam);
            MainEvents.GameDataUpdatEvent?.Invoke(_gameData);

            if (_gam < 0)
            {
                _gam = 0;
                Debug.LogWarning("[주의] 현재 잼이 -임");
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        MainEvents.GameDataLoadEvent += GameDataLoad;
    }

    public string CalculatePriceText(int price)
    {
        int k = 1000;
        int h = 100;
        string calP = "";

        if (price / k > 0) {
            int kC = price / k;
            int hC = (price % k) / h;
            if (hC == 0) {
                calP = $"{kC}{kilo}";
            }
            else {
                Debug.Log(hC);
                calP = $"{kC}.{hC}{kilo}";
            }
        }
        else {
            calP = price.ToString();
        }

        return calP;
    }
    public void ResetGoods()
    {
        _coin = 0;
        _gam = 0;
    }
    private void GameDataLoad(GameData data)
    {
        if (data == null)
        {
            return;
        }
        _gameData = data;

        _coin = data.coin;
        _gam = data.gam;
    }
}
