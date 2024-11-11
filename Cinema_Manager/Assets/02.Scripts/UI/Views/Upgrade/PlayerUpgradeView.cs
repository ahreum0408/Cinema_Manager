using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class PlayerUpgradeView : UIView {
    private Button _closeBtn;

    private Button _upgradeMovespeedBtn;
    private Button _upgradeVolumeBtn;
    private Button _upgradeSellingCostBtn;

    private List<VisualElement> _moveSpeedGaugeList;
    private List<VisualElement> _volumeGaugeList;
    private List<VisualElement> _sellingCostGaugeList;

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

        _closeBtn = topElement.Q<Button>("close-btn");

        var upgradeMoveSpeedContent = topElement.Q<VisualElement>("upgrade-movespeed-content");
        var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");
        var upgradeEmploymentContent = topElement.Q<VisualElement>("upgrade-sellingcost-content");

        _moveSpeedGaugeList = upgradeMoveSpeedContent.Query<VisualElement>(name : "gauge").ToList();
        _volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name : "gauge").ToList();
        _sellingCostGaugeList = upgradeEmploymentContent.Query<VisualElement>(name : "gauge").ToList();

        _upgradeMovespeedBtn = upgradeMoveSpeedContent.Q<Button>("upgrade-btn");
        _upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        _upgradeSellingCostBtn = upgradeEmploymentContent.Q<Button>("upgrade-btn");
    }
    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

        _upgradeMovespeedBtn.RegisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        _upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        _upgradeSellingCostBtn.RegisterCallback<ClickEvent>(ClickSellingCostBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

        _upgradeMovespeedBtn.UnregisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        _upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        _upgradeSellingCostBtn.UnregisterCallback<ClickEvent>(ClickSellingCostBtn);
    }

    #region registercallback
    private void ClickUpgradeMoveSpeedBtn(ClickEvent evt) {
        foreach(VisualElement gauge in _moveSpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.p_movespeedLevel++;
                PlayerUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
        Debug.Log("change data");
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        foreach (VisualElement gauge in _volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.p_volumeLevel++;
                PlayerUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
    }
    private void ClickSellingCostBtn(ClickEvent evt) {
        foreach (VisualElement gauge in _sellingCostGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.p_sellingcostLevel++;
                PlayerUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
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
                _moveSpeedGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.p_volumeLevel > i) {
                _volumeGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.p_sellingcostLevel > i) {
                _sellingCostGaugeList[i].RemoveFromClassList("off");
            }
        }

        //PlayerUpgradeEvents.GameDataUpdatEvent.Invoke(_gameData);
    }
}
