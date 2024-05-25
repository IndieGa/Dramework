using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection;
using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Container.Factories;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Common;
using IG.HappyCoder.Dramework3.Runtime.Factories.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;

using Sirenix.OdinInspector;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEditor;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher
{
    [HideMonoScript]
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    public sealed partial class DDispatcher : DBehaviour, IDispatcher
    {
        #region ================================ FIELDS

        [BoxGroup("Main", false)] [BoxGroup("Main/Initialize Mode", false)]
        [LabelText("Initialize Mode:")]
        [SerializeField]
        private InitializationMode _initializationMode;

        [BoxGroup("Main", false)]
        [LabelText("Objects for Binding:")]
        [SerializeField] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)] [ReadOnly]
        private List<DContainerObject> _sceneObjectsForBinding = new List<DContainerObject>();

        [BoxGroup("Main", false)]
        [LabelText("Dispatched Behaviours:")]
        [SerializeField] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)] [ReadOnly]
        private List<MonoBehaviour> _dispatchedBehaviours = new List<MonoBehaviour>();

        private readonly List<object> _container = new List<object>();
        private readonly HashSet<object> _tempObjects = new HashSet<object>();
        private readonly Dictionary<string, object> _identifiedObjects = new Dictionary<string, object>();
        private readonly Dictionary<string, byte[]> _remoteFiles = new Dictionary<string, byte[]>();

        private DDispatcherInitializer _initializer;
        private DDispatcherStarter _starter;
        private DDispatcherUpdater _updater;
        private DDispatcherPauser _pauser;
        private DDispatcherStopper _stopper;
        private DDispatcherDisposer _disposer;

        private bool _isDontDestroyOnLoad;
        private Transform _instantiatedObjects;
        private bool _isRegistered;
        private CancellationTokenSource _ct;
        private bool _lock;
        private bool _pause;
        private bool _isManualInitializationStarted;
        private bool _isInitialized;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string SceneName { get; private set; }
        internal DDispatcherUpdater Updater => _updater;
        private string LogPrefix => $"{name}";

        #endregion

        #region ================================ METHODS

        private static object GetStub(Type objType)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());

            foreach (var type in types)
            {
                var stubAttribute = type.GetCustomAttribute<StubAttribute>();
                if (stubAttribute == null || objType.EqualOrBase(type) == false) continue;
                return Activator.CreateInstance(type);
            }

            return null;
        }

        internal T Bind<T>(T obj, string instanceID)
        {
            if (_container.Any(o => o.Equals(obj)) == false)
            {
                _container.Add(obj);
#if DRAMEWORK_LOG
                Log($"Object «{obj.GetType()}» instance {(string.IsNullOrEmpty(instanceID) ? "without id" : $"id: «{instanceID}»")} is binded to container «{SceneName}»", $"[{LogPrefix}.{nameof(Bind)}()]", this);
#endif
            }

            if (string.IsNullOrEmpty(instanceID) == false)
            {
                _identifiedObjects.TryAdd(instanceID, obj);
#if DRAMEWORK_LOG
                Log($"Object «{obj.GetType()}» instance id: «{instanceID}» is binded to identified objects container «{SceneName}»", $"[{LogPrefix}.{nameof(Bind)}()]", this);
#endif
            }

            return obj;
        }

        internal object GetContainerObject(Type objType, string instanceID)
        {
            if (Application.isPlaying == false)
                return null;

            var result = string.IsNullOrEmpty(instanceID)
                ? FirstOrDefault()
                : _identifiedObjects.GetValueOrDefault(instanceID); // ?? GetStub(objType);

            return result;

            object FirstOrDefault()
            {
                for (var i = 0; i < _container.Count; i++)
                {
                    if (objType.EqualOrBase(_container[i].GetType()) == false) continue;
                    return _container[i];
                }
                return null;
            }
        }

        internal IEnumerable<object> GetContainerObjects(Type objectType)
        {
            var result = _container.Where(obj => obj.GetType() == objectType || objectType.IsInstanceOfType(obj)).ToArray();
#if UNITY_EDITOR
            // if (result.Length == 0)
            // result = new[] { GetStub(objectType) };
#endif
            return result;
        }

        internal byte[] GetRemoteFile(string filename)
        {
            return Application.isPlaying == false ? null : _remoteFiles.GetValueOrDefault(filename);
        }

        internal void InitializeContainerObject(object obj)
        {
            if (obj is DBehaviour dBehaviour)
                dBehaviour.InternalInitialize();

            _updater.Add(obj);
            _pauser.Add(obj);
            _stopper.Add(obj);
            _disposer.Add(obj);

            _updater.Order();
            _pauser.Order();
            _stopper.Order();
            _disposer.Order();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnContinue()
        {
            _pauser.OnContinue();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnPause()
        {
            _pauser.OnPause();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnQuit()
        {
            _stopper.OnQuit();
        }

        internal void Unbind<T>(T obj)
        {
            var removed = false;
            for (var i = _container.Count - 1; i >= 0; i--)
            {
                if (_container[i].Equals(obj) == false) continue;
                _container.RemoveAt(i);
                removed = true;
            }

            foreach (var identifiedObject in _identifiedObjects.Where(identifiedObject => identifiedObject.Value.Equals(obj)))
            {
                _identifiedObjects.Remove(identifiedObject.Key);
                removed = true;
            }

            if (removed == false) return;
            LogError($"Object {obj.GetType()} is not found in container {SceneName}", $"{nameof(DDispatcher)}.Unbind()", this);
        }

        private void Awake()
        {
#if UNITY_EDITOR
            if (EditorBuildSettings.scenes.Any(scene => scene.path == gameObject.scene.path && scene.enabled)
                && gameObject.scene.buildIndex != 0
                && DCore.DispatchersCount == 0)
            {
                _lock = true;
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                return;
            }
#endif

            SceneName = gameObject.scene.name;

            MoveToDontDestroyScene();

            _pause = _initializationMode == InitializationMode.Manual;
            _ct = new CancellationTokenSource();
            _initializer = new DDispatcherInitializer(name);
            _starter = new DDispatcherStarter(name);
            _updater = new DDispatcherUpdater();
            _pauser = new DDispatcherPauser(name);
            _stopper = new DDispatcherStopper(name);
            _disposer = new DDispatcherDisposer(name);
#if DRAMEWORK_LOG
            Log($"Dispatcher on scene \"{SceneName}\" is created", $"[{LogPrefix}.{nameof(Awake)}()]", this);
#endif

            _isRegistered = DCore.RegisterDispatcher(this);
#if DRAMEWORK_LOG
            Log($"Dispatcher on scene \"{SceneName}\" awake is completed", $"[{LogPrefix}.{nameof(Awake)}()]", this);
#endif
        }

        private void ClearTempContainerObjects()
        {
            _tempObjects.Clear();
            _dispatchedBehaviours.Clear();
        }

        private async UniTask CreateContainerObjects()
        {
            if (_ct.IsCancellationRequested) return;

            if (gameObject.scene.buildIndex == 0)
                _container.Add(new DGlobalFactory(this));

            _container.AddRange(await DProjectConfig.LoadSceneResources(SceneName));

            foreach (var sceneObject in _sceneObjectsForBinding)
            {
                Bind(sceneObject.Instance, sceneObject.InstanceID);
            }

            var installers = new List<DInstaller>();

            CreateInstallers();
            CreateModels();

            DCore.PreInitializationIsComplete(SceneName);

            await UniTask.WaitWhile(() => _pause);

            if (_ct.IsCancellationRequested) return;

            CreateFactories();
            CreateSystems();

            return;

            void CreateInstallers()
            {
                foreach (var assemblyQualifiedName in DProjectConfig.GetInstallerTypes(SceneName))
                {
                    var type = Type.GetType(assemblyQualifiedName);
                    if (type == null)
                    {
#if DRAMEWORK_LOG
                        LogError($"Can not create installer {assemblyQualifiedName}", LogPrefix, this);
#endif
                        continue;
                    }

                    installers.Add((DInstaller)Activator.CreateInstance(type));
                }
            }

            void CreateModels()
            {
                foreach (var installer in installers)
                {
                    foreach (var model in installer.CreateModels())
                    {
                        if (model.Bind)
                        {
                            Bind(model.Object, model.InstanceID);
                        }
                        else
                        {
                            _tempObjects.Add(model.Object);
#if DRAMEWORK_LOG
                            Log($"Model type of «{model.Object.GetType()}» is created", LogPrefix, this);
#endif
                        }
                    }
                }
            }

            void CreateFactories()
            {
                foreach (var installer in installers)
                {
                    foreach (var factory in installer.CreateFactories())
                    {
                        if (factory.Bind)
                        {
                            Bind(factory.Object, factory.InstanceID);
                        }
                        else
                        {
                            _tempObjects.Add(factory.Object);
#if DRAMEWORK_LOG
                            Log($"Factory type of «{factory.Object.GetType()}» is created", LogPrefix, this);
#endif
                        }
                    }
                }
            }

            void CreateSystems()
            {
                foreach (var installer in installers)
                {
                    foreach (var system in installer.CreateSystems())
                    {
                        if (system.Bind)
                        {
                            Bind(system.Object, system.InstanceID);
                        }
                        else
                        {
                            _tempObjects.Add(system.Object);
#if DRAMEWORK_LOG
                            Log($"System type of «{system.Object.GetType()}» is created", LogPrefix, this);
#endif
                        }
                    }
                }
            }
        }

        async UniTask IDispatcher.Initialize()
        {
            if (_pause == false || _isManualInitializationStarted) return;

            _isManualInitializationStarted = true;
            _pause = false;

            await UniTask.WaitUntil(() => _isInitialized);
        }

        private void InitializeTempContainerObjects()
        {
            foreach (var obj in _container)
                _tempObjects.Add(obj);

            foreach (var obj in _dispatchedBehaviours)
                _tempObjects.Add(obj);
        }

        private async UniTask InvokeInitializeOnObjectsAsync()
        {
            await _initializer.InitializeAsync();
            _initializer.Dispose();
            _initializer = null;
        }

        private async UniTask InvokeStartOnObjectsAsync()
        {
            await _starter.StartAsync();
            _starter = null;
        }

        private async UniTask LoadRemoteFiles()
        {
            if (DProjectConfig.TryGetRemoteFileUrls(SceneName, out var urls) == false || urls.Count == 0) return;

            var directory = DProjectConfig.GetRemoteFilesDirectory(SceneName);

            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            foreach (var url in urls)
            {
                var filename = Path.GetFileName(url);
                var stopWatch = new Stopwatch();
                var useRemoteUrl = url.StartsWith("http");

                if (useRemoteUrl)
                {
                    stopWatch.Start();

                    var request = UnityWebRequest.Get(url);
                    var asyncOp = request.SendWebRequest();

                    DCore.LoadResourceProgress(0);
                    while (asyncOp.isDone == false)
                    {
                        DCore.LoadResourceProgress(asyncOp.progress);
                        await UniTask.Yield();
                    }
                    DCore.LoadResourceProgress(1);

                    stopWatch.Stop();

                    if (request is { result: UnityWebRequest.Result.Success })
                    {
#if DRAMEWORK_LOG
                        Log($"Remote file is loaded by url:{url} in {stopWatch.ElapsedMilliseconds} msec.");
#endif
                        _remoteFiles.Add(filename, request.downloadHandler.data);
                    }
                    else
                    {
                        await LoadFromStreamingAssets(filename);
                    }
                }
                else
                {
                    await LoadFromStreamingAssets(filename);
                }
            }
            return;

            async UniTask LoadFromStreamingAssets(string filename)
            {
                var loadPath = Path.Combine(Application.streamingAssetsPath, filename);
#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                using var webRequest = UnityWebRequest.Get(loadPath);
                var asyncOp = webRequest.SendWebRequest();

                DCore.LoadResourceProgress(0);
                while (asyncOp.isDone == false)
                {
                    DCore.LoadResourceProgress(asyncOp.progress);
                    await UniTask.Yield();
                }
                DCore.LoadResourceProgress(1);

                stopWatch.Stop();
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
#if DRAMEWORK_LOG
                    Log($"Remote file is loaded at path:{loadPath} in {stopWatch.ElapsedMilliseconds} msec.");
#endif
                    _remoteFiles.Add(filename, webRequest.downloadHandler.data);
                }
#else
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var bytes = await File.ReadAllBytesAsync(loadPath);
                _remoteFiles.Add(filename, bytes);
                stopWatch.Stop();
#if DRAMEWORK_LOG
                Log($"Remote file is loaded at path:{loadPath} in {stopWatch.ElapsedMilliseconds} msec.");
#endif
#endif
            }
        }

        private void MoveToDontDestroyScene()
        {
            var scene = gameObject.scene;
            if (scene.buildIndex > 0) return;

            _isDontDestroyOnLoad = true;

            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootGameObject in rootObjects)
            {
                DontDestroyOnLoad(rootGameObject);
                DCore.RegisterDontDestroyObject(rootGameObject);
            }

            for (var i = 0; i < rootObjects.Length; i++)
                rootObjects[i].transform.SetSiblingIndex(i);
        }

        private void OnDestroy()
        {
            if (_lock || _isRegistered == false) return;

            if (_isDontDestroyOnLoad)
                DContainerFactory.Dispose();

            _ct.Cancel();
            _container.Clear();
            _tempObjects.Clear();
            _identifiedObjects.Clear();

            _starter?.Dispose();
            _updater?.Dispose();
            _pauser?.Dispose();
            _stopper?.Dispose();
            _disposer?.Dispose();

            _isRegistered = false;
            DCore.UnregisterDispatcher(this);
            DProjectConfig.ReleaseResources(SceneName);
#if DRAMEWORK_LOG
            Log($"Dispatcher on scene \"{SceneName}\" is destroyed", $"[{LogPrefix}.{nameof(OnDestroy)}()]", this);
#endif
        }

        private void OrderContainerObjects()
        {
            _initializer.Order();
            _starter.Order();
            _updater.Order();
            _pauser.Order();
            _stopper.Order();
            _disposer.Order();
        }

        private void SortContainerObjects()
        {
            foreach (var obj in _tempObjects)
            {
                _initializer.Add(obj);
                _starter.Add(obj);
                _updater.Add(obj);
                _pauser.Add(obj);
                _stopper.Add(obj);
                _disposer.Add(obj);
            }
        }

        private async UniTaskVoid Start()
        {
#if UNITY_EDITOR
            if (_lock)
            {
#if DRAMEWORK_LOG
                Log($"Dispatcher on scene \"{SceneName}\" run Start. _lock: {_lock}", $"[{LogPrefix}.{nameof(Start)}()]", this);
#endif
                return;
            }
#endif

#if DRAMEWORK_LOG
            Log($"Dispatcher on scene \"{SceneName}\" run Start. _isRegistered: {_isRegistered}", $"[{LogPrefix}.{nameof(Start)}()]", this);
#endif
            if (_isRegistered == false) return;

            await LoadRemoteFiles();
            await CreateContainerObjects();
            InitializeTempContainerObjects();
            SortContainerObjects();
            // OrderContainerObjects();
            await InvokeInitializeOnObjectsAsync();
            ClearTempContainerObjects();
            await InvokeStartOnObjectsAsync();
            StartUpdateObjects();
            _isInitialized = true;
            DCore.InitializationIsComplete(SceneName);
        }

        private void StartUpdateObjects()
        {
            _updater.StartUpdate();
        }

        #endregion

        #region ================================ NESTED TYPES

        private enum InitializationMode
        {
            Manual,
            Auto
        }

        #endregion
    }
}