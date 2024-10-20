using System;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingView : UIView {

    private Slider _bgmSlider;
    private Slider _effctSlider;
    private Toggle _hapticToggle;

    private Button _closeBtn;

    private GameData _gameData;

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

        _bgmSlider = topElement.Q<Slider>("bgm-slider");
        _effctSlider = topElement.Q<Slider>("effect-slider");
        _hapticToggle = topElement.Q<Toggle>("haptic-toggle");

        _closeBtn = topElement.Q<Button>("closee-btn");
    }

    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        _bgmSlider.RegisterCallback<ChangeEvent<float>>(ChangBgmValue);
        _effctSlider.RegisterCallback<ChangeEvent<float>>(ChangEffectValue);
        _hapticToggle.RegisterCallback<ChangeEvent<bool>>(ChangeHapticValue);

        _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();
        _bgmSlider.UnregisterCallback<ChangeEvent<float>>(ChangBgmValue);
        _effctSlider.UnregisterCallback<ChangeEvent<float>>(ChangEffectValue);
        _hapticToggle.UnregisterCallback<ChangeEvent<bool>>(ChangeHapticValue);

        _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
    }

    #region registercallback
    private void ChangBgmValue(ChangeEvent<float> evt) {
        evt.StopPropagation();
        _gameData.bgmValue = evt.newValue;
        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangEffectValue(ChangeEvent<float> evt) {
        evt.StopPropagation();
        _gameData.effectValue = evt.newValue;
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

        _bgmSlider.value = _gameData.bgmValue;
        _effctSlider.value = _gameData.effectValue;
        _hapticToggle.value = _gameData.haptic;

        //SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
