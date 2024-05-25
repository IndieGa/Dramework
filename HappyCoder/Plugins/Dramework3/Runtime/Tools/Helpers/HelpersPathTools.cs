using System;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class PathTools
        {
            #region ================================ PROPERTIES AND INDEXERS

            public static string ProjectPath => Directory.GetParent(Application.dataPath)?.FullName;

#if UNITY_EDITOR
            public static string[] ScenePaths => (from buildSettingsScene in EditorBuildSettings.scenes where buildSettingsScene.enabled select buildSettingsScene.path).ToArray();
#endif

            #endregion

            #region ================================ METHODS

            public static void DeleteDirectory(string path)
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }

            public static string GetRelativePath(string absolutePath)
            {
                return absolutePath.Remove(0, absolutePath.IndexOf("Assets", StringComparison.Ordinal));
            }

            #endregion
        }

        #endregion
    }
}