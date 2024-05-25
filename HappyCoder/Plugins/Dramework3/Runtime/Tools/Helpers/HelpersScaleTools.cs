using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class ScaleTools
        {
            #region ================================ METHODS

            public static Vector3 GetAdjustedToScreenScale(Vector3 initialScale, Vector2 referenceSize)
            {
                var scaleX = Screen.width / referenceSize.x;
                var scaleY = Screen.height / referenceSize.y;
                return new Vector3(initialScale.x * scaleX, initialScale.y * scaleY, 1f);
            }

            public static void ScaleToScreen(Transform objectTransform, Vector3 initialScale, Vector2 referenceSize)
            {
                var scaleX = Screen.width / referenceSize.x;
                var scaleY = Screen.height / referenceSize.y;
                var newScale = new Vector3(initialScale.x * scaleX, initialScale.y * scaleY, 1f);
                objectTransform.localScale = newScale;
            }

            #endregion
        }

        #endregion
    }
}