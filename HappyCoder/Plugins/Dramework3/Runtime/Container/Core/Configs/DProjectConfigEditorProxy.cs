#if UNITY_EDITOR

using System.Collections.Generic;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    public static class DProjectConfigEditorProxy
    {
        #region ================================ PROPERTIES AND INDEXERS

        public static string ProjectRootFolder => DProjectConfig.Instance.ProjectRootFolder;
        public static IReadOnlyList<string> Scenes => DProjectConfig.Instance.Scenes;

        #endregion

        #region ================================ METHODS

        public static void ClearInstallers()
        {
            DProjectConfig.ClearInstallers();
        }

        public static void InitializeProjectSettings()
        {
            DProjectConfig.InitializeProjectSettings();
        }

        #endregion
    }
}

#endif