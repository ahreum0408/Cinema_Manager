using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkit {
    public class LevelUpView : UIView {
        private VisualTreeAsset _addItemAsset;

        private VisualElement _mainContent;

        private Button _closeBtn;
        private Level CurrentLevel => _gameData.level;

        public LevelUpView(VisualElement topElement) : base(topElement) {
            _addItemAsset = Resources.Load<VisualTreeAsset>("UI/Templeate/AddItem");
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
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.FoodContainer:
                        itemNameLebel.text = "음식트럭";
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.BoxContainer:
                        itemNameLebel.text = "택배트럭";
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.Table:
                        itemNameLebel.text = "테이블";
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.ParcelService:
                        itemNameLebel.text = "택배서비스";
                        itemCountLebel.text = $"{item.Value}개";
                        break;
                    case TargetType.Room:
                        itemNameLebel.text = "구역";
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
