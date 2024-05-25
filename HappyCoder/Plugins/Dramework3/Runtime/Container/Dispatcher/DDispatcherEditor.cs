#if UNITY_EDITOR

using System;
using System.Linq;
using System.Reflection;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection;
using IG.HappyCoder.Dramework3.Runtime.Container.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Controlling;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Initialization;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Updating;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher
{
    public sealed partial class DDispatcher
    {
        #region ================================ METHODS

        private static bool IsInitializable(object obj)
        {
            return obj
                is IEarlyInitializable
                or IInitializable
                or ILateInitializable
                or ILastInitializable;
        }

        private static bool IsPausable(object obj)
        {
            return obj
                is IEarlyPausable
                or IPausable
                or ILatePausable;
        }

        private static bool IsStartable(object obj)
        {
            return obj
                is IEarlyStartable
                or IStartable
                or ILateStartable
                or ILastStartable;
        }

        private static bool IsStoppable(object obj)
        {
            return obj
                is IEarlyStoppable
                or IStoppable
                or ILateStoppable;
        }

        private static bool IsUnpausable(object obj)
        {
            return obj
                is IEarlyUnpausable
                or IUnpausable
                or ILateUnpausable;
        }

        private static bool IsUpdatable(object obj)
        {
            return obj
                is IEarlyUpdatable
                or IFixedUpdatable
                or IPreUpdatable
                or IUpdatable
                or IPreLateUpdatable
                or IPostLateUpdatable;
        }

        protected override void OnEditorInitialize()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || gameObject.scene.IsValid() == false) return;
            InitializeDispatcherData();
        }

        private void InitializeDispatcherData()
        {
            if (this == null || gameObject == null || gameObject.scene.isLoaded == false) return;

            _sceneObjectsForBinding.Clear();

            var monoBehaviours = gameObject.scene
                .GetRootGameObjects()
                .SelectMany(o => o.GetComponentsInChildren<MonoBehaviour>(true))
                .Where(m => m != null)
                .ToArray();

            var bindedMonoBehaviours = monoBehaviours
                .Where(m => m.GetType().GetCustomAttribute<BindAttribute>() != null);

            _sceneObjectsForBinding.AddRange(bindedMonoBehaviours.Select(monoBehaviour => new DContainerObject
            {
                SceneID = gameObject.scene.name,
                Instance = monoBehaviour,
                AssemblyQualifiedName = monoBehaviour.GetType().AssemblyQualifiedName,
                Bind = true,
                InstanceID = monoBehaviour.GetType().GetCustomAttribute<BindAttribute>().InstanceID,
                HideSceneID = true
            }));

            var binders = gameObject.scene
                .GetRootGameObjects()
                .SelectMany(o => o.GetComponentsInChildren<DBinder>(true));

            _sceneObjectsForBinding.AddRange(binders.Where(b => b.Component != null).Select(b => new DContainerObject
            {
                SceneID = gameObject.scene.name,
                Instance = b.Component,
                AssemblyQualifiedName = b.Component.GetType().AssemblyQualifiedName,
                Bind = true,
                InstanceID = b.InstanceID,
                HideSceneID = true
            }));

            _sceneObjectsForBinding = _sceneObjectsForBinding.OrderBy(d => d.Title).ToList();

            // ReSharper disable SuspiciousTypeConversion.Global
            _dispatchedBehaviours = monoBehaviours.Where
            (m => IsInitializable(m)
                  || IsStartable(m)
                  || IsPausable(m)
                  || IsUnpausable(m)
                  || IsStoppable(m)
                  || IsUpdatable(m)
                  || m is IDisposable
            ).ToList();
            // ReSharper restore SuspiciousTypeConversion.Global
        }

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal")]
        [Button("Validate", 22)]
        private void OnValidate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || gameObject.scene.IsValid() == false) return;
            Validate().Forget();
        }

        private async UniTask Validate()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            EditorApplication.hierarchyChanged -= InitializeDispatcherData;
            EditorApplication.hierarchyChanged += InitializeDispatcherData;
            InitializeDispatcherData();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}

#endif