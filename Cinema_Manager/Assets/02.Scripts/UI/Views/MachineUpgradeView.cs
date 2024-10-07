using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

[Serializable]
public class MachineUpgradeView : UIView {
    private Button closeBtn;

    private Button upgradeProductionSpeedBtn;
    private Button upgradeVolumeBtn;
    private Button upgradeStorageBtn;

    private List<VisualElement> productionSpeedGaugeList;
    private List<VisualElement> volumeGaugeList;
    private List<VisualElement> storageGaugeList;

    private GameData _gameData;

    public MachineUpgradeView(VisualElement topElement) : base(topElement) {
        MachineUpgradeEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose() {
        base.Dispose();
        MachineUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
    }
    public override void Show() {
        base.Show();
        MachineUpgradeEvents.ShowEvent?.Invoke();
    }

    protected override void SetVisualElements() {
        base.SetVisualElements();

        closeBtn = topElement.Q<Button>("close-btn");

        var upgradeProductionSpeedContent = topElement.Q<VisualElement>("upgrade-productionspeed-content");
        var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");
        var upgradeStorageContent = topElement.Q<VisualElement>("upgrade-storage-content");

        productionSpeedGaugeList = upgradeProductionSpeedContent.Query<VisualElement>(name : "gauge").ToList();
        volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name : "gauge").ToList();
        storageGaugeList = upgradeStorageContent.Query<VisualElement>(name : "gauge").ToList();

        upgradeProductionSpeedBtn = upgradeProductionSpeedContent.Q<Button>("upgrade-btn");
        upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        upgradeStorageBtn = upgradeStorageContent.Q<Button>("upgrade-btn");
    }

    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

        upgradeProductionSpeedBtn.RegisterCallback<ClickEvent>(ClickProductionSpeedBtn);
        upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeStorageBtn.RegisterCallback<ClickEvent>(ClickStorageBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

        upgradeProductionSpeedBtn.UnregisterCallback<ClickEvent>(ClickProductionSpeedBtn);
        upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeStorageBtn.UnregisterCallback<ClickEvent>(ClickStorageBtn);
    }

    #region registercallback
    private void ClickProductionSpeedBtn(ClickEvent evt) {
        foreach(VisualElement gauge in productionSpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.m_productionspeedLevel++;
                return;
            }
        }
        MachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        foreach (VisualElement gauge in volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.e_volumeLevel++;
                return;
            }
        }
        MachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickStorageBtn(ClickEvent evt) {
        foreach (VisualElement gauge in storageGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.m_storageLevel++;
                return;
            }
        }
        MachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
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

        for (int i = 0; i < 5; i++) {
            if (_gameData.m_productionspeedLevel < i) {
                productionSpeedGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.m_volumeLevel < i) {
                volumeGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.m_storageLevel < i) {
                storageGaugeList[i].RemoveFromClassList("off");
            }
        }
        //MachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
