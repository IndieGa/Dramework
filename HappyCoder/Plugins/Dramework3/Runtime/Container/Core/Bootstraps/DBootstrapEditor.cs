#if UNITY_EDITOR

using UnityEditor;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Bootstraps
{
    public static partial class DBootstrap
    {
        #region ================================ METHODS

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            InitializeAppTypes();
        }

        #endregion
    }
}

#endif