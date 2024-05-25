using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class CameraTools
        {
            #region ================================ METHODS

            public static float GetOrthographicSize(float sceneWidth)
            {
                var unitsPerPixel = sceneWidth / Screen.width;
                return 0.5f * unitsPerPixel * Screen.height;
            }

            public static float GetVerticalFOV(float horizontalFov)
            {
                var halfWidth = Mathf.Tan(0.5f * horizontalFov * Mathf.Deg2Rad);
                var halfHeight = halfWidth * Screen.height / Screen.width;
                return 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;
            }

            #endregion
        }

        #endregion
    }
}