using System.Collections.Generic;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Ordering;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Updating;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal class DDispatcherUpdater
    {
        #region ================================ FIELDS

        private readonly List<IEarlyUpdatable> _earlyUpdatables = new List<IEarlyUpdatable>();
        private readonly List<IFixedUpdatable> _fixedUpdatables = new List<IFixedUpdatable>();
        private readonly List<IPreUpdatable> _preUpdatables = new List<IPreUpdatable>();
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        private readonly List<IPreLateUpdatable> _preLateUpdatables = new List<IPreLateUpdatable>();
        private readonly List<IPostLateUpdatable> _postLateUpdatables = new List<IPostLateUpdatable>();

        private bool _isUpdating;

        #endregion

        #region ================================ METHODS

        internal void Add(object obj)
        {
            if (obj is IEarlyUpdatable earlyUpdatable)
                _earlyUpdatables.Add(earlyUpdatable);
            if (obj is IFixedUpdatable fixedUpdatable)
                _fixedUpdatables.Add(fixedUpdatable);
            if (obj is IPreUpdatable preUpdatable)
                _preUpdatables.Add(preUpdatable);
            if (obj is IUpdatable updatable)
                _updatables.Add(updatable);
            if (obj is IPreLateUpdatable preLateUpdatable)
                _preLateUpdatables.Add(preLateUpdatable);
            if (obj is IPostLateUpdatable postLateUpdatable)
                _postLateUpdatables.Add(postLateUpdatable);
        }

        internal void Dispose()
        {
            _isUpdating = false;
            _earlyUpdatables.Clear();
            _fixedUpdatables.Clear();
            _preUpdatables.Clear();
            _updatables.Clear();
            _preLateUpdatables.Clear();
            _postLateUpdatables.Clear();
        }

        internal void EarlyUpdate()
        {
            if (_isUpdating == false) return;
            foreach (var updatable in _earlyUpdatables)
                updatable.OnEarlyUpdate();
        }

        internal void FixedUpdate()
        {
            if (_isUpdating == false) return;
            foreach (var updatable in _fixedUpdatables)
                updatable.OnFixedUpdate();
        }

        internal void Order()
        {
            Helpers.CollectionTools.InsertionSort(_earlyUpdatables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<EarlyUpdateOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_fixedUpdatables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<FixedUpdateOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_preUpdatables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<PreUpdateOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_updatables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<UpdateOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_preLateUpdatables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<PreLateUpdateOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_postLateUpdatables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<PostLateUpdateOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });
        }

        internal void PostLateUpdate()
        {
            if (_isUpdating == false) return;
            foreach (var updatable in _postLateUpdatables)
                updatable.OnPostLateUpdate();
        }

        internal void PreLateUpdate()
        {
            if (_isUpdating == false) return;
            foreach (var updatable in _preLateUpdatables)
                updatable.OnPreLateUpdate();
        }

        internal void PreUpdate()
        {
            if (_isUpdating == false) return;
            foreach (var updatable in _preUpdatables)
                updatable.OnPreUpdate();
        }

        internal void StartUpdate()
        {
            _isUpdating = true;
        }

        internal void Update()
        {
            if (_isUpdating == false) return;
            foreach (var updatable in _updatables)
                updatable.OnUpdate();
        }

        #endregion
    }
}