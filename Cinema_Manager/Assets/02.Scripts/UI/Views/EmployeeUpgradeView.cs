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

    public EmployeeUpgradeView(VisualElement topElement) : base(topElement) {

    }
    public override void Dispose() {
        base.Dispose();
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

    #region Handle
    private void ClickUpgradeMoveSpeedBtn(ClickEvent evt) {
        EmployeeUpgradeEvents.UpgradeMoveSpeedEvent?.Invoke();
        foreach(VisualElement gauge in moveSpeedGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                return;
            }
        }
    }
    private void ClickVolumeBtn(ClickEvent evt) {
        EmployeeUpgradeEvents.UpgradekVolumeEvent?.Invoke();
        foreach (VisualElement gauge in volumeGaugeList) {
            if (gauge.ClassListContains("off")) {
                gauge.RemoveFromClassList("off");
                return;
            }
        }
    }
    private void ClickEmploymentBtn(ClickEvent evt) {
        EmployeeUpgradeEvents.UpgradeEmploymentEvent?.Invoke();
        foreach (VisualElement gauge in employmentGaugeList) {
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
