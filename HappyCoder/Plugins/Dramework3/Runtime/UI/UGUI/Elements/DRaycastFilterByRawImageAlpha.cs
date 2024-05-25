using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RawImage))]
    public class DRaycastFilterByRawImageAlpha : DBehaviour, ICanvasRaycastFilter
    {
        #region ================================ FIELDS

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/COMPONENTS")]
        [SerializeField] [GetComponent]
        private RawImage _image;

        [FoldoutGroup("@Title/SETTINGS", 40)]
        [SerializeField]
        private bool _reversed;

        private Texture2D _texture;

        #endregion

        #region ================================ METHODS

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (enabled == false || _texture == null) return true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_image.rectTransform, screenPoint, eventCamera, out var local);

            var rect = _image.rectTransform.rect;
            var pivot = _image.rectTransform.pivot;
            local.x += pivot.x * rect.width;
            local.y += pivot.y * rect.height;

            var uvRect = _image.uvRect;
            var u = local.x / rect.width * uvRect.width + uvRect.x;
            var v = local.y / rect.height * uvRect.height + uvRect.y;

            var alpha = _texture.GetPixelBilinear(u, v).a;
            return _reversed ? alpha == 0 : alpha != 0;
        }

        private void Awake()
        {
            _texture = _image.texture as Texture2D;
        }

        #endregion
    }
}