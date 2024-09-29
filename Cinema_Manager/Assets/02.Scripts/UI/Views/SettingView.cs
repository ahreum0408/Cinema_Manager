using System;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingView : UIView {

    private Slider bgmSlider;
    private Slider effctSlider;

    private Button closeBtn;

    public SettingView(VisualElement topElement) : base(topElement) {
    }

    public override void Dispose() {
        base.Dispose();
    }

    protected override void SetVisualElements() {
        base.SetVisualElements();

        bgmSlider = topElement.Q<Slider>("bgm-slider");
        effctSlider = topElement.Q<Slider>("effect-slider");

        closeBtn = topElement.Q<Button>("closee-btn");
    }

    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        bgmSlider.RegisterCallback<ChangeEvent<float>>(ChangBgmValue);
        effctSlider.RegisterCallback<ChangeEvent<float>>(ChangEffectValue);

        closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
    }


    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();
        bgmSlider.UnregisterCallback<ChangeEvent<float>>(ChangBgmValue);
        effctSlider.UnregisterCallback<ChangeEvent<float>>(ChangEffectValue);

        closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
    }

    #region Handle
    private void ChangBgmValue(ChangeEvent<float> evt) {
        Debug.Log($"bgm value : {evt.newValue}");
        SettingEvents.UIGameDataChange?.Invoke();
        // 값 변한거 저장해줘야 하고
        // sound변경된거 재생 부분에서 처리하기
    }
    private void ChangEffectValue(ChangeEvent<float> evt) {
        Debug.Log($"effect value : {evt.newValue}");
        SettingEvents.UIGameDataChange?.Invoke();
        // 값 변한거 저장해줘야 하고
        // sound변경된거 재생 부분에서 처리하기
    }

    private void ClickCloseBtn(ClickEvent evt) {
        MainEvents.MainViewShow?.Invoke();
        // 창 변경 됬다는거 uimanager한테 안알려줬음 주의 할 것
    }
    #endregion
}
