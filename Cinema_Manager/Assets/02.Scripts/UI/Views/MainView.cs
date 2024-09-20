using System;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class MainView : UIView {
    private Button _settingBtn;
    private Button _storeBtn;

    private Label _coinTxt;
    private Label _gamTxt;

    private ProgressBar _levelBar;

    public MainView(VisualElement topElement) : base(topElement) {
        MainEvents.ChangeCoinEvent += UpdateCoinTxt;
        MainEvents.ChangeGamEvent += UpdateGamTxt;
    }
    
    public override void Dispose() {
        base.Dispose();

        MainEvents.ChangeCoinEvent -= UpdateCoinTxt;
        MainEvents.ChangeGamEvent -= UpdateGamTxt;
    }
    protected override void SetVisualElements() {
        base.SetVisualElements();
        _settingBtn = topElement.Q<Button>("setting-btn");
        _storeBtn = topElement.Q<Button>("store-btn");

        _coinTxt = topElement.Q<Label>("coin-txt");
        _gamTxt = topElement.Q<Label>("gam-txt");

        _levelBar = topElement.Q<ProgressBar>("gaugebar");
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
    private void SetLevelBarMinMaxValue(float minValue, float maxValue) {
        _levelBar.lowValue = minValue;
        _levelBar.highValue = maxValue;
    }
    private void LevelUp() {
        Debug.Log("levelUp");
        // 레벨에 맞게 텍스트도 변경
        SetLevelBarMinMaxValue(0, 110); // 다음 레벨에 맞도록 변경하고 
        float remainingValue = _levelBar.value - _levelBar.highValue;
        _levelBar.value = remainingValue;
    }
    public void GetEx() {
        _levelBar.value += 90;
        if(_levelBar.value >= _levelBar.highValue) {
            LevelUp();
        }
    }
    #endregion

    #region Handle
    private void ClickSettingBtn(ClickEvent evt) {
        Debug.Log("설정창 켜짐");
    }
    private void ClickStoreBtn(ClickEvent evt) {
        Debug.Log("상점창 켜짐");
    }
    #endregion

    private void UpdateCoinTxt(int coin) {
        _coinTxt.text = coin.ToString();    
    }
    private void UpdateGamTxt(int gam) {
        _gamTxt.text = gam.ToString();
    }
}
