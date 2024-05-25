using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Common;
using IG.HappyCoder.Dramework3.Runtime.Timers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Sirenix.OdinInspector;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core
{
    [HideMonoScript]
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator")]
    public partial class DCore : DBehaviour
    {
        #region ================================ FIELDS

        private const string LOG_PREFIX = "Core";
        private readonly List<DDispatcher> _dispatchers = new List<DDispatcher>();
        private readonly List<GameObject> _dontDestroyObjects = new List<GameObject>();

        private float _timer;
        private bool _locked;

        #endregion

        #region ================================ EVENTS AND DELEGATES

        public static event Action<string> onDispatcherReadyForInitialization;
        public static event Action onHardRestart;
        public static event Action<string> onInitializationIsComplete;
        public static event Action<float> onLoadResourceProgress;
        public static event Action onSoftRestart;
        internal static event Action onContinue;
        internal static event Action onDestroy;
        internal static event Action onPause;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal static int DispatchersCount => Instance._dispatchers.Count;
        internal static DCore Instance { get; private set; }
        private string LogPrefix => $"{name}";

        #endregion

        #region ================================ METHODS

        public static void Continue()
        {
            if (Instance._locked == false) return;
            Instance.OnContinue();
        }

        public static IDispatcher GetDispatcher(string sceneName)
        {
            foreach (var dispatcher in Instance._dispatchers)
            {
                if (dispatcher.SceneName != sceneName) continue;
                return dispatcher;
            }
            return null;
        }

        public static void HardRestart()
        {
            onHardRestart?.Invoke();
            onHardRestart = null;

            foreach (var rootGameObject in Instance._dontDestroyObjects)
                Destroy(rootGameObject);

            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public static void Pause()
        {
            if (Instance._locked) return;
            Instance.OnPause();
        }

        public static void SoftRestart(int sceneIndex)
        {
            onSoftRestart?.Invoke();
            onSoftRestart = null;
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }

        internal static T Bind<T>(T obj, string containerID, string instanceID)
        {
            var dispatcher = Instance._dispatchers.FirstOrDefault(d => d.SceneName == containerID);
            if (dispatcher == null)
            {
                ConsoleLogger.LogError($"Container {containerID} is not found", $"{nameof(DCore)}.Bind()", Instance);
                return obj;
            }
            dispatcher.Bind(obj, instanceID);
            return obj;
        }

        internal static void Create()
        {
            var go = new GameObject(nameof(DCore))
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            DontDestroyOnLoad(go);
            go.AddComponent<DCore>();
        }

        internal static object GetContainerObject(DDispatcher requester, Type fieldType, string instanceID)
        {
            if (Instance == null) return null;

            foreach (var dispatcher in Instance._dispatchers.Where(dispatcher => dispatcher != requester))
            {
                var obj = dispatcher.GetContainerObject(fieldType, instanceID);
                if (obj != null) return obj;
            }

            return null;
        }

        internal static object GetContainerObject(Type fieldType, string instanceID)
        {
            if (Instance == null) return null;

            foreach (var dispatcher in Instance._dispatchers)
            {
                var obj = dispatcher.GetContainerObject(fieldType, instanceID);
                if (obj != null) return obj;
            }

            return null;
        }

        internal static IEnumerable<object> GetContainerObjects(Type fieldType)
        {
            if (Instance == null) return null;
            var objects = new List<object>();
            foreach (var dispatcher in Instance._dispatchers)
                objects.AddRange(dispatcher.GetContainerObjects(fieldType));
            return objects;
        }

        internal static DDispatcher GetDispatcherInternal(string sceneName)
        {
            return Instance._dispatchers.FirstOrDefault(d => d.SceneName == sceneName);
        }

        internal static byte[] GetRemoteFile(string filename)
        {
            if (Instance == null) return null;

            foreach (var dispatcher in Instance._dispatchers)
            {
                var bytes = dispatcher.GetRemoteFile(filename);
                if (bytes != null) return bytes;
            }

            return null;
        }

        internal static void InitializationIsComplete(string sceneName)
        {
            onInitializationIsComplete?.Invoke(sceneName);
        }

        internal static void InitializeContainerObject(string sceneName, object obj)
        {
            var dispatcher = GetDispatcherInternal(sceneName);
            dispatcher.InitializeContainerObject(obj);
        }

        internal static void InitializeContainerObject(object obj)
        {
            var dispatcher = Instance._dispatchers.Last();
            dispatcher.InitializeContainerObject(obj);
        }

        internal static void LoadResourceProgress(float progress)
        {
            onLoadResourceProgress?.Invoke(progress);
        }

        internal static void PreInitializationIsComplete(string sceneName)
        {
            onDispatcherReadyForInitialization?.Invoke(sceneName);
        }

        internal static bool RegisterDispatcher(DDispatcher dispatcher)
        {
            if (Instance._dispatchers.Contains(dispatcher)) return false;

            Instance._locked = true;
            Instance._dispatchers.Add(dispatcher);
#if DRAMEWORK_LOG
            Instance.Log($"Dispatcher on scene \"{dispatcher.gameObject.scene.name}\" is registered", $"[{LOG_PREFIX}.{nameof(RegisterDispatcher)}()]", Instance);
#endif
            Instance._locked = false;
            return true;
        }

        internal static bool RegisterDontDestroyObject(GameObject dontDestroyObject)
        {
            if (Instance == null || Instance._dontDestroyObjects.Contains(dontDestroyObject)) return false;
            Instance._locked = true;
            Instance._dontDestroyObjects.Add(dontDestroyObject);
#if DRAMEWORK_LOG
            Instance.Log($"Don't destroy object \"{dontDestroyObject.name}\" is registered", $"[{LOG_PREFIX}.{nameof(RegisterDontDestroyObject)}()]", Instance);
#endif
            Instance._locked = false;
            return true;
        }

        internal static T Unbind<T>(T obj, string containerName)
        {
            var dispatcher = Instance._dispatchers.FirstOrDefault(d => d.SceneName == containerName);
            if (dispatcher == null)
            {
                ConsoleLogger.LogError($"Container {containerName} is not found", $"{nameof(DCore)}.Unbind()", Instance);
                return obj;
            }
            dispatcher.Unbind(obj);
            return obj;
        }

        internal static void UnregisterDispatcher(DDispatcher dispatcher)
        {
            if (Instance == null || Instance._dispatchers.Contains(dispatcher) == false) return;
            Instance._locked = true;
            Instance._dispatchers.Remove(dispatcher);
#if DRAMEWORK_LOG
            Instance.Log($"Dispatcher on scene \"{dispatcher.SceneName}\" is unregistered", $"[{LOG_PREFIX}.{nameof(UnregisterDispatcher)}()]", Instance);
#endif
            Instance._locked = false;
        }

        public void TimeUpdateByPlayerLoop()
        {
            if (Application.isPlaying == false || _locked) return;
            DTimer.Update();
        }

        private void Awake()
        {
            Instance = this;
            DTimer.Initialize();
            InitializePlayerLoopSystems();

#if DRAMEWORK_LOG
            Log("DCore is created", $"[{LOG_PREFIX}.{nameof(Awake)}()]", this);
#endif
        }

        private void EarlyUpdateByPlayerLoop()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.Updater.EarlyUpdate();
        }

        private void FixedUpdateByPlayerLoop()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.Updater.FixedUpdate();
        }

        private void InitializePlayerLoopSystems()
        {
#if UNITY_EDITOR

            if (Helpers.IsDomainReloadDisabled)
            {
                Helpers.UnityPlayerLoop.RemoveSystem<TimeUpdate>(typeof(DCore));
                Helpers.UnityPlayerLoop.RemoveSystem<EarlyUpdate>(typeof(DCore));
                Helpers.UnityPlayerLoop.RemoveSystem<FixedUpdate>(typeof(DCore));
                Helpers.UnityPlayerLoop.RemoveSystem<PreUpdate>(typeof(DCore));
                Helpers.UnityPlayerLoop.RemoveSystem<Update>(typeof(DCore));
                Helpers.UnityPlayerLoop.RemoveSystem<PreLateUpdate>(typeof(DCore));
                Helpers.UnityPlayerLoop.RemoveSystem<PostLateUpdate>(typeof(DCore));
            }

#endif
            if (DProjectConfig.Instance.PlayerLoopCustomUpdateConfig.TimerUpdate)
            {
                var loopSystem = new PlayerLoopSystem
                {
                    subSystemList = null,
                    updateDelegate = TimeUpdateByPlayerLoop,
                    type = typeof(DCore)
                };
                Helpers.UnityPlayerLoop.AddSystem<TimeUpdate>(loopSystem);
            }

            if (DProjectConfig.Instance.PlayerLoopCustomUpdateConfig.EarlyUpdate)
            {
                var loopSystem = new PlayerLoopSystem
                {
                    subSystemList = null,
                    updateDelegate = EarlyUpdateByPlayerLoop,
                    type = typeof(DCore)
                };
                Helpers.UnityPlayerLoop.AddSystem<EarlyUpdate>(loopSystem);
            }

            if (DProjectConfig.Instance.PlayerLoopCustomUpdateConfig.FixedUpdate)
            {
                var loopSystem = new PlayerLoopSystem
                {
                    subSystemList = null,
                    updateDelegate = FixedUpdateByPlayerLoop,
                    type = typeof(DCore)
                };
                Helpers.UnityPlayerLoop.InsertSystem<FixedUpdate>(6, loopSystem);
            }

            if (DProjectConfig.Instance.PlayerLoopCustomUpdateConfig.PreUpdate)
            {
                var loopSystem = new PlayerLoopSystem
                {
                    subSystemList = null,
                    updateDelegate = PreUpdateByPlayerLoop,
                    type = typeof(DCore)
                };
                Helpers.UnityPlayerLoop.AddSystem<PreUpdate>(loopSystem);
            }

            if (DProjectConfig.Instance.PlayerLoopCustomUpdateConfig.Update)
            {
                var loopSystem = new PlayerLoopSystem
                {
                    subSystemList = null,
                    updateDelegate = UpdateByPlayerLoop,
                    type = typeof(DCore)
                };
                Helpers.UnityPlayerLoop.InsertSystem<Update>(0, loopSystem);
            }

            if (DProjectConfig.Instance.PlayerLoopCustomUpdateConfig.PreLateUpdate)
            {
                var loopSystem = new PlayerLoopSystem
                {
                    subSystemList = null,
                    updateDelegate = PreLateUpdateByPlayerLoop,
                    type = typeof(DCore)
                };
                Helpers.UnityPlayerLoop.InsertSystem<PreLateUpdate>(14, loopSystem);
            }

            if (DProjectConfig.Instance.PlayerLoopCustomUpdateConfig.PostLateUpdate)
            {
                var loopSystem = new PlayerLoopSystem
                {
                    subSystemList = null,
                    updateDelegate = PostLateUpdateByPlayerLoop,
                    type = typeof(DCore)
                };
                Helpers.UnityPlayerLoop.AddSystem<PostLateUpdate>(loopSystem);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                if (_locked == false) return;
                OnContinue();
            }
            else
            {
                if (_locked) return;
                OnPause();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (_locked) return;
                OnPause();
            }
            else
            {
                if (_locked == false) return;
                OnContinue();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnApplicationQuit()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.OnQuit();
        }

        private void OnContinue()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.OnContinue();

            DTimer.OnContinue();
            _locked = false;
            onContinue?.Invoke();
        }

        private void OnDestroy()
        {
            Instance = null;
            DTimer.OnDestroy();
            onDestroy?.Invoke();
#if DRAMEWORK_LOG
            Log("DCore is destroyed", $"[{LOG_PREFIX}.{nameof(OnDestroy)}()]", this);
#endif
        }

        private void OnPause()
        {
            _locked = true;
            foreach (var dispatcher in _dispatchers)
                dispatcher.OnPause();

            DTimer.OnPause();
            onPause?.Invoke();
        }

        private void PostLateUpdateByPlayerLoop()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.Updater.PostLateUpdate();
        }

        private void PreLateUpdateByPlayerLoop()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.Updater.PreLateUpdate();
        }

        private void PreUpdateByPlayerLoop()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.Updater.PreUpdate();
        }

        private void UpdateByPlayerLoop()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.Updater.Update();
        }

        #endregion
    }
}