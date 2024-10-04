using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

[Serializable]
public class PlayerUpgradeView : UIView {
    private Button closeBtn;

    private Button upgradeMovespeedBtn;
    private Button upgradeVolumeBtn;
    private Button upgradeSellingCostBtn;

    private List<VisualElement> moveSpeedGaugeList;
    private List<VisualElement> volumeGaugeList;
    private List<VisualElement> sellingCostGaugeList;

    public PlayerUpgradeView(VisualElement topElement) : base(topElement) {

    }
    public override void Dispose() {
        base.Dispose();
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

    #region Handle
    private void ClickUpgradeMoveSpeedBtn(ClickEvent evt) {
        PlayerUpgradeEvents.UpgradeMoveSpeedEvent?.Invoke();
        foreach(VisualElement gauge in moveSpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                return;
            }
        }
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        PlayerUpgradeEvents.UpgradekVolumeEvent?.Invoke();
        foreach (VisualElement gauge in volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                return;
            }
        }
    }
    private void ClickSellingCostBtn(ClickEvent evt) {
        PlayerUpgradeEvents.UpgradeSellingCostEvent?.Invoke();
        foreach (VisualElement gauge in sellingCostGaugeList) {
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
