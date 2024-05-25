using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Helpers
{
    [ExecuteAlways]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DSafeArea : DBehaviour
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 100;

        [FoldoutGroup("Components")] [BoxGroup("Components/Canvas", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Canvas:")]
        [SerializeField] [GetComponentInParent(ignoreName: true)] [ReadOnly]
        private Canvas _canvas;

        [FoldoutGroup("Components")] [BoxGroup("Components/Rect Transform", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Rect Transform:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private RectTransform _rectTransform;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Auto Change In Editor", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Change In Edit Mode:")]
        [SerializeField]
        private bool _changeInEditMode;

        #endregion

        #region ================================ METHODS

        public void ApplySafeArea()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying == false && _changeInEditMode == false) return;
#endif
            SafeArea();
        }

        private void Awake()
        {
            ApplySafeArea();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)]
        [HorizontalGroup("Editor Tools/Box/Buttons", MinWidth = 60)] [PropertyOrder(-2)]
        [Button(ButtonSizes.Medium)]
        private void Default()
        {
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.sizeDelta = Vector2.zero;
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)]
        [HorizontalGroup("Editor Tools/Box/Buttons", MinWidth = 80)] [PropertyOrder(-1)]
        [Button(ButtonSizes.Medium)]
        private void SafeArea()
        {
            if (_rectTransform == null) return;
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.sizeDelta = Vector2.zero;

            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            var pixelRect = _canvas.pixelRect;
            anchorMin.x = Mathf.Clamp01(anchorMin.x / pixelRect.width);
            anchorMin.y = Mathf.Clamp01(anchorMin.y / pixelRect.height);
            anchorMax.x = Mathf.Clamp01(anchorMax.x / pixelRect.width);
            anchorMax.y = Mathf.Clamp01(anchorMax.y / pixelRect.height);
            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }

        #endregion
    }
}