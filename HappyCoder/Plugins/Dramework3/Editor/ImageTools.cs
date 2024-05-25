using UnityEditor;

using UnityEngine;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Editor
{
    public static class ImageTools
    {
        #region ================================ METHODS

        [MenuItem("Tools/Happy Coder/Dramework 3/Image/Change Size With Aspect By Height", false, 160)]
        [MenuItem("GameObject/Happy Coder/D Framework 3/Image/Change Size With Aspect By Height", false, 30)]
        private static void ChangeImageSizeWithAspectByHeight()
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogError("No game objects is selected");
                return;
            }

            if (Selection.activeGameObject.activeInHierarchy == false)
            {
                Debug.LogError("Select game object on scene, not in project");
                return;
            }

            var image = Selection.activeGameObject.GetComponent<Image>();
            if (image == null)
            {
                Debug.LogError("Selected game object has no component \"Image\"");
                return;
            }

            if (image.sprite == null)
            {
                Debug.LogError("Selected image has no sprite");
                return;
            }

            var rectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
            var spriteRect = image.sprite.rect;
            var aspect = spriteRect.width / spriteRect.height;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * aspect);
            rectTransform.ForceUpdateRectTransforms();
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Image/Change Size With Aspect By Width", false, 160)]
        [MenuItem("GameObject/Happy Coder/D Framework 3/Image/Change Size With Aspect By Width", false, 31)]
        private static void ChangeImageSizeWithAspectByWidth()
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogError("No game objects is selected");
                return;
            }

            if (Selection.activeGameObject.activeInHierarchy == false)
            {
                Debug.LogError("Select game object on scene, not in project");
                return;
            }

            var image = Selection.activeGameObject.GetComponent<Image>();
            if (image == null)
            {
                Debug.LogError("Selected game object has no component \"Image\"");
                return;
            }

            if (image.sprite == null)
            {
                Debug.LogError("Selected image has no sprite");
                return;
            }

            var rectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
            var spriteRect = image.sprite.rect;
            var aspect = spriteRect.width / spriteRect.height;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.width / aspect);
            rectTransform.ForceUpdateRectTransforms();
        }

        #endregion
    }
}