using System;
using System.IO;
using System.Linq;

using UnityEditor;


namespace IG.HappyCoder.Dramework3.Editor
{
    public static class PackageTools
    {
        #region ================================ METHODS

        [MenuItem("Tools/Happy Coder/Dramework 3/Packages/Export", false, 120)]
        private static void ExportPackages()
        {
            var pathFrom = EditorUtility.OpenFolderPanel("Export from", "Assets", "");
            if (string.IsNullOrEmpty(pathFrom)) return;

            var pathTo = EditorUtility.OpenFolderPanel("Save to", "Assets", "");
            if (string.IsNullOrEmpty(pathFrom)) return;

            var guids = AssetDatabase.FindAssets("t:prefab", new[] { pathFrom.Remove(0, pathFrom.IndexOf("Assets/", StringComparison.Ordinal)) });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            foreach (var path in paths)
            {
                var filename = $"{Path.GetFileNameWithoutExtension(path)}.unitypackage";
                var savePath = Path.Combine(pathTo, filename);
                AssetDatabase.ExportPackage(path, savePath, ExportPackageOptions.IncludeDependencies);
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Packages/Import", false, 120)]
        private static void ImportPackages()
        {
            var pathFrom = EditorUtility.OpenFolderPanel("Import packages", "Assets", "");
            if (string.IsNullOrEmpty(pathFrom)) return;

            var guids = AssetDatabase.FindAssets("", new[] { pathFrom.Remove(0, pathFrom.IndexOf("Assets/", StringComparison.Ordinal)) });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            foreach (var path in paths)
            {
                if (Path.GetExtension(path) != ".unitypackage") continue;
                AssetDatabase.ImportPackage(path, false);
            }
            AssetDatabase.Refresh();
        }

        #endregion
    }
}