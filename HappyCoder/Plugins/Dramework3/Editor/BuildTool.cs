#if UNITY_EDITOR

using System.IO;
using System.Linq;
using System.Threading.Tasks;

using UnityEditor;
using UnityEditor.Build;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Editor
{
    public class BuildTool : ScriptableObject
    {
        #region ================================ FIELDS

        [SerializeField]
        private BuildTargetEnum _buildTarget;
        [SerializeField]
        private BuildOptions _buildOptions;
        [SerializeField]
        private ScriptingImplementation _scriptingBackend;
        [SerializeField]
        private bool _buildAppBundle;
        [SerializeField]
        private int _majorVersion;
        [SerializeField]
        private int _minorVersion = 1;
        [SerializeField]
        private int _patchVersion;
        [SerializeField]
        private string _buildPath;

        private bool _isBuilding;

        #endregion

        #region ================================ METHODS

        [MenuItem("Tools/Happy Coder/Dramework 3/Builds/Build Tool", false, 200)]
        private static void ShowBuildTool()
        {
            const string directory = "Assets/Plugins/Dramework 3/Editor";
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
                AssetDatabase.Refresh();
            }

            var fullPath = Path.Combine(directory, "Build Tool.asset");
            if (File.Exists(fullPath) == false)
            {
                var instance = CreateInstance<BuildTool>();
                AssetDatabase.CreateAsset(instance, fullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            var buildTool = AssetDatabase.LoadAssetAtPath<BuildTool>(fullPath);
            // var version = PlayerSettings.bundleVersion.Split('.');
            // if (version.Length > 0 && int.TryParse(version[0], out var major))
            //     buildTool._majorVersion = major;
            //
            // if (version.Length > 1 && int.TryParse(version[1], out var minor))
            //     buildTool._minorVersion = minor;
            //
            // if (version.Length > 2 && int.TryParse(version[2], out var patch))
            //     buildTool._patchVersion = patch;
            Selection.activeObject = buildTool;
        }

        internal async void Build()
        {
            if (_isBuilding) return;
            _isBuilding = true;

            switch (_buildTarget)
            {
                case BuildTargetEnum.StandaloneWindows:
                    if (await PrepareBuild())
                        BuildStandaloneWindows();
                    break;

                case BuildTargetEnum.Android:
                    if (await PrepareBuild())
                        BuildAndroid();
                    break;
            }
        }

        private async void BuildAndroid()
        {
            var levels = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
            var development = _buildOptions.HasFlag(BuildOptions.Development) ? "development" : "release";
            var filename = $"{PlayerSettings.productName}_{development}_{PlayerSettings.bundleVersion}.apk".ToLower();
            var fullPath = Path.Combine(_buildPath, filename);
            BuildPipeline.BuildPlayer(levels, fullPath, BuildTarget.Android, _buildOptions);

            while (BuildPipeline.isBuildingPlayer)
                await Task.Yield();

            _isBuilding = false;
        }

        private async void BuildStandaloneWindows()
        {
            var levels = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
            var filename = $"{PlayerSettings.productName}.exe";
            var fullPath = Path.Combine(_buildPath, $"{PlayerSettings.productName}_Version_{PlayerSettings.bundleVersion}", filename);
            BuildPipeline.BuildPlayer(levels, fullPath, BuildTarget.StandaloneWindows, _buildOptions);

            while (BuildPipeline.isBuildingPlayer)
                await Task.Yield();

            _isBuilding = false;
        }

        private async Task<bool> PrepareBuild()
        {
            var buildPath = EditorUtility.SaveFolderPanel("Choose Location of Built Project", $"{_buildPath}", "");
            if (string.IsNullOrEmpty(buildPath))
            {
                _isBuilding = false;
                return false;
            }

            _buildPath = buildPath;
            if (Path.GetFileName(_buildPath) != PlayerSettings.productName)
                _buildPath = Path.Combine(_buildPath, PlayerSettings.productName);

            _patchVersion++;

            PlayerSettings.bundleVersion = $"{_majorVersion}.{_minorVersion}.{_patchVersion}";
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, _scriptingBackend);
            EditorUserBuildSettings.buildAppBundle = _buildAppBundle;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            await Task.Delay(100);
            return true;
        }

        #endregion

        #region ================================ NESTED TYPES

        private enum BuildTargetEnum
        {
            Android,
            StandaloneWindows
        }

        #endregion
    }
}

#endif