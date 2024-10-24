using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class MainView : UIView {
    private Button _settingBtn;
    private Button _storeBtn;

    private Label _coinTxt;
    private Label _gamTxt;

    private ProgressBar _levelBar;
    private Label _levelTxt;

    private GameData _gameData;


    public MainView(VisualElement topElement) : base(topElement) {
        MainEvents.GameDataLoadEvent += GameDataLoad;

        MainEvents.ChangeCoinEvent += UpdateCoinTxt;
        MainEvents.ChangeGamEvent += UpdateGamTxt;

        MainEvents.GetExpEvent += UpdateExp;
        MainEvents.UpgradeLevelEvent += UpdateLevel;
    }
    public override void Dispose() {
        base.Dispose();
        MainEvents.GameDataLoadEvent -= GameDataLoad;

        MainEvents.ChangeCoinEvent -= UpdateCoinTxt;
        MainEvents.ChangeGamEvent -= UpdateGamTxt;

        MainEvents.GetExpEvent -= UpdateExp;
        MainEvents.UpgradeLevelEvent -= UpdateLevel;
    }
    protected override void SetVisualElements() {
        base.SetVisualElements();

        _settingBtn = topElement.Q<Button>("setting-btn");
        _storeBtn = topElement.Q<Button>("store-btn");

        _coinTxt = topElement.Q<Label>("coin-txt");
        _gamTxt = topElement.Q<Label>("gam-txt");

        _levelBar = topElement.Q<ProgressBar>("gaugebar");
        _levelTxt = topElement.Q<Label>("level-txt");
    }
    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        _settingBtn.RegisterCallback<ClickEvent>(ClickSettingBtn);
        _storeBtn.RegisterCallback<ClickEvent>(ClickStoreBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

        _settingBtn.UnregisterCallback<ClickEvent>(ClickSettingBtn);
        _settingBtn.UnregisterCallback<ClickEvent>(ClickStoreBtn);
    }

    #region level-bar
    private void UpdateExp(int exp) {
        _levelBar.value = exp;
        _levelBar.title = $"{exp} / {_gameData.level.highValue}";
        _gameData.exp = exp;
        MainEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void UpdateLevel(Level data, int index) {
        _levelTxt.text = data.levelNumder.ToString();
        SetLevelBarMinMaxValue(data);
        _gameData.level = data;
        _gameData.levelIndex = index;
        MainEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void SetLevelBarMinMaxValue(Level data) {
        _levelBar.lowValue = data.lowValue;
        _levelBar.highValue = data.highValue;
    }
    #endregion

    #region Handle
    private void ClickSettingBtn(ClickEvent evt) {
        MainEvents.SettingViewShow?.Invoke();
    }
    private void ClickStoreBtn(ClickEvent evt) {
        Debug.Log("»óÁ¡Ã¢ ÄÑÁü");
    }
    #endregion
    private void GameDataLoad(GameData data) {
        if (data == null) {
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
    private void UpdateCoinTxt(string coin) {
        _coinTxt.text = coin;
    }
    private void UpdateGamTxt(string gam) {
        _gamTxt.text = gam;
    }
}