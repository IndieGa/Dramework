using System;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [HideMonoScript]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    [SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeEvident")]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class DButtonNoLabel : DUIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")] [BoxGroup("Components/Button", false)]
        [LabelWidth(ConstantValues.Int_140)]
        [SerializeField] [GetComponent] [ReadOnly]
        private Button _button;

        [FoldoutGroup("Components")] [BoxGroup("Components/Image", false)]
        [LabelWidth(ConstantValues.Int_140)]
        [SerializeField] [GetComponent] [ReadOnly]
        private Image _image;

        #endregion

        #region ================================ EVENTS AND DELEGATES

        public event Action onClick;
        public event Action<PointerEventData> onClickWithParam;
        public event Action onDown;
        public event Action<PointerEventData> onDownWithParam;
        public event Action onEnter;
        public event Action<PointerEventData> onEnterWithParam;
        public event Action onExit;
        public event Action<PointerEventData> onExitWithParam;
        public event Action onUp;
        public event Action<PointerEventData> onUpWithParam;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public bool interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }

        public Sprite sprite
        {
            get => _image.sprite;
            set => _image.sprite = value;
        }

        private Color color
        {
            get => _image.color;
            set => _image.color = value;
        }

        #endregion

        #region ================================ METHODS

#if UNITY_EDITOR

        protected void OnValidate()
        {
            _hideButtonTitle = "Hide Button";
            _showButtonTitle = "Show Button";
        }

#endif

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (_button.interactable == false) return;
            onClick?.Invoke();
            onClickWithParam?.Invoke(eventData);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (_button.interactable == false) return;
            onDown?.Invoke();
            onDownWithParam?.Invoke(eventData);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (_button.interactable == false) return;
            onEnter?.Invoke();
            onEnterWithParam?.Invoke(eventData);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (_button.interactable == false) return;
            onExit?.Invoke();
            onExitWithParam?.Invoke(eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (_button.interactable == false) return;
            onUp?.Invoke();
            onUpWithParam?.Invoke(eventData);
        }

        private void SetSprite(Sprite newSprite)
        {
            _image.sprite = newSprite;
        }

        #endregion
    }
}