using System;
using System.Collections.Generic;

using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.ResourceManagement;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core
{
    public abstract class DInstaller
    {
        #region ================================ METHODS

        protected static T GetAsset<T>(Type type, string sceneName, string groupID, string resourceID) where T : Object
        {
            if (DProjectConfig.TryGetResource(sceneName, groupID, resourceID, out var resource)) return resource.GetAsset<T>();
            ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not found", $"[{nameof(DInstaller)}.{nameof(GetAsset)}]");
            return default;
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
            if (DProjectConfig.TryGetResource(sceneName, groupID, resourceID, out var resource)) return resource;
            ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not found", $"[{nameof(DInstaller)}.{nameof(GetResource)}]");
            return null;
        }

        protected static Dictionary<string, IResource> GetResourceGroup(Type type, string sceneName, string groupID)
        {
            if (DProjectConfig.TryGetResourceGroup(sceneName, groupID, out var resourceGroup) == false)
            {
                ConsoleLogger.LogError($"The resource group «{groupID}» for object «{type}» is not found", $"[{nameof(DInstaller)}.{nameof(GetResourceGroup)}]");
                return null;
            }

            return resourceGroup;
        }

        protected static T InstantiateAsset<T>(Type type, string sceneName, string groupID, string resourceID, bool active) where T : Component
        {
            if (DProjectConfig.TryGetResource(sceneName, groupID, resourceID, out var resource) == false)
            {
                ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not found", $"[{nameof(DInstaller)}.{nameof(GetAsset)}]");
                return null;
            }

            if (((DResource)resource).IsLoadAtStart == false)
            {
                ConsoleLogger.LogError($"The resource «{resourceID}» for object «{type}» is not marked as «LoadAtStart» and was not loaded", $"[{nameof(DInstaller)}.{nameof(InstantiateAsset)}]");
                return null;
            }

            var go = Object.Instantiate(resource.GetAsset<GameObject>()).RemoveClonePostfix();
            go.SetActive(active);
            return (T)go.GetComponent(typeof(T));
        }

        public abstract DInstallData[] CreateFactories();
        public abstract DInstallData[] CreateModels();
        public abstract DInstallData[] CreateSystems();

        #endregion
    }
}