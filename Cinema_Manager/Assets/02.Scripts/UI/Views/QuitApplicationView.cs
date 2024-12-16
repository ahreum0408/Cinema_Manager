using System;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class QuitApplicationView : UIView {
    private Button _closeBtn;
    private Button _quitBtn;
    public QuitApplicationView(VisualElement topContainer) : base(topContainer) {
    }

    public override void Initialize() {
        base.Initialize();
    }

    public override void Show() {
        base.Show();
    }

    protected override void SetVisualElements() {
        base.SetVisualElements();

        _closeBtn = topElement.Q<Button>("close-btn");
        _quitBtn = topElement.Q<Button>("quit-btn");
    }
    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();
        _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        _quitBtn.RegisterCallback<ClickEvent>(ClickQuitBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

    }
    private void ClickCloseBtn(ClickEvent evt) {
        MainEvents.MainViewShow?.Invoke();
    }

    private void ClickQuitBtn(ClickEvent evt) {
        Application.Quit(); //¾Û Á¾·á
    }
}
