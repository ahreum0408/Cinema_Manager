using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkit {
    public class PackageMachineUpgradeView : UIView {
        private Button closeBtn;

        private Button upgradePackingSpeedBtn;
        private Button upgradeVolumeBtn;

        private List<VisualElement> packingSpeedGaugeList;
        private List<VisualElement> volumeGaugeList;

        public PackageMachineUpgradeView(VisualElement topElement) : base(topElement) {
            PackageMachineUpgradeEvents.GameDataLoadEvent += GameDataLoad;
        }
        public override void Dispose() {
            base.Dispose();
            PackageMachineUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
        }
        public override void Show() {
            base.Show();
            MainEvents.ShowViewEvent?.Invoke();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
            closeBtn = topElement.Q<Button>("close-btn");

            var upgradeProductionSpeedContent = topElement.Q<VisualElement>("upgrade-packingspeed-content");
            var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");

            packingSpeedGaugeList = upgradeProductionSpeedContent.Query<VisualElement>(name: "gauge").ToList();
            volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name: "gauge").ToList();

            upgradePackingSpeedBtn = upgradeProductionSpeedContent.Q<Button>("upgrade-btn");
            upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();

            closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

            upgradePackingSpeedBtn.RegisterCallback<ClickEvent>(ClickPackingSpeedBtn);
            upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);

            upgradePackingSpeedBtn.UnregisterCallback<ClickEvent>(ClickPackingSpeedBtn);
            upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        }
        #region registercallback
        private void ClickCloseBtn(ClickEvent evt) {
            MainEvents.MainViewShow?.Invoke();
        }

        private void ClickPackingSpeedBtn(ClickEvent evt) {
            foreach (VisualElement gauge in packingSpeedGaugeList) {
                if (gauge.ClassListContains("off")) {
                    gauge.RemoveFromClassList("off");
                    _gameData.mp_packingspeedLevel++;
                    PackageMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                    return;
                }
            }
        }

        private void ClickVolumeBtn(ClickEvent evt) {
            foreach (VisualElement gauge in volumeGaugeList) {
                if (gauge.ClassListContains("off")) {
                    gauge.RemoveFromClassList("off");
                    _gameData.mp_volumeLevel++;
                    PackageMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                    return;
                }
            }
        }
        #endregion

        private void GameDataLoad(GameData data) {
            if (data == null) {
                return;
            }
            _gameData = data;
            // gaugeÄÑ±â
            for (int i = 4; i >= 0; i--) {
                if (_gameData.mp_packingspeedLevel > i) {
                    packingSpeedGaugeList[i].RemoveFromClassList("off");
                }
                if (_gameData.mp_volumeLevel > i) {
                    volumeGaugeList[i].RemoveFromClassList("off");
                }
            }
        }
    }
}
