using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class DRaycastFilterByTargetImage : DBehaviour, ICanvasRaycastFilter
    {
        #region ================================ FIELDS

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/COMPONENTS")]
        [SerializeField] [GetComponent]
        private Image _image;

        [FoldoutGroup("@Title/SETTINGS", 40)]
        [SerializeField]
        private bool _reversed;

        [FoldoutGroup("@Title/SETTINGS", 40)]
        [SerializeField]
        private Image _targetImage;

        [FoldoutGroup("@Title/SETTINGS", 40)]
        [SerializeField]
        private bool _targetReversed;

        private Sprite _sprite;
        private Sprite _targetSprite;

        #endregion

        #region ================================ METHODS

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (enabled == false || _sprite == null) return true;
            var isTargetImage = RectTransformUtility.RectangleContainsScreenPoint(_targetImage.rectTransform, screenPoint, eventCamera);
            return isTargetImage ? IsRaycastLocationValid(_targetImage, _targetSprite, _targetReversed, screenPoint, eventCamera) : IsRaycastLocationValid(_image, _sprite, _reversed, screenPoint, eventCamera);
        }

        private void Awake()
        {
            _sprite = _image.overrideSprite;
            _targetSprite = _targetImage.overrideSprite;
        }

        private bool IsRaycastLocationValid(Graphic image, Sprite sprite, bool reversed, Vector2 screenPoint, Camera eventCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, screenPoint, eventCamera, out var local);

            var rect = image.rectTransform.rect;
            var pivot = image.rectTransform.pivot;
            local.x += pivot.x * rect.width;
            local.y += pivot.y * rect.height;

            var u = local.x / rect.width;
            var v = local.y / rect.height;

            var alpha = sprite.texture.GetPixelBilinear(u, v).a;
            return reversed ? alpha == 0 : alpha != 0;
        }

        #endregion
    }
}