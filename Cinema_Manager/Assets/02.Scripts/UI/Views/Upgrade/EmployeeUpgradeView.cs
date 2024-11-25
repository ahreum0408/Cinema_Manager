using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class EmployeeUpgradeView : UIView
{
    private Button _closeBtn;

    private Button _upgradeMovespeedBtn;
    private Button _upgradeVolumeBtn;
    private Button _upgradeEmploymentBtn;

    private Label _moveSpeedUpgradePrice;
    private Label _volumeUpgradePrice;
    private Label _employmentUpgradePrice;

    private List<VisualElement> _moveSpeedGaugeList;
    private List<VisualElement> _volumeGaugeList;
    private List<VisualElement> _employmentGaugeList;

    public EmployeeUpgradeView(VisualElement topElement) : base(topElement)
    {
        EmployeeUpgradeEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose()
    {
        base.Dispose();
        EmployeeUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
    }

    public override void Show()
    {
        base.Show();
        MainEvents.ShowViewEvent?.Invoke();
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        _closeBtn = topElement.Q<Button>("close-btn");

        var upgradeMoveSpeedContent = topElement.Q<VisualElement>("upgrade-movespeed-content");
        var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");
        var upgradeEmploymentContent = topElement.Q<VisualElement>("upgrade-employment-content");

        _moveSpeedGaugeList = upgradeMoveSpeedContent.Query<VisualElement>(name: "gauge").ToList();
        _volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name: "gauge").ToList();
        _employmentGaugeList = upgradeEmploymentContent.Query<VisualElement>(name: "gauge").ToList();

        _upgradeMovespeedBtn = upgradeMoveSpeedContent.Q<Button>("upgrade-btn");
        _upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        _upgradeEmploymentBtn = upgradeEmploymentContent.Q<Button>("upgrade-btn");

        _moveSpeedUpgradePrice = upgradeMoveSpeedContent.Q<Label>("movespeed-upgrade-price");
        _volumeUpgradePrice = upgradeVolumeContent.Q<Label>("volume-upgrade-price");
        _employmentUpgradePrice = upgradeEmploymentContent.Q<Label>("employment-upgrade-price");
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

        _upgradeMovespeedBtn.RegisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        _upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        _upgradeEmploymentBtn.RegisterCallback<ClickEvent>(ClickEmploymentBtn);
    }
    protected override void UnRegisterButtonCallbacks()
    {
        base.UnRegisterButtonCallbacks();

        _upgradeMovespeedBtn.UnregisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        _upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        _upgradeEmploymentBtn.UnregisterCallback<ClickEvent>(ClickEmploymentBtn);
    }

    #region registercallback
    private void ClickUpgradeMoveSpeedBtn(ClickEvent evt)
    {
        foreach (VisualElement gauge in _moveSpeedGaugeList)
        {
            if (gauge.ClassListContains("off") && UpgradeManager.Instance.CanUpgrade(UpgradeTarget.employeeMoveSpeedStat))
            {
                Debug.Log("in");
                gauge.RemoveFromClassList("off");
                _gameData.e_movespeedLevel++;

                if (_gameData.e_movespeedLevel > 4) {
                    _moveSpeedUpgradePrice.text = "Max";
                }
                else {
                    int moveSpeedPrice = UpgradeManager.Instance.GetEmployeeUpgradePrice(_gameData.e_movespeedLevel);
                    _moveSpeedUpgradePrice.text = moveSpeedPrice.ToString();
                }

                UpgradeManager.Instance.FindDataAndCalculate(UpgradeTarget.employeeMoveSpeedStat);
                EmployeeUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
    }
    private void ClickVolumeBtn(ClickEvent evt)
    {
        foreach (VisualElement gauge in _volumeGaugeList)
        {
            if (gauge.ClassListContains("off") && UpgradeManager.Instance.CanUpgrade(UpgradeTarget.employeeVolumeVolumeStat))
            {
                gauge.RemoveFromClassList("off");
                _gameData.e_volumeLevel++;

                if (_gameData.e_volumeLevel > 4) {
                    _volumeUpgradePrice.text = "Max";
                }
                else {
                    int volumePrice = UpgradeManager.Instance.GetEmployeeUpgradePrice(_gameData.e_volumeLevel);
                    _volumeUpgradePrice.text = volumePrice.ToString();
                }

                UpgradeManager.Instance.FindDataAndCalculate(UpgradeTarget.employeeVolumeVolumeStat);
                EmployeeUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
    }
    private void ClickEmploymentBtn(ClickEvent evt)
    {
        foreach (VisualElement gauge in _employmentGaugeList)
        {
            if (gauge.ClassListContains("off") && UpgradeManager.Instance.CanUpgrade(UpgradeTarget.employeeAddStat))
            {
                gauge.RemoveFromClassList("off");
                _gameData.e_employmentLevel++;

                if (_gameData.e_volumeLevel > 4){
                    _employmentUpgradePrice.text = "Max";
                }
                else {
                    int employmentPrice = UpgradeManager.Instance.GetEmployeeUpgradePrice(_gameData.e_employmentLevel);
                    _employmentUpgradePrice.text = employmentPrice.ToString();
                }

                UpgradeManager.Instance.FindDataAndCalculate(UpgradeTarget.employeeAddStat);
                EmployeeUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
    }

    private void ClickCloseBtn(ClickEvent evt)
    {
        MainEvents.MainViewShow?.Invoke();
    }
    #endregion

    private void GameDataLoad(GameData data)
    {
        if (data == null)
        {
            return;
        }
        _gameData = data;
        SettingPrice();

        // gaugeÄÑ±â
        for (int i = 4; i >= 0; i--)
        {
            if (_gameData.e_movespeedLevel > i)
            {
                _moveSpeedGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.e_volumeLevel > i)
            {
                _volumeGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.e_employmentLevel > i)
            {
                _employmentGaugeList[i].RemoveFromClassList("off");
            }
        }
    }
    private void SettingPrice() {
        if (_gameData.e_movespeedLevel > 4) {
            _moveSpeedUpgradePrice.text = "Max";
        }
        else {
            int moveSpeedPrice = UpgradeManager.Instance.GetEmployeeUpgradePrice(_gameData.e_movespeedLevel);
            _moveSpeedUpgradePrice.text = moveSpeedPrice.ToString();
        }

        if (_gameData.e_volumeLevel > 4) {
            _volumeUpgradePrice.text = "Max";
        }
        else {
            int volumePrice = UpgradeManager.Instance.GetEmployeeUpgradePrice(_gameData.e_volumeLevel);
            _volumeUpgradePrice.text = volumePrice.ToString();
        }

        if (_gameData.e_employmentLevel > 4) {
            _employmentUpgradePrice.text = "Max";
        }
        else {
            int employmentPrice = UpgradeManager.Instance.GetEmployeeUpgradePrice(_gameData.e_employmentLevel);
            _employmentUpgradePrice.text = employmentPrice.ToString();
        }
    }
}
