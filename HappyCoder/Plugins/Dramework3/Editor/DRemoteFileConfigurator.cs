using System;
using System.Collections;
using System.IO;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Editor
{
    [HideMonoScript]
    public abstract class DRemoteFileConfigurator : ScriptableObject
    {
        #region ================================ FIELDS

        private const string STREAMING_ASSETS_FOLDER = "Assets/StreamingAssets";

        [BoxGroup("@Title", centerLabel: true)] [FoldoutGroup("@Title/File Settings")]
        [BoxGroup("@Title/File Settings/Scene", false)]
        [LabelText("Scene:")]
        [SerializeField] [ValueDropdown("SceneNames")]
        private string _scene;

        [BoxGroup("@Title", centerLabel: true)] [FoldoutGroup("@Title/File Settings")]
        [BoxGroup("@Title/File Settings/Filename", false)]
        [LabelText("Filename:")]
        [SerializeField]
        private string _filename;

        private string _title;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private string FilePath => $"{STREAMING_ASSETS_FOLDER}/@{_scene}@{_filename}.cfg";
        private IEnumerable SceneNames
        {
            get
            {
                var scenePaths = new ValueDropdownList<string>();
                foreach (var sceneAsset in Helpers.EditorTools.LoadAssets<SceneAsset>(string.Empty, new[] { DProjectConfigEditorProxy.ProjectRootFolder }))
                    scenePaths.Add(sceneAsset.name, sceneAsset.name);

                return scenePaths;
            }
        }
        private string Title => string.IsNullOrEmpty(_title) ? "CONFIGURATOR" : _title;

        #endregion

        #region ================================ METHODS

        protected abstract byte[] OnExport();
        protected abstract void OnImport(byte[] bytes);
        protected abstract void OnReset();

        protected void SetTitle(string title)
        {
            _title = title;
        }

        private async UniTask DeferredSave()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        [BoxGroup("@Title", centerLabel: true)] [BoxGroup("@Title/Menu", false, order: -1000)] [HorizontalGroup("@Title/Menu/Horizontal")]
        [Button("Export", ButtonSizes.Medium)] [PropertyOrder(1)]
        private void Export()
        {
            try
            {
                if (string.IsNullOrEmpty(_scene))
                {
                    Debug.LogError("[RemoteFileConfigurator]. Scene is null or empty", this);
                    return;
                }

                if (string.IsNullOrEmpty(_filename))
                {
                    Debug.LogError("[RemoteFileConfigurator]. Filename is null or empty", this);
                    return;
                }

                if (Directory.Exists(STREAMING_ASSETS_FOLDER) == false)
                    Directory.CreateDirectory(STREAMING_ASSETS_FOLDER);

                File.WriteAllBytes(FilePath, OnExport());

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        [BoxGroup("@Title", centerLabel: true)] [BoxGroup("@Title/Menu", false, order: -1000)] [HorizontalGroup("@Title/Menu/Horizontal")]
        [Button("Import", ButtonSizes.Medium)] [PropertyOrder(0)]
        private void Import()
        {
            try
            {
                if (Directory.Exists(STREAMING_ASSETS_FOLDER) == false || File.Exists(FilePath) == false) return;

                OnImport(File.ReadAllBytes(FilePath));

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        [BoxGroup("@Title", centerLabel: true)] [BoxGroup("@Title/Menu", false, order: -1000)] [HorizontalGroup("@Title/Menu/Horizontal")]
        [Button("Reset", ButtonSizes.Medium)] [PropertyOrder(3)]
        private void ResetConfig()
        {
            OnReset();
        }

        [BoxGroup("@Title", centerLabel: true)] [BoxGroup("@Title/Menu", false, order: -1000)] [HorizontalGroup("@Title/Menu/Horizontal")]
        [Button("Save", ButtonSizes.Medium)] [PropertyOrder(2)]
        private void Save()
        {
            DeferredSave().Forget();
        }

        [BoxGroup("@Title", centerLabel: true)] [BoxGroup("@Title/Menu", false, order: -1000)] [HorizontalGroup("@Title/Menu/Horizontal")]
        [Button("Select", ButtonSizes.Medium)] [PropertyOrder(4)]
        private void Select()
        {
            if (File.Exists(FilePath) == false) return;
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(FilePath);
        }

        #endregion
    }
}