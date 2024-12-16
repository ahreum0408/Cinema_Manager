using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace UIToolkit
{
    public enum UpgradeViewType
    {
        PlayerUpgradeView,
        EmployeeUpgradeView,
        TruckMachineUpgradeView,
        PackageMachineUpgradeView
    }
    public abstract class UIView : IDisposable
    {
        protected bool isOverlay; // 부분 투명 여부
        protected bool hideOnAwake = true;
        protected VisualElement topContainer; // templeateContainer 말하는거임
        protected VisualElement topElement;
        protected GameData _gameData;

        public VisualElement Root => topContainer;
        public bool IsTransparent => isOverlay;
        public bool IsHidden => topContainer.style.display == DisplayStyle.None;

        public UIView(VisualElement topContainer)
        {
            // null이 아니라면 m_TopElement에 topElement넣어주고 
            this.topContainer = topContainer ?? throw new ArgumentNullException(nameof(topContainer));
            topElement = topContainer.Q<VisualElement>("background");

            Initialize();
        }

        public virtual void Initialize()
        {
            if (hideOnAwake)
            {
                //Hide();
            }
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        // 세팅
        protected virtual void SetVisualElements()
        {

        }

        // 콜백 등록 및 해제
        protected virtual void RegisterButtonCallbacks()
        {

        }
        protected virtual void UnRegisterButtonCallbacks()
        {

        }

        public virtual void Show()
        {
            topContainer.style.display = DisplayStyle.Flex;
            topElement.AddToClassList("openView");
        }

        public async virtual void Hide()
        {
            topElement.RemoveFromClassList("openView");
            await Task.Delay(200);
            topContainer.style.display = DisplayStyle.None;
        }

        // 이벤트 핸들러를 등록 해제
        public virtual void Dispose()
        {
            UnRegisterButtonCallbacks();
        }
    }
}

