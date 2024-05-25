using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class ScaleExtensions
    {
        #region ================================ METHODS

        public static void ScaleToScreen(this Transform objectTransform, Vector3 initialScale, Vector2 referenceSize)
        {
            Helpers.Helpers.ScaleTools.ScaleToScreen(objectTransform, initialScale, referenceSize);
        }

        #endregion
    }
}