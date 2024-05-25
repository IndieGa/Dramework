using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class GameObjectExtensions
    {
        #region ================================ METHODS

        public static GameObject DontDestroyOnLoad(this GameObject go)
        {
            Object.DontDestroyOnLoad(go);
            return go;
        }

        #endregion
    }
}