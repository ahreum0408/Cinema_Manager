using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine.UIElements;

[Serializable]
public class TruckMachineUpgradeView : UIView
{
    private Button _closeBtn;

    private Button _upgradeDeliverySpeedBtn;
    private Button _upgradeVolumeBtn;
    private Button _upgradeStorageBtn;

    private List<VisualElement> _deliverySpeedGaugeList;
    private List<VisualElement> _volumeGaugeList;
    private List<VisualElement> _storageGaugeList;

    public TruckMachineUpgradeView(VisualElement topElement) : base(topElement)
    {
        TruckMachineUpgradeEvents.GameDataLoadEvent += GameDataLoad;
    }
    public override void Dispose()
    {
        base.Dispose();
        TruckMachineUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
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

        var upgradeProductionSpeedContent = topElement.Q<VisualElement>("upgrade-deliveryspeed-content");
        var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");
        var upgradeStorageContent = topElement.Q<VisualElement>("upgrade-storage-content");

        _deliverySpeedGaugeList = upgradeProductionSpeedContent.Query<VisualElement>(name: "gauge").ToList();
        _volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name: "gauge").ToList();
        _storageGaugeList = upgradeStorageContent.Query<VisualElement>(name: "gauge").ToList();

        _upgradeDeliverySpeedBtn = upgradeProductionSpeedContent.Q<Button>("upgrade-btn");
        _upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        _upgradeStorageBtn = upgradeStorageContent.Q<Button>("upgrade-btn");
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

        _upgradeDeliverySpeedBtn.RegisterCallback<ClickEvent>(ClickProductionSpeedBtn);
        _upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        _upgradeStorageBtn.RegisterCallback<ClickEvent>(ClickStorageBtn);
    }
    protected override void UnRegisterButtonCallbacks()
    {
        base.UnRegisterButtonCallbacks();

        _upgradeDeliverySpeedBtn.UnregisterCallback<ClickEvent>(ClickProductionSpeedBtn);
        _upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        _upgradeStorageBtn.UnregisterCallback<ClickEvent>(ClickStorageBtn);
    }

    #region registercallback
    private void ClickProductionSpeedBtn(ClickEvent evt)
    {
        foreach (VisualElement gauge in _deliverySpeedGaugeList)
        {
            if (gauge.ClassListContains("off"))
            {
                gauge.RemoveFromClassList("off");
                _gameData.mt_productionspeedLevel++;
                TruckMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
    }
    private void ClickVolumeBtn(ClickEvent evt)
    {
        foreach (VisualElement gauge in _volumeGaugeList)
        {
            if (gauge.ClassListContains("off"))
            {
                gauge.RemoveFromClassList("off");
                _gameData.e_volumeLevel++;
                TruckMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                return;
            }
        }
    }
    private void ClickStorageBtn(ClickEvent evt)
    {
        foreach (VisualElement gauge in _storageGaugeList)
        {
            if (gauge.ClassListContains("off"))
            {
                gauge.RemoveFromClassList("off");
                _gameData.mt_storageLevel++;
                TruckMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
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

        // gaugeÄÑ±â
        for (int i = 4; i >= 0; i--)
        {
            if (_gameData.mt_productionspeedLevel > i)
            {
                _deliverySpeedGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.mt_volumeLevel > i)
            {
                _volumeGaugeList[i].RemoveFromClassList("off");
            }
            if (_gameData.mt_storageLevel > i)
            {
                _storageGaugeList[i].RemoveFromClassList("off");
            }
        }
        //MachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
