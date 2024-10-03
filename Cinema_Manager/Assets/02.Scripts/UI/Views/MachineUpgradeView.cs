using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

[Serializable]
public class MachinepgradeView : UIView {
    private Button closeBtn;

    private Button upgradeProductionSpeedBtn;
    private Button upgradeVolumeBtn;
    private Button upgradeStorageBtn;

    private List<VisualElement> productionSpeedGaugeList;
    private List<VisualElement> volumeGaugeList;
    private List<VisualElement> storageGaugeList;

    public MachinepgradeView(VisualElement topElement) : base(topElement) {

    }
    public override void Dispose() {
        base.Dispose();
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

        upgradeProductionSpeedBtn.RegisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeStorageBtn.RegisterCallback<ClickEvent>(ClickSellingCostBtn);
    }
    protected override void UnRegisterButtonCallbacks() {
        base.UnRegisterButtonCallbacks();

        upgradeProductionSpeedBtn.UnregisterCallback<ClickEvent>(ClickUpgradeMoveSpeedBtn);
        upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        upgradeStorageBtn.UnregisterCallback<ClickEvent>(ClickSellingCostBtn);
    }

    #region Handle
    private void ClickUpgradeMoveSpeedBtn(ClickEvent evt) {
        MachineUpgradeEvents.UpgradeProductionSpeedEvent?.Invoke();
        foreach(VisualElement gauge in productionSpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                return;
            }
        }
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        MachineUpgradeEvents.UpgradekVolumeEvent?.Invoke();
        foreach (VisualElement gauge in volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                return;
            }
        }
    }
    private void ClickSellingCostBtn(ClickEvent evt) {
        MachineUpgradeEvents.UpgradeStorageEvent?.Invoke();
        foreach (VisualElement gauge in storageGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                return;
            }
        }
    }

    private void ClickCloseBtn(ClickEvent evt) {
        MainEvents.MainViewShow?.Invoke();
    }
    #endregion
}
