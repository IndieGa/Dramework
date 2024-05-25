using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Initialization;
using IG.HappyCoder.Dramework3.Runtime.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.ResourceManagement
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class AssetSet<T> : DConfig, IInitializable, IDisposable where T : Object
    {
        #region ================================ FIELDS

        [Indent] [FoldoutGroup("Set")]
        [LabelWidth(ConstantValues.Int_150)] [LabelText("Path:")]
        [SerializeField] [FolderPath] [OnValueChanged("InitializeAssets")]
        private string _path;

        [Indent] [FoldoutGroup("Set")]
        [LabelWidth(ConstantValues.Int_150)] [LabelText("Initialize On Start:")]
        [SerializeField]
        private bool _initializeOnStart;

        [Indent] [FoldoutGroup("Set")]
        [LabelWidth(ConstantValues.Int_150)] [LabelText("Initializing Batch Size:")]
        [SerializeField]
        private int _initializingBatchSize;

        [Indent] [FoldoutGroup("Set")]
        [LabelWidth(ConstantValues.Int_150)] [LabelText("Addressables Group:")]
        [SerializeField]
        private string _addressablesGroup;

        [Indent] [FoldoutGroup("Set")] [HorizontalGroup("Set/Horizontal")]
        [LabelWidth(ConstantValues.Int_150)] [LabelText("Assets:")]
        [SerializeField] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)] [ReadOnly]
        private AssetReferenceT<T>[] _references;

        // ReSharper disable once CollectionNeverUpdated.Local
        private IReadOnlyCollection<AsyncOperationHandle<T>> _handles;
        private Dictionary<string, T> _assets;

        private bool _isInitializing;
        private bool _isInitialized;

        #endregion

        #region ================================ METHODS

        public async UniTask Initialize()
        {
            if (_isInitializing) return;

            if (_isInitialized)
            {
                ConsoleLogger.Log("Asset set is initialized", name, this);
                return;
            }

            _isInitializing = true;

            _handles = await Helpers.AddressablesTools.LoadAsync(_references, _initializingBatchSize);

            _assets = new Dictionary<string, T>();
            foreach (var handle in _handles)
                _assets.Add(handle.Result.name, handle.Result);

            _isInitialized = true;
            _isInitializing = false;

            ConsoleLogger.Log("Asset set is initialized", name, this);
        }

        public bool TryGetAsset(string assetName, out T asset)
        {
            if (_assets.TryGetValue(assetName, out asset))
                return true;

            ConsoleLogger.LogError($"Asset {assetName} is not found", name, this);
            return false;
        }

        void IDisposable.Dispose()
        {
            ReleaseHandles();
            _isInitialized = false;
        }

        async UniTask IInitializable.OnInitialize()
        {
            if (_initializeOnStart == false) return;
            await Initialize();
        }

        private void ReleaseHandles()
        {
            if (_handles == null) return;

            foreach (var handle in _handles)
            {
                if (handle.IsValid() == false) continue;
                Addressables.Release(handle);
            }

            ConsoleLogger.Log("Handles are released", name, this);
        }

        #endregion

#if UNITY_EDITOR

        [FoldoutGroup("Set")] [HorizontalGroup("Set/Horizontal", 40)]
        [Button("Init", ButtonHeight = 24)]
        private void InitializeAssets()
        {
            if (string.IsNullOrEmpty(_path))
            {
                Debug.LogError("Path is null or empty");
                return;
            }

            if (string.IsNullOrEmpty(_addressablesGroup))
            {
                Debug.LogError("Addressables group is null or empty");
                return;
            }

            RemoveAssetsFromAddressables();
            AddAssets();

            _initializingBatchSize = _references.Length;
        }

        private void RemoveAssetsFromAddressables()
        {
            foreach (var asset in _references.Select(r => r.editorAsset))
            {
                var group = Helpers.AddressablesTools.GetGroup(asset);
                if (group == null) continue;
                Helpers.AddressablesTools.RemoveAssetEntry(asset);
            }
        }

        private void AddAssets()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { _path });
            _references = guids.Select(guid => new AssetReferenceT<T>(guid)).ToArray();

            foreach (var reference in _references)
            {
                if (reference.editorAsset == null || Helpers.AddressablesTools.IsAssetAddressable(reference.editorAsset)) continue;
                Helpers.AddressablesTools.CreateAssetEntry(reference.editorAsset, _addressablesGroup);
            }
        }

        private void OnValidate()
        {
            if (this == null) return;
            _initializingBatchSize = Mathf.Clamp(_initializingBatchSize, 1, _references.Length);
            _isInitializing = false;
        }

#endif
    }
}