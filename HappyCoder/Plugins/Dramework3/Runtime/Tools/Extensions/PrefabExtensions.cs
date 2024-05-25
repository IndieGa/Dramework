#if UNITY_EDITOR

using System.Diagnostics.CodeAnalysis;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class PrefabExtensions
    {
        #region ================================ METHODS

        public static Object GetPrefabAsset(this GameObject go)
        {
            var pathToPrefab = GetPrefabPath(go);
            return PrefabUtility.GetCorrespondingObjectFromSourceAtPath(go, pathToPrefab);
        }

        public static string GetPrefabPath(GameObject go)
        {
            return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
        }

        #endregion
    }
}
#endif