#if UNITY_EDITOR

using Cysharp.Threading.Tasks;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base
{
    public partial class DUIBehaviour
    {
        #region ================================ METHODS

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 70)]
        [Button("Fade In", ButtonSizes.Medium)]
        [ShowIf("@EditorApplication.isPlaying && _canvasGroup != null && _canvasGroup.alpha == 0")]
        private void EditorFadeIn()
        {
            FadeIn(_fadeDuration).Forget();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 70)]
        [Button("Fade Out", ButtonSizes.Medium)]
        [ShowIf("@EditorApplication.isPlaying && _canvasGroup != null && _canvasGroup.alpha == 1")]
        private void EditorFadeOut()
        {
            FadeOut(_fadeDuration).Forget();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 100)]
        [Button("@_hideButtonTitle", ButtonSizes.Medium)]
        [HideIf("@_canvasGroup != null && _canvasGroup.alpha == 0")]
        private void EditorHide()
        {
            Hide();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 100)]
        [Button("@_showButtonTitle", ButtonSizes.Medium)]
        [ShowIf("@_canvasGroup != null && _canvasGroup.alpha == 0")]
        private void EditorShow()
        {
            Show();
        }

        #endregion

        [SerializeField] [HideInInspector]
        // ReSharper disable NotAccessedField.Global
        private protected string _hideButtonTitle = "Hide";
        [SerializeField] [HideInInspector]
        private protected string _showButtonTitle = "Show";
        // ReSharper restore NotAccessedField.Global
    }
}

#endif