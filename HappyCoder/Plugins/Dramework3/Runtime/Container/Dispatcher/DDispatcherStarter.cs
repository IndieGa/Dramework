using System.Collections.Generic;
using System.Reflection;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Ordering;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Controlling;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal class DDispatcherStarter
    {
        #region ================================ FIELDS

        private readonly string _name;
        private readonly List<IEarlyStartable> _earlyStartables = new List<IEarlyStartable>();
        private readonly List<IStartable> _startables = new List<IStartable>();
        private readonly List<ILateStartable> _lateStartables = new List<ILateStartable>();
        private readonly List<ILastStartable> _lastStartables = new List<ILastStartable>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DDispatcherStarter(string name)
        {
            _name = name;
        }

        #endregion

        #region ================================ METHODS

        internal void Add(object obj)
        {
            if (obj is IEarlyStartable earlyStartable)
                _earlyStartables.Add(earlyStartable);
            if (obj is IStartable startable)
                _startables.Add(startable);
            if (obj is ILateStartable lateStartable)
                _lateStartables.Add(lateStartable);
            if (obj is ILastStartable lastStartable)
                _lastStartables.Add(lastStartable);
        }

        internal void Dispose()
        {
            _earlyStartables.Clear();
            _startables.Clear();
            _lateStartables.Clear();
            _lastStartables.Clear();
        }

        internal void Order()
        {
            Helpers.CollectionTools.InsertionSort(_earlyStartables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<EarlyStartOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_startables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<StartOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_lateStartables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<LateStartOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_lastStartables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<LastStartOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });
        }

        internal async UniTask StartAsync()
        {
            var prefix = $"[{_name}.StartAsync()]";

            foreach (var obj in _earlyStartables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to early start", prefix);
                await obj.OnEarlyStart();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is early started", prefix);
            }

            foreach (var obj in _startables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to start", prefix);
                await obj.OnStart();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is started", prefix);
            }

            foreach (var obj in _lateStartables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to late start", prefix);
                await obj.OnLateStart();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is late started", prefix);
            }

            foreach (var obj in _lastStartables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to last start", prefix);
                await obj.OnLastStart();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is last started", prefix);
            }
        }

        #endregion
    }
}