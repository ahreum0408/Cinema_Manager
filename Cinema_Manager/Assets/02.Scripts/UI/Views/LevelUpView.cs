using UnityEditor.iOS;
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
        }
        public override void Dispose() {
            base.Dispose();
            LevelUpEvents.GameDataLoadEvent -= GameDataLoad;
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
        }

        private void GameDataLoad(GameData data) {
            if (data == null) {
                return;
            }
            _gameData = data;

            SettingLevelContent();
        }
        private void SettingLevelContent() {
            int standCount = CurrentLevel.GetStandListLength();

            if(standCount > 0) {
                var addPanel = _addItemAsset.Instantiate("addItem-container");
                var icon = addPanel.Q<VisualElement>("icon");
                var itemNameLebel = addPanel.Q<Label>("item-name");
                var itemCountLebel = addPanel.Q<Label>("item-count");

                itemNameLebel.text = "Stand";
                itemCountLebel.text = standCount.ToString();
                _mainContent.Add(addPanel);
            }

        }
    }
}
