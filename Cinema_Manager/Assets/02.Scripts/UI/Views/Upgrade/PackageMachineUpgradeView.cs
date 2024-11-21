using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIToolkit
{
    public class PackageMachineUpgradeView : UIView
    {
        private Button _closeBtn;

        private Button _upgradePackingSpeedBtn;
        private Button _upgradeVolumeBtn;

        private List<VisualElement> _packingSpeedGaugeList;
        private List<VisualElement> _volumeGaugeList;

        public PackageMachineUpgradeView(VisualElement topElement) : base(topElement)
        {
            PackageMachineUpgradeEvents.GameDataLoadEvent += GameDataLoad;
        }
        public override void Dispose()
        {
            base.Dispose();
            PackageMachineUpgradeEvents.GameDataLoadEvent -= GameDataLoad;
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

            var upgradeProductionSpeedContent = topElement.Q<VisualElement>("upgrade-packingspeed-content");
            var upgradeVolumeContent = topElement.Q<VisualElement>("upgrade-volume-content");

            _packingSpeedGaugeList = upgradeProductionSpeedContent.Query<VisualElement>(name: "gauge").ToList();
            _volumeGaugeList = upgradeVolumeContent.Query<VisualElement>(name: "gauge").ToList();

            _upgradePackingSpeedBtn = upgradeProductionSpeedContent.Q<Button>("upgrade-btn");
            _upgradeVolumeBtn = upgradeVolumeContent.Q<Button>("upgrade-btn");
        }

        protected override void RegisterButtonCallbacks()
        {
            base.RegisterButtonCallbacks();

            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);

            _upgradePackingSpeedBtn.RegisterCallback<ClickEvent>(ClickPackingSpeedBtn);
            _upgradeVolumeBtn.RegisterCallback<ClickEvent>(ClickVolumeBtn);
        }
        protected override void UnRegisterButtonCallbacks()
        {
            base.UnRegisterButtonCallbacks();
            _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);

            _upgradePackingSpeedBtn.UnregisterCallback<ClickEvent>(ClickPackingSpeedBtn);
            _upgradeVolumeBtn.UnregisterCallback<ClickEvent>(ClickVolumeBtn);
        }
        #region registercallback
        private void ClickCloseBtn(ClickEvent evt)
        {
            MainEvents.MainViewShow?.Invoke();
        }

        private void ClickPackingSpeedBtn(ClickEvent evt)
        {
            foreach (VisualElement gauge in _packingSpeedGaugeList)
            {
                if (gauge.ClassListContains("off"))
                {
                    gauge.RemoveFromClassList("off");
                    _gameData.mp_packingspeedLevel++;
                    PackageMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
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
                    _gameData.mp_volumeLevel++;
                    PackageMachineUpgradeEvents.GameDataUpdatEvent?.Invoke(_gameData);
                    return;
                }
            }
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
                if (_gameData.mp_packingspeedLevel > i)
                {
                    _packingSpeedGaugeList[i].RemoveFromClassList("off");
                }
                if (_gameData.mp_volumeLevel > i)
                {
                    _volumeGaugeList[i].RemoveFromClassList("off");
                }
            }
        }
    }
}
