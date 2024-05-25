using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class DRaycastFilterByImageAlpha : DBehaviour, ICanvasRaycastFilter
    {
        #region ================================ FIELDS

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/COMPONENTS")]
        [SerializeField] [GetComponent]
        private Image _image;

        [FoldoutGroup("@Title/SETTINGS", 40)]
        [SerializeField]
        private bool _reversed;

        private Sprite _sprite;

        #endregion

        #region ================================ METHODS

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (enabled == false || _sprite == null) return true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_image.rectTransform, screenPoint, eventCamera, out var local);

            var rect = _image.rectTransform.rect;
            var pivot = _image.rectTransform.pivot;
            local.x += pivot.x * rect.width;
            local.y += pivot.y * rect.height;

            var u = local.x / rect.width;
            var v = local.y / rect.height;

            var alpha = _sprite.texture.GetPixelBilinear(u, v).a;
            return _reversed ? alpha == 0 : alpha != 0;
        }

        private void Awake()
        {
            _sprite = _image.overrideSprite;
        }

        #endregion
    }
}