using System;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingView : UIView {

    private Toggle _bgmToggle;
    private Toggle _effctToggle;
    private Toggle _hapticToggle;

    private Button _closeBtn;

    public SettingView(VisualElement topElement) : base(topElement) {
        SettingEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose() {
        base.Dispose();
        SettingEvents.GameDataLoadEvent-= GameDataLoad;
    }

    public override void Show() {
        base.Show();
        MainEvents.ShowViewEvent?.Invoke();
    }

    protected override void SetVisualElements() {
        base.SetVisualElements();

        _bgmToggle = topElement.Q<Toggle>("bgm-toggle");
        _effctToggle = topElement.Q<Toggle>("effect-toggle");
        _hapticToggle = topElement.Q<Toggle>("haptic-toggle");

        _closeBtn = topElement.Q<Button>("closee-btn");
    }

    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        _bgmToggle.RegisterCallback<ChangeEvent<bool>>(ChangBgmValue);
        _effctToggle.RegisterCallback<ChangeEvent<bool>>(ChangEffectValue);
        _hapticToggle.RegisterCallback<ChangeEvent<bool>>(ChangeHapticValue);

        _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();
        _bgmToggle.UnregisterCallback<ChangeEvent<bool>>(ChangBgmValue);
        _effctToggle.UnregisterCallback<ChangeEvent<bool>>(ChangEffectValue);
        _hapticToggle.UnregisterCallback<ChangeEvent<bool>>(ChangeHapticValue);

        _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
    }

    #region registercallback
    private void ChangBgmValue(ChangeEvent<bool> evt) {
        evt.StopPropagation();
        _gameData.bgm = evt.newValue;
        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangEffectValue(ChangeEvent<bool> evt) {
        evt.StopPropagation();
        _gameData.effect = evt.newValue;
        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeHapticValue(ChangeEvent<bool> evt) {
        evt.StopPropagation();
        _gameData.haptic = evt.newValue;
        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickCloseBtn(ClickEvent evt) {
        MainEvents.MainViewShow?.Invoke();
        // 창 변경 됬다는거 uimanager한테 안알려줬음 주의 할 것
    }

    #endregion
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        _bgmToggle.value = _gameData.bgm;
        _effctToggle.value = _gameData.effect;
        _hapticToggle.value = _gameData.haptic;

        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
