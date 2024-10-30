using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

[Serializable]
public class EmployeeUpgradeView : UIView {
    private Button closeBtn;

    private Button upgradeMovespeedBtn;
    private Button upgradeVolumeBtn;
    private Button upgradeEmploymentBtn;

    private List<VisualElement> moveSpeedGaugeList;
    private List<VisualElement> volumeGaugeList;
    private List<VisualElement> employmentGaugeList;

    private GameData _gameData;

    public EmployeeUpgradeView(VisualElement topElement) : base(topElement) {
        EmployeeUpgradeEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose() {
        base.Dispose();
        EmployeeUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
    }

    public override void Show() {
        base.Show();
        MainEvents.ShowViewEvent?.Invoke();
    }

    protected override void SetVisualElements() {
        base.SetVisualElements();

        closeBtn = topElement.Q<Button>("close-btn");

        var upgradeMoveSpeedContent = topElement.Q<VisualElement>("upgrade-movespeed-content");
        var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");
        var upgradeEmploymentContent = topElement.Q<VisualElement>("upgrade-employment-content");

        moveSpeedGaugeList = upgradeMoveSpeedContent.Query<VisualElement>(name : "gauge").ToList();
        volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name : "gauge").ToList();
        employmentGaugeList = upgradeEmploymentContent.Query<VisualElement>(name : "gauge").ToList();

        upgradeMovespeedBtn = upgradeMoveSpeedContent.Q<Button>("upgrade-btn");
        upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        upgradeEmploymentBtn = upgradeEmploymentContent.Q<Button>("upgrade-btn");
    }

    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

        upgradeMovespeedBtn.RegisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeEmploymentBtn.RegisterCallback<ClickEvent>(ClickEmploymentBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

        upgradeMovespeedBtn.UnregisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeEmploymentBtn.UnregisterCallback<ClickEvent>(ClickEmploymentBtn);
    }

    #region registercallback
    private void ClickUpgradeMoveSpeedBtn(ClickEvent evt) {
        foreach(VisualElement gauge in moveSpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.e_movespeedLevel++;
                return;
            }
        }
        EmployeeUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        foreach (VisualElement gauge in volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.e_volumeLevel++;
                return;
            }
        }
        EmployeeUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickEmploymentBtn(ClickEvent evt) {
        foreach (VisualElement gauge in employmentGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.e_employmentLevel++;
                return;
            }
        }
        EmployeeUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }

    private void ClickCloseBtn(ClickEvent evt) {
        MainEvents.MainViewShow?.Invoke();
    }
    #endregion

    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        // gaugeÄÑ±â
        for (int i = 4; i >= 0; i--) {
            if (_gameData.e_movespeedLevel > i) {
                moveSpeedGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.e_volumeLevel > i) {
                volumeGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.e_employmentLevel > i) {
                employmentGaugeList[i].RemoveFromClassList("off");
            }
        }
    }
}
