using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [HideMonoScript]
    [RequireComponent(typeof(Image))]
    public class DImage : DUIBehaviour
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")] [BoxGroup("Components/Image", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Image:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private Image _image;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public Color color
        {
            get => _image.color;
            set => _image.color = value;
        }

        public Sprite sprite
        {
            get => _image.sprite;
            set => _image.sprite = value;
        }

        #endregion

        #region ================================ METHODS

#if UNITY_EDITOR

        protected void OnValidate()
        {
            _hideButtonTitle = "Hide Image";
            _showButtonTitle = "Show Image";
        }

#endif

        #endregion
    }
}