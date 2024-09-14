using UnityEngine.UIElements;
using System;

namespace UIToolkit {
    public abstract class UIView : IDisposable {
        protected bool isOverlay; // 부분 투명 여부
        protected bool hideOnAwake = true;
        protected VisualElement topElement; // templeateContainer말하는거임

        public VisualElement Root => topElement;
        public bool IsTransparent => isOverlay;
        public bool IsHidden => topElement.style.display == DisplayStyle.None;

        public UIView(VisualElement topElement) {
            // null이 아니라면 m_TopElement에 topElement넣어주고 
            this.topElement = topElement ?? throw new ArgumentNullException(nameof(topElement));
            Initialize();
        }

        public virtual void Initialize() {
            if (hideOnAwake) {
                Hide();
            }
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        // 설정
        protected virtual void SetVisualElements() {

        }

        // 전체 콜백 등록
        protected virtual void RegisterButtonCallbacks() {

        }

        public virtual void Show() {
            topElement.style.display = DisplayStyle.Flex;
        }

        public virtual void Hide() {
            topElement.style.display = DisplayStyle.None;
        }

        // 모든 콜백 또는 이벤트 핸들러를 등록 해제
        public virtual void Dispose() {

        }
    }
}

