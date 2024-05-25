using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher;
using IG.HappyCoder.Dramework3.Runtime.Factories.Interfaces;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Factories.Core
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class DGlobalFactory : IGlobalFactory
    {
        #region ================================ FIELDS

        private readonly DDispatcher _dispatcher;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal DGlobalFactory(DDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        #endregion

        #region ================================ METHODS

        public T Create<T>() where T : class, new()
        {
            var obj = new T();
            _dispatcher.InitializeContainerObject(obj);

            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        public T CreateAndBind<T>() where T : class, new()
        {
            var obj = new T();
            _dispatcher.InitializeContainerObject(obj);
            _dispatcher.Bind(obj, string.Empty);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        public T CreateAndBind<T>(string instanceID) where T : class, new()
        {
            var obj = new T();
            _dispatcher.InitializeContainerObject(obj);
            _dispatcher.Bind(obj, instanceID);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        public T CreateAndBind<T>(string containerID, string instanceID) where T : class, new()
        {
            var obj = new T();
            _dispatcher.InitializeContainerObject(obj);
            DCore.Bind(obj, containerID, instanceID);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        public T Instantiate<T>(GameObject prefab, Transform parent, bool worldPositionStays = true) where T : Component
        {
            var go = Object.Instantiate(prefab, parent, worldPositionStays);

            var obj = go.GetComponent<T>();
            if (obj == null)
                obj = go.AddComponent<T>();

            foreach (var monoBehaviour in go.GetComponentsInChildren<MonoBehaviour>(true))
            {
                DCore.InitializeContainerObject(monoBehaviour);
            }

            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        public T InstantiateAndBind<T>(GameObject prefab, Transform parent, bool worldPositionStays = true) where T : Component
        {
            var go = Object.Instantiate(prefab, parent, worldPositionStays);

            var obj = go.GetComponent<T>();
            if (obj == null)
                obj = go.AddComponent<T>();

            _dispatcher.Bind(obj, string.Empty);

            foreach (var monoBehaviour in go.GetComponentsInChildren<MonoBehaviour>(true))
            {
                DCore.InitializeContainerObject(monoBehaviour);
            }

            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        public T InstantiateAndBind<T>(GameObject prefab, Transform parent, string instanceID, bool worldPositionStays = true) where T : Component
        {
            var go = Object.Instantiate(prefab, parent, worldPositionStays);

            var obj = go.GetComponent<T>();
            if (obj == null)
                obj = go.AddComponent<T>();

            _dispatcher.Bind(obj, instanceID);

            foreach (var monoBehaviour in go.GetComponentsInChildren<MonoBehaviour>(true))
            {
                DCore.InitializeContainerObject(monoBehaviour);
            }

            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        public T InstantiateAndBind<T>(GameObject prefab, Transform parent, string containerID = "", string instanceID = "", bool worldPositionStays = true) where T : Component
        {
            var go = Object.Instantiate(prefab, parent, worldPositionStays);

            var obj = go.GetComponent<T>();
            if (obj == null)
                obj = go.AddComponent<T>();

            DCore.Bind(obj, containerID, instanceID);

            foreach (var monoBehaviour in go.GetComponentsInChildren<MonoBehaviour>(true))
            {
                DCore.InitializeContainerObject(monoBehaviour);
            }

            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and object type of «{typeof(T)}» is created", $"[{nameof(DGlobalFactory)}.{nameof(CreateAndBind)}()]", _dispatcher);
            return obj;
        }

        #endregion
    }
}