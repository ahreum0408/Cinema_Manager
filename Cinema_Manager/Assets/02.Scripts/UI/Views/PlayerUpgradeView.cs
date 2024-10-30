using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class PlayerUpgradeView : UIView {
    private Button closeBtn;

    private Button upgradeMovespeedBtn;
    private Button upgradeVolumeBtn;
    private Button upgradeSellingCostBtn;

    private List<VisualElement> moveSpeedGaugeList;
    private List<VisualElement> volumeGaugeList;
    private List<VisualElement> sellingCostGaugeList;

    private GameData _gameData;

    public PlayerUpgradeView(VisualElement topElement) : base(topElement) {
        PlayerUpgradeEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose() {
        base.Dispose();
        PlayerUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
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
        var upgradeEmploymentContent = topElement.Q<VisualElement>("upgrade-sellingcost-content");

        moveSpeedGaugeList = upgradeMoveSpeedContent.Query<VisualElement>(name : "gauge").ToList();
        volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name : "gauge").ToList();
        sellingCostGaugeList = upgradeEmploymentContent.Query<VisualElement>(name : "gauge").ToList();

        upgradeMovespeedBtn = upgradeMoveSpeedContent.Q<Button>("upgrade-btn");
        upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        upgradeSellingCostBtn = upgradeEmploymentContent.Q<Button>("upgrade-btn");
    }
    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

        upgradeMovespeedBtn.RegisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeSellingCostBtn.RegisterCallback<ClickEvent>(ClickSellingCostBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

        upgradeMovespeedBtn.UnregisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeSellingCostBtn.UnregisterCallback<ClickEvent>(ClickSellingCostBtn);
    }

    #region registercallback
    private void ClickUpgradeMoveSpeedBtn(ClickEvent evt) {
        foreach(VisualElement gauge in moveSpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.p_movespeedLevel++;
                return;
            }
        }
        PlayerUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        foreach (VisualElement gauge in volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.p_volumeLevel++;
                return;
            }
        }
        PlayerUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickSellingCostBtn(ClickEvent evt) {
        foreach (VisualElement gauge in sellingCostGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.p_sellingcostLevel++;
                return;
            }
        }
        PlayerUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
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
            if (_gameData.p_movespeedLevel > i) {
                moveSpeedGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.p_volumeLevel > i) {
                volumeGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.p_sellingcostLevel > i) {
                sellingCostGaugeList[i].RemoveFromClassList("off");
            }
        }

        //PlayerUpgradeEvents.GameDataUpdatEvent.Invoke(_gameData);
    }
}
