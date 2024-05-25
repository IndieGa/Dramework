using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.EventSystems;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    public class DJoystick : DUIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")] [BoxGroup("Components/Background", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Background:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private RectTransform _background;

        [FoldoutGroup("Components")] [BoxGroup("Components/Handle", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Handle:")]
        [SerializeField] [GetComponentInChildren] [ReadOnly]
        private RectTransform _handle;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Sensitivity", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Sensitivity:")]
        [SerializeField]
        private float _sensitivity = 1;

        private float _size;
        private Vector2 _startDelta;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public Vector2 Direction => _handle.anchoredPosition.normalized * _sensitivity;

        #endregion

        #region ================================ METHODS

        private void Awake()
        {
            _size = _background.sizeDelta.x * 0.5f;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _startDelta = eventData.position - _background.anchoredPosition;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            var current = eventData.position - _background.anchoredPosition - _startDelta;
            _handle.anchoredPosition = current.normalized * Mathf.Clamp(current.magnitude, 0, _size);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _handle.anchoredPosition = Vector2.zero;
        }

        #endregion
    }
}