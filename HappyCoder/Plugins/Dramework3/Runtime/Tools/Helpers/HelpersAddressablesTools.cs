#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Core;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static partial class AddressablesTools
        {
            #region ================================ METHODS

            public static bool AddressablesContainsKey(object key)
            {
                // ReSharper disable once UnusedVariable
                return Addressables.ResourceLocators.Any(l => l.Locate(key, null, out var locations));
            }

            public static void ClearCache()
            {
                ClearDependencyCache().Forget();
            }

            public static async UniTask DownloadAllDependencies(Action<float> onProgress, bool force = false)
            {
                foreach (var resourceLocator in Addressables.ResourceLocators)
                {
                    foreach (var key in resourceLocator.Keys)
                    {
                        await DownloadAssetDependencies(key, onProgress, force);
                    }
                }
            }

            public static async UniTask DownloadAssetDependencies(object key, Action<float> onProgress, bool force = false)
            {
#if UNITY_EDITOR
                if (AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilderIndex == 0 && AddressablesContainsKey(key) == false)
                    return;
#endif

                if (force == false)
                {
                    long totalDownloadSize = 0;
                    var sizeAsync = Addressables.GetDownloadSizeAsync(key);
                    await sizeAsync;

                    if (sizeAsync.Status == AsyncOperationStatus.Succeeded)
                        totalDownloadSize = sizeAsync.Result;

                    if (totalDownloadSize == 0)
                        return;
                }

                Debug.Log($"Asset {key} has a new version. It's dependencies will updated");
                var downloadAsync = Addressables.DownloadDependenciesAsync(key);
                while (downloadAsync.IsDone == false)
                {
                    var percent = downloadAsync.PercentComplete;
                    onProgress?.Invoke(percent);
                    Debug.Log($"Asset {key}: loaded {percent * 100} %");
                    await UniTask.Yield();
                }
                Debug.Log($"Download asset {key} dependencies is complete");
            }

            public static async UniTask Initialize()
            {
                await Addressables.InitializeAsync();
            }

            public static async UniTask<T> LoadAssetAsync<T>(string key, Action<float> onProgress, Action<Exception> onFailed) where T : class
            {
// #if UNITY_EDITOR
//                 if (AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilderIndex == 0 && AddressablesContainsKey(key) == false)
//                     return null;
// #endif

                var handle = Addressables.LoadAssetAsync<T>(key);
                onProgress?.Invoke(0);
                while (handle.IsDone == false)
                {
                    onProgress?.Invoke(handle.PercentComplete);
                    await UniTask.Yield();
                }
                onProgress?.Invoke(1);
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    return handle.Result;

                onFailed?.Invoke(handle.OperationException);
                return null;
            }

            public static async UniTask<IEnumerable<T>> LoadAssetsAsync<T>(string key, Action<float> onProgress, Action<Exception> onFailed)
            {
// #if UNITY_EDITOR
//                 if (AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilderIndex == 0 && AddressablesContainsKey(key) == false)
//                     return null;
// #endif

                var handle = Addressables.LoadAssetsAsync<T>(key, null);
                onProgress?.Invoke(0);
                while (handle.IsDone == false)
                {
                    onProgress?.Invoke(handle.PercentComplete);
                    await UniTask.Yield();
                }
                onProgress?.Invoke(1);
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    return handle.Result;

                onFailed?.Invoke(handle.OperationException);
                return null;
            }

            public static async UniTask<IReadOnlyCollection<AsyncOperationHandle<T>>> LoadAsync<T>(IReadOnlyCollection<AssetReferenceT<T>> references, int batchSize) where T : Object
            {
                if (batchSize <= 1)
                    return await LoadAsync(references);

                var handles = new List<AsyncOperationHandle<T>>();
                var total = references.Count;
                var complete = 0f;
                batchSize = Mathf.Max(1, batchSize);
                var queue = new Queue<AssetReferenceT<T>>(references);

                DCore.LoadResourceProgress(0);

                while (queue.Count > 0)
                {
                    var batch = new List<AssetReferenceT<T>>();
                    for (var i = 0; i < batchSize && queue.Count > 0; i++)
                        batch.Add(queue.Dequeue());

                    var uniTasks = new List<UniTask>();

                    foreach (var spriteReference in batch)
                    {
                        var handle = spriteReference.LoadAssetAsync<T>();
                        uniTasks.Add(handle.ToUniTask());
                        handles.Add(handle);
                    }

                    await UniTask.WhenAll(uniTasks.ToArray());

                    complete += batch.Count;
                    DCore.LoadResourceProgress(complete / total);
                }

                DCore.LoadResourceProgress(1);

                return handles;
            }

            public static async UniTask<IReadOnlyCollection<AsyncOperationHandle<T>>> LoadAsync<T>(IReadOnlyCollection<AssetReferenceT<T>> references) where T : Object
            {
                var handles = new List<AsyncOperationHandle<T>>();
                var total = references.Count;
                var complete = 0f;

                DCore.LoadResourceProgress(0);

                foreach (var reference in references)
                {
                    var handle = reference.LoadAssetAsync<T>();
                    handles.Add(handle);
                    await handle;
                    ++complete;
                    DCore.LoadResourceProgress(complete / total);
                }

                DCore.LoadResourceProgress(1);

                return handles;
            }

            public static async UniTask RemoveAssetFromCache(object key)
            {
#if UNITY_EDITOR
                if (AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilderIndex == 0 && AddressablesContainsKey(key) == false)
                    return;
#endif

                var async = Addressables.LoadResourceLocationsAsync(key);
                await async;

                if (async.Status == AsyncOperationStatus.Succeeded)
                {
                    var resourceLocations = async.Result;
                    foreach (var resourceLocation in resourceLocations)
                        Addressables.ClearDependencyCacheAsync(resourceLocation.PrimaryKey);
                }
            }

            public static async UniTask UpdateCatalog()
            {
                var catalogsToUpdate = new List<string>();
                var checkCatalogHandle = Addressables.CheckForCatalogUpdates(false);
                await checkCatalogHandle;

                if (checkCatalogHandle.Status == AsyncOperationStatus.Succeeded)
                    catalogsToUpdate = checkCatalogHandle.Result;

                Addressables.Release(checkCatalogHandle);

                Debug.Log($"Catalogs to update count: {catalogsToUpdate.Count}");

                if (catalogsToUpdate.Count > 0)
                {
                    var updateCatalogHandle = Addressables.UpdateCatalogs(catalogsToUpdate, false);
                    await updateCatalogHandle;
                }
            }

#pragma warning disable CS1998
            private static async UniTaskVoid ClearDependencyCache()
#pragma warning restore CS1998
            {
#if !UNITY_WEBGL
                foreach (var resourceLocator in Addressables.ResourceLocators)
                {
                    var async = Addressables.ClearDependencyCacheAsync(resourceLocator.Keys, false);
                    await async;
                    Addressables.Release(async);
                }
                await Addressables.CleanBundleCache();
                Caching.ClearCache();

                Debug.Log("Cache is cleared");

#endif
            }

            #endregion
        }

        #endregion
    }
}