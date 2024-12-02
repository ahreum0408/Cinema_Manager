using UIToolkit;
using UnityEngine.UIElements;
using static AyunDefine;

public class SettingView : UIView
{

    private Toggle _soundToggle;
    private Toggle _hapticToggle;

    private Button _closeBtn;
    private Button _moneyBtn;

    public SettingView(VisualElement topElement) : base(topElement)
    {
        SettingEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose()
    {
        base.Dispose();
        SettingEvents.GameDataLoadEvent -= GameDataLoad;
    }

    public override void Show()
    {
        base.Show();
        MainEvents.ShowViewEvent?.Invoke();
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        _soundToggle = topContainer.Q<Toggle>("sound-toggle");
        _hapticToggle = topContainer.Q<Toggle>("haptic-toggle");

        _closeBtn = topContainer.Q<Button>("closee-btn");
        _moneyBtn = topContainer.Q<Button>("money-btn");
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        _soundToggle.RegisterCallback<ChangeEvent<bool>>(ChangSoundValue);
        _hapticToggle.RegisterCallback<ChangeEvent<bool>>(ChangeHapticValue);

        _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        _moneyBtn.RegisterCallback<ClickEvent>(ClickMoneyBtn);
    }
    protected override void UnRegisterButtonCallbacks()
    {
        base.UnRegisterButtonCallbacks();
        _soundToggle.UnregisterCallback<ChangeEvent<bool>>(ChangSoundValue);
        _hapticToggle.UnregisterCallback<ChangeEvent<bool>>(ChangeHapticValue);

        _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
        _moneyBtn.UnregisterCallback<ClickEvent>(ClickMoneyBtn);
    }

    #region registercallback
    private void ChangSoundValue(ChangeEvent<bool> evt)
    {
        evt.StopPropagation();
        _gameData.bgm = evt.newValue;
        SoundManager.Instance.Play(AudioClips.Click, 1);
        SoundManager.Instance.SoundSet(evt.newValue);
        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeHapticValue(ChangeEvent<bool> evt)
    {
        evt.StopPropagation();
        _gameData.haptic = evt.newValue;
        VibrationManager.Instance.VibrationSet(evt.newValue);
        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickCloseBtn(ClickEvent evt)
    {
        MainEvents.MainViewShow?.Invoke();
        // 창 변경 됬다는거 uimanager한테 안알려줬음 주의 할 것
    }
    private void ClickMoneyBtn(ClickEvent evt) {
        CoinManager.Instance.Coin += 1000;
    }

    #endregion
    private void GameDataLoad(GameData data)
    {
        if (data == null)
        {
            return;
        }
        _gameData = data;

        _soundToggle.value = _gameData.bgm;
        _hapticToggle.value = _gameData.haptic;
        SoundManager.Instance.SoundSet(_gameData.bgm);
        VibrationManager.Instance.VibrationSet(_gameData.haptic);

        SettingEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
