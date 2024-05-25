using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.ResourceManagement;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Factories
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DContainerFactory
    {
        #region ================================ FIELDS

        protected static DContainerFactory Instance;

        private readonly Dictionary<string, DContainerFactoryData[]> _container = new Dictionary<string, DContainerFactoryData[]>();

        #endregion

        #region ================================ METHODS

        protected static T GetAsset<T>(Type type, string sceneName, string groupID, string resourceID) where T : Object
        {
            if (DProjectConfig.TryGetResource(sceneName, groupID, resourceID, out var resource) == false)
            {
                ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not found", $"[{nameof(DContainerFactory)}.{nameof(GetAsset)}]");
                return default;
            }

            return resource.GetAsset<T>();
        }

        protected static T GetObject<T>(string instanceID = null)
        {
            return (T)DCore.GetContainerObject(typeof(T), instanceID);
        }

        protected static byte[] GetRemoteFile(string filename)
        {
            return DProjectConfig.GetRemoteFile(filename);
        }

        protected static IResource GetResource(Type type, string sceneName, string groupID, string resourceID)
        {
            if (DProjectConfig.TryGetResource(sceneName, groupID, resourceID, out var resource) == false)
            {
                ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not found", $"[{nameof(DContainerFactory)}.{nameof(GetResource)}]");
                return null;
            }

            return resource;
        }

        protected static Dictionary<string, IResource> GetResourceGroup(Type type, string sceneName, string groupID)
        {
            if (DProjectConfig.TryGetResourceGroup(sceneName, groupID, out var resourceGroup) == false)
            {
                ConsoleLogger.LogError($"The resource group «{groupID}» for object «{type}» is not found", $"[{nameof(DContainerFactory)}.{nameof(GetResourceGroup)}]");
                return null;
            }

            return resourceGroup;
        }

        protected static void Initialize(Dictionary<string, DContainerFactoryData[]> objectsData)
        {
            Instance ??= new DContainerFactory();

            foreach (var data in objectsData)
            {
                if (Instance._container.TryGetValue(data.Key, out var value) == false)
                    Instance._container.Add(data.Key, Array.Empty<DContainerFactoryData>());

                var hashSet = value?.ToHashSet() ?? Array.Empty<DContainerFactoryData>().ToHashSet();
                foreach (var func in data.Value)
                    hashSet.Add(func);

                Instance._container[data.Key] = hashSet.OrderBy(d => d.CreationOrder).ToArray();
            }
        }

        protected static T InstantiateAsset<T>(Type type, string sceneName, string groupID, string resourceID, bool active) where T : Component
        {
            if (DProjectConfig.TryGetResource(sceneName, groupID, resourceID, out var resource) == false)
            {
                ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not found", $"[{nameof(DContainerFactory)}.{nameof(InstantiateAsset)}]");
                return null;
            }

            if (((DResource)resource).IsLoadAtStart == false)
            {
                ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not marked as «LoadAtStart» and was not loaded", $"[{nameof(DContainerFactory)}.{nameof(InstantiateAsset)}]");
                return null;
            }

            var go = Object.Instantiate(resource.GetAsset<GameObject>()).RemoveClonePostfix();
            go.SetActive(active);
            return (T)go.GetComponent(typeof(T));
        }

        internal static IEnumerable<DContainerFactoryObject> Create(string sceneID)
        {
            return Instance._container.TryGetValue(sceneID, out var data)
                ? data.Select(d => d.FactoryObject.Invoke())
                : Array.Empty<DContainerFactoryObject>();
        }

        internal static void Dispose()
        {
            Instance = null;
        }

        #endregion
    }
}