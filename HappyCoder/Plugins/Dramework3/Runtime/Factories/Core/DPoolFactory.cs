using System;
using System.Collections.Generic;

using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Factories.Core
{
    public class DPoolFactory
    {
        #region ================================ FIELDS

        private readonly Dictionary<Type, List<DPoolFactoryItem>> _objects = new Dictionary<Type, List<DPoolFactoryItem>>();
        private readonly Dictionary<Type, List<DPoolFactoryItemComponent>> _components = new Dictionary<Type, List<DPoolFactoryItemComponent>>();

        #endregion

        #region ================================ METHODS

        protected virtual T Create<T>() where T : DPoolFactoryItem, new()
        {
            var type = typeof(T);

            if (_objects.TryGetValue(type, out var pool) == false) return CreateItem<T>(type, null);

            for (var i = 0; i < pool.Count; i++)
            {
                var item = pool[i];
                if (item.Active) continue;
                item.Activate();
                return (T)item;
            }

            return CreateItem<T>(type, pool);
        }

        protected virtual T CreateAndBind<T>(string containerID) where T : DPoolFactoryItem, new()
        {
            var item = Create<T>();
            DCore.Bind(item, containerID, string.Empty);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return item;
        }

        protected virtual T CreateAndBind<T>(string containerID, string instanceID) where T : DPoolFactoryItem, new()
        {
            var item = Create<T>();
            DCore.Bind(item, containerID, instanceID);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return item;
        }

        protected virtual T Instantiate<T>(GameObject prefab, Transform parent, bool worldPositionStays = true) where T : DPoolFactoryItemComponent
        {
            var type = typeof(T);

            if (_components.TryGetValue(type, out var pool) == false)
                return InstantiateItem<T>(type, null, prefab, parent, worldPositionStays);

            for (var i = 0; i < pool.Count; i++)
            {
                var item = pool[i];
                if (item.Active) continue;
                item.Activate();
                return (T)item;
            }

            return InstantiateItem<T>(type, pool, prefab, parent, worldPositionStays);
        }

        protected virtual T InstantiateAndBind<T>(GameObject prefab, Transform parent, string containerID, bool worldPositionStays = true) where T : DPoolFactoryItemComponent
        {
            var item = Instantiate<T>(prefab, parent, worldPositionStays);
            DCore.Bind(item, containerID, string.Empty);
            return item;
        }

        protected virtual T InstantiateAndBind<T>(GameObject prefab, Transform parent, string containerID, string instanceID, bool worldPositionStays = true) where T : DPoolFactoryItemComponent
        {
            var item = Instantiate<T>(prefab, parent, worldPositionStays);
            DCore.Bind(item, containerID, instanceID);
            return item;
        }

        protected void Log(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.Log(message.ToString(), prefix, sender);
        }

        protected void LogError(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogError(message.ToString(), prefix, sender);
        }

        protected void LogWarning(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogWarning(message.ToString(), prefix, sender);
        }

        private T CreateItem<T>(Type type, List<DPoolFactoryItem> currentPool) where T : DPoolFactoryItem, new()
        {
            var item = new T();
            DCore.InitializeContainerObject(item);

            if (currentPool == null)
            {
                currentPool = new List<DPoolFactoryItem>();
                _objects.Add(type, currentPool);
            }

            item.Activate();
            currentPool.Add(item);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(CreateItem)}()]");
            return item;
        }

        private T InstantiateItem<T>(Type type, List<DPoolFactoryItemComponent> currentPool, GameObject prefab, Transform parent, bool worldPositionStays) where T : DPoolFactoryItemComponent
        {
            var go = Object.Instantiate(prefab, parent, worldPositionStays);

            var item = go.GetComponent<T>();
            if (item == null)
                item = go.AddComponent<T>();

            DCore.InitializeContainerObject(item);

            if (currentPool == null)
            {
                currentPool = new List<DPoolFactoryItemComponent>();
                _components.Add(type, currentPool);
            }

            item.Activate();
            currentPool.Add(item);
            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and component type of «{typeof(T)}» is added", $"[{GetType()}.{nameof(InstantiateItem)}()]");
            return item;
        }

        #endregion
    }
}