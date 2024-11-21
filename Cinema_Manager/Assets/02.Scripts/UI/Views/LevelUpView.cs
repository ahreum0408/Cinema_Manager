using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkit {
    public class LevelUpView : UIView {
        private VisualTreeAsset _addItemAsset;

        private StyleBackground _boxTruckIcon;
        private StyleBackground _foodTruckIcon;
        private StyleBackground _counterStaffIcon;
        private StyleBackground _newAreaIcon;
        private StyleBackground _parcelServiceIcon;
        private StyleBackground _standIcon;
        private StyleBackground _tableIcon;

        private VisualElement _mainContent;

        private Button _closeBtn;
        private Level CurrentLevel => _gameData.level;

        public LevelUpView(VisualElement topElement) : base(topElement) {
            _addItemAsset = Resources.Load<VisualTreeAsset>("UI/Templeate/AddItem");

            Sprite boxTruckIcon = Resources.Load<Sprite>("LevelUpView/box-truck");
            Sprite foodTruckIcon = Resources.Load<Sprite>("LevelUpView/food-truck");
            Sprite counterStaffIcon = Resources.Load<Sprite>("LevelUpView/counterStaff");
            Sprite newAreaIcon = Resources.Load<Sprite>("LevelUpView/newArea");
            Sprite parcelServiceIcon = Resources.Load<Sprite>("LevelUpView/parcelService");
            Sprite standIcon = Resources.Load<Sprite>("LevelUpView/stand");
            Sprite tableIcon = Resources.Load<Sprite>("LevelUpView/table");

            _standIcon = new StyleBackground(boxTruckIcon);
            _foodTruckIcon = new StyleBackground(foodTruckIcon);
            _counterStaffIcon = new StyleBackground(counterStaffIcon);
            _newAreaIcon = new StyleBackground(newAreaIcon);
            _parcelServiceIcon = new StyleBackground(parcelServiceIcon);
            _standIcon = new StyleBackground(standIcon);
            _tableIcon = new StyleBackground(tableIcon);

            LevelUpEvents.GameDataLoadEvent += GameDataLoad;
            LevelUpEvents.LevelUpUpdate += GameDataUpdate;
        }
        public override void Dispose() {
            base.Dispose();
            LevelUpEvents.GameDataLoadEvent -= GameDataLoad;
            LevelUpEvents.LevelUpUpdate -= GameDataUpdate;
        }
        public override void Show() {
            base.Show();
            MainEvents.ShowViewEvent?.Invoke();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();

            _mainContent = topElement.Q<VisualElement>("main-container");
            _closeBtn = topElement.Q<Button>("close-btn");  
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }

        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }

        private void ClickCloseBtn(ClickEvent evt) {
            MainEvents.MainViewShow?.Invoke();
            LevelUpEvents.CloseView?.Invoke();
        }

        private void GameDataLoad(GameData data) {
            if (data == null) {
                return;
            }
            _gameData = data;
        }
        private void GameDataUpdate(GameData data) {
            if (data == null) {
                return;
            }
            _gameData = data;
            SettingLevelContent();
        }
        private void SettingLevelContent() {
            _mainContent.Clear(); // 이전 값 비워주기
            Dictionary<TargetType, int> targetDictionary = CurrentLevel.GetTargetDictionary();

            foreach (var item in targetDictionary) {
                var addPanel = _addItemAsset.Instantiate("addItem-container");
                var icon = addPanel.Q<VisualElement>("icon");
                var itemNameLebel = addPanel.Q<Label>("item-name");
                var itemCountLebel = addPanel.Q<Label>("item-count");

                switch (item.Key) {
                    case TargetType.DisplayStand:
                        itemNameLebel.text = "진열대";
                        icon.style.backgroundImage = _standIcon;
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.FoodContainer:
                        itemNameLebel.text = "음식트럭";
                        icon.style.backgroundImage = _foodTruckIcon;
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.BoxContainer:
                        itemNameLebel.text = "택배트럭";
                        icon.style.backgroundImage = _boxTruckIcon;
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.Table:
                        itemNameLebel.text = "테이블";
                        icon.style.backgroundImage = _tableIcon;
                        itemCountLebel.text = $"{item.Value}개"; 
                        break;
                    case TargetType.ParcelService:
                        itemNameLebel.text = "택배서비스";
                        icon.style.backgroundImage = _parcelServiceIcon;
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.Room:
                        itemNameLebel.text = "구역";
                        icon.style.backgroundImage =_newAreaIcon;
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.CounterStaff:
                        itemNameLebel.text = "직원";
                        icon.style.backgroundImage =_counterStaffIcon;
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    default:
                        Debug.LogWarning("올바르지 못한 형식");
                        break;
                }

                _mainContent.Add(addPanel);
            }

            MainEvents.LevelUpViewShow?.Invoke();
        }
    }
}
