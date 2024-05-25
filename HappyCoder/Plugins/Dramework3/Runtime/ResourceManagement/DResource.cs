#if DRAMEWORK_LOG
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;
#endif
using System;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.ResourceManagement
{
    [Serializable]
    internal partial class DResource : IResource
    {
        #region ================================ FIELDS

        private const string LogPrefix = "[DResource.LoadAsync()]";
        private const string GameObjectType = "GameObject";
        private const string ScriptableObjectType = "ScriptableObject";
        private const string MaterialType = "Material";
        private const string TextureType = "Texture2D";
        private const string SpriteType = "Sprite";
        private const string SpriteAtlasType = "SpriteAtlas";
        private const string AudioClipType = "AudioClip";
        private const string AudioMixerControllerType = "AudioMixerController";

        private const int LabelWidth = 100;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LabelWidth)] [LabelText("Scene ID:")]
        [SerializeField] [ReadOnly]
        private string _sceneID;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LabelWidth)] [LabelText("Group ID:")]
        [SerializeField] [ReadOnly]
        private string _groupID;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LabelWidth)] [LabelText("ID:")]
        [SerializeField] [ReadOnly]
        private string _id;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LabelWidth)] [LabelText("Asset Type:")]
        [SerializeField] [ReadOnly]
        private string _assetType;

        [Indent] [FoldoutGroup("@Title")] [HorizontalGroup("@Title/Assets Reference")]
        [LabelWidth(LabelWidth)] [LabelText("Asset:")]
        [SerializeField] [ReadOnly]
        private AssetReference _assetReference;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LabelWidth)] [LabelText("Load At Start:")]
        [SerializeField] [ReadOnly]
        private bool _loadAtStart;

        [SerializeField] [HideInInspector]
        private string _title;

        private AsyncOperationHandle _handle;
        private Type _type;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal string GroupID => _groupID;
        internal string ID => _id;
        internal bool IsLoadAtStart => _loadAtStart;
        internal string SceneID => _sceneID;
        internal string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title) == false) return _title;
                _title = $"{_sceneID}{_groupID}{_id}";
                return _title;
            }
        }

        #endregion

        #region ================================ METHODS

        public T GetAsset<T>() where T : Object
        {
            if (typeof(Component).IsAssignableFrom(typeof(T)))
                return ((GameObject)_handle.Result).GetComponent<T>();

            return (T)_handle.Result;
        }

        public async UniTask<T> InstantiateAsync<T>(bool autoRelease = false) where T : Object
        {
            if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                var go = Object.Instantiate(await LoadAsync<GameObject>()).RemoveClonePostfix();
                var component = go.GetComponent<T>();
                if (autoRelease) Release();
                return component;
            }

            var obj = Object.Instantiate(await LoadAsync<T>()).RemoveClonePostfix();
            if (autoRelease) Release();
            return obj;
        }

        public async UniTask<T> LoadAsync<T>() where T : Object
        {
            if (_handle.IsValid() == false)
            {
                if (typeof(Component).IsAssignableFrom(typeof(T)))
                    _handle = _assetReference.LoadAssetAsync<GameObject>();
                else
                    _handle = _assetReference.LoadAssetAsync<T>();

                DCore.LoadResourceProgress(0);
                while (_handle.PercentComplete < 1)
                {
                    DCore.LoadResourceProgress(_handle.PercentComplete);
                    await UniTask.Yield();
                }
                DCore.LoadResourceProgress(1);

#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Asset «{Title}» is loaded", LogPrefix);
#endif
            }

            if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                return ((GameObject)_handle.Result).GetComponent<T>();
            }

            return (T)_handle.Result;
        }

        public void Release()
        {
            if (_handle.IsValid() == false) return;
            Addressables.Release(_handle);
        }

        internal async UniTask LoadAtStartAsync()
        {
            if (_loadAtStart == false) return;

            switch (_assetType)
            {
                case GameObjectType:
                    await LoadAsync<GameObject>();
                    break;
                case ScriptableObjectType:
                    await LoadAsync<ScriptableObject>();
                    break;
                case MaterialType:
                    await LoadAsync<Material>();
                    break;
                case TextureType:
                    await LoadAsync<Texture2D>();
                    break;
                case SpriteType:
                    await LoadAsync<Sprite>();
                    break;
                case SpriteAtlasType:
                    await LoadAsync<SpriteAtlas>();
                    break;
                case AudioClipType:
                    await LoadAsync<AudioClip>();
                    break;
                case AudioMixerControllerType:
                    await LoadAsync<AudioMixer>();
                    break;
            }
        }

        #endregion
    }
}