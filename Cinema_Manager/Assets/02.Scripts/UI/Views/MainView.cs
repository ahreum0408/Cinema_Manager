using System.Threading.Tasks;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class MainView : UIView
{
    private Button _settingBtn;
    private Button _storeBtn;

    private Label _coinTxt;
    private Label _gamTxt;

    private ProgressBar _levelBar;
    private Label _levelTxt;

    public MainView(VisualElement topElement) : base(topElement)
    {
        MainEvents.GameDataLoadEvent += GameDataLoad;

        MainEvents.ChangeCoinEvent += UpdateCoinTxt;
        MainEvents.ChangeGamEvent += UpdateGamTxt;

        MainEvents.GetExpEvent += UpdateExp;
        MainEvents.UpgradeLevelEvent += UpdateLevel;
    }
    public override void Dispose()
    {
        base.Dispose();
        MainEvents.GameDataLoadEvent -= GameDataLoad;

        MainEvents.ChangeCoinEvent -= UpdateCoinTxt;
        MainEvents.ChangeGamEvent -= UpdateGamTxt;

        MainEvents.GetExpEvent -= UpdateExp;
        MainEvents.UpgradeLevelEvent -= UpdateLevel;
    }
    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        _settingBtn = topContainer.Q<Button>("setting-btn");
        _storeBtn = topContainer.Q<Button>("store-btn");

        _coinTxt = topContainer.Q<Label>("coin-txt");
        _gamTxt = topContainer.Q<Label>("gam-txt");

        _levelBar = topContainer.Q<ProgressBar>("gaugebar");
        _levelTxt = topContainer.Q<Label>("level-txt");
    }
    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        _settingBtn.RegisterCallback<ClickEvent>(ClickSettingBtn);
        _storeBtn.RegisterCallback<ClickEvent>(ClickStoreBtn);
    }
    protected override void UnRegisterButtonCallbacks()
    {
        base.UnRegisterButtonCallbacks();

        _settingBtn.UnregisterCallback<ClickEvent>(ClickSettingBtn);
        _settingBtn.UnregisterCallback<ClickEvent>(ClickStoreBtn);
    }

    public override void Show()
    {
        base.Show();
        MainEvents.ShowViewEvent?.Invoke();
    }

    public override void Hide() {
        base.Hide();
        Debug.Log("main view hide");
    }

    #region level-bar
    private async void UpdateExp(int exp)
    {
        float currentExp = _levelBar.value;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _levelBar.value = Mathf.Lerp(currentExp, exp, t);
            _levelBar.title = $"{Mathf.RoundToInt(_levelBar.value)} / {_gameData.level.highValue}";
            await Task.Yield(); // 다음 프레임까지 대기
        }

        _levelBar.value = exp;
        _levelBar.title = $"{exp} / {_gameData.level.highValue}";
        _gameData.exp = exp;
        MainEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void UpdateLevel(Level data, int index)
    {
        _levelTxt.text = data.levelNumder.ToString();
        SetLevelBarMinMaxValue(data);
        _gameData.level = data;
        _gameData.levelIndex = index;
        //_levelBar.title = $"0 / {_gameData.level.highValue}";
        //UpdateExp(_gameData.exp);
        MainEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void SetLevelBarMinMaxValue(Level data)
    {
        _levelBar.lowValue = data.lowValue;
        _levelBar.highValue = data.highValue;
    }
    #endregion

    #region Handle
    private void ClickSettingBtn(ClickEvent evt)
    {
        MainEvents.SettingViewShow?.Invoke();
    }
    private void ClickStoreBtn(ClickEvent evt)
    {
        Debug.Log("상점창 켜짐");
    }
    #endregion
    private void GameDataLoad(GameData data)
    {
        if (data == null)
        {
            return;
        }
        _gameData = data;

        _levelBar.value = data.exp;
        _levelBar.title = $"{data.exp} / {_gameData.level.highValue}";
        _levelTxt.text = data.level.levelNumder.ToString();
        SetLevelBarMinMaxValue(_gameData.level);

        UpdateCoinTxt(CoinManager.Instance.CalculatePriceText(data.coin));
        UpdateGamTxt(CoinManager.Instance.CalculatePriceText(data.gam));


        //SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void UpdateCoinTxt(string coin)
    {
        _coinTxt.text = coin;
    }
    private void UpdateGamTxt(string gam)
    {
        _gamTxt.text = gam;
    }
}