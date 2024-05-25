using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Factories.Core
{
    public class DFactory
    {
        #region ================================ METHODS

        protected virtual T Create<T>() where T : class, new()
        {
            var obj = new T();
            DCore.InitializeContainerObject(obj);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return obj;
        }

        protected virtual T CreateAndBind<T>(string containerID) where T : class, new()
        {
            var obj = new T();
            DCore.InitializeContainerObject(obj);
            DCore.Bind(obj, containerID, string.Empty);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return obj;
        }

        protected virtual T CreateAndBind<T>(string containerID, string instanceID) where T : class, new()
        {
            var obj = new T();
            DCore.InitializeContainerObject(obj);
            DCore.Bind(obj, containerID, instanceID);
            ConsoleLogger.Log($"Object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return obj;
        }

        protected virtual T Instantiate<T>(GameObject prefab, Transform parent, bool worldPositionStays = true) where T : Component
        {
            var go = Object.Instantiate(prefab, parent, worldPositionStays);

            var obj = go.GetComponent<T>();
            if (obj == null)
                obj = go.AddComponent<T>();

            foreach (var monoBehaviour in go.GetComponentsInChildren<MonoBehaviour>(true))
            {
                DCore.InitializeContainerObject(monoBehaviour);
            }

            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return obj;
        }

        protected virtual T InstantiateAndBind<T>(GameObject prefab, Transform parent, string containerID, bool worldPositionStays = true) where T : Component
        {
            var go = Object.Instantiate(prefab, parent, worldPositionStays);

            var obj = go.GetComponent<T>();
            if (obj == null)
                obj = go.AddComponent<T>();

            DCore.Bind(obj, containerID, string.Empty);

            foreach (var monoBehaviour in go.GetComponentsInChildren<MonoBehaviour>(true))
            {
                DCore.InitializeContainerObject(monoBehaviour);
            }

            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return obj;
        }

        protected virtual T InstantiateAndBind<T>(GameObject prefab, Transform parent, string containerID, string instanceID, bool worldPositionStays = true) where T : Component
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

            ConsoleLogger.Log($"Game object «{go.name}» is instantiated and object type of «{typeof(T)}» is created", $"[{GetType()}.{nameof(Instantiate)}()]");
            return obj;
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

        #endregion
    }
}