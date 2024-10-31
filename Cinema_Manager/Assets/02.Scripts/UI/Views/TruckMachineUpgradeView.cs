using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

[Serializable]
public class TruckMachineUpgradeView : UIView {
    private Button closeBtn;

    private Button upgradeDeliverySpeedBtn;
    private Button upgradeVolumeBtn;
    private Button upgradeStorageBtn;

    private List<VisualElement> deliverySpeedGaugeList;
    private List<VisualElement> volumeGaugeList;
    private List<VisualElement> storageGaugeList;

    public TruckMachineUpgradeView(VisualElement topElement) : base(topElement) {
        TruckMachineUpgradeEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose() {
        base.Dispose();
        TruckMachineUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
    }
    public override void Show() {
        base.Show();
        MainEvents.ShowViewEvent?.Invoke();
    }

    protected override void SetVisualElements() {
        base.SetVisualElements();

        closeBtn = topElement.Q<Button>("close-btn");

        var upgradeProductionSpeedContent = topElement.Q<VisualElement>("upgrade-deliveryspeed-content");
        var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");
        var upgradeStorageContent = topElement.Q<VisualElement>("upgrade-storage-content");

        deliverySpeedGaugeList = upgradeProductionSpeedContent.Query<VisualElement>(name : "gauge").ToList();
        volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name : "gauge").ToList();
        storageGaugeList = upgradeStorageContent.Query<VisualElement>(name : "gauge").ToList();

        upgradeDeliverySpeedBtn = upgradeProductionSpeedContent.Q<Button>("upgrade-btn");
        upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        upgradeStorageBtn = upgradeStorageContent.Q<Button>("upgrade-btn");
    }

    protected override void RegisterButtonCallbacks() {
        base.RegisterButtonCallbacks();

        closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

        upgradeDeliverySpeedBtn.RegisterCallback<ClickEvent>(ClickProductionSpeedBtn);
        upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeStorageBtn.RegisterCallback<ClickEvent>(ClickStorageBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

        upgradeDeliverySpeedBtn.UnregisterCallback<ClickEvent>(ClickProductionSpeedBtn);
        upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeStorageBtn.UnregisterCallback<ClickEvent>(ClickStorageBtn);
    }

    #region registercallback
    private void ClickProductionSpeedBtn(ClickEvent evt) {
        foreach(VisualElement gauge in deliverySpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.mt_productionspeedLevel++;
                return;
            }
        }
        TruckMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        foreach (VisualElement gauge in volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.e_volumeLevel++;
                return;
            }
        }
        TruckMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ClickStorageBtn(ClickEvent evt) {
        foreach (VisualElement gauge in storageGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                _gameData.mt_storageLevel++;
                return;
            }
        }
        TruckMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
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
            if (_gameData.mt_productionspeedLevel > i) {
                deliverySpeedGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.mt_volumeLevel > i) {
                volumeGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.mt_storageLevel > i) {
                storageGaugeList[i].RemoveFromClassList("off");
            }
        }
        //MachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
