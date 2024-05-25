using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

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
    internal class DDispatcherStopper
    {
        #region ================================ FIELDS

        private readonly string _name;

        private readonly List<IEarlyStoppable> _earlyStoppables = new List<IEarlyStoppable>();
        private readonly List<IStoppable> _stoppables = new List<IStoppable>();
        private readonly List<ILateStoppable> _lateStoppables = new List<ILateStoppable>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DDispatcherStopper(string name)
        {
            _name = name;
        }

        #endregion

        #region ================================ METHODS

        internal void Add(object obj)
        {
            if (obj is IEarlyStoppable earlyStoppable)
                _earlyStoppables.Add(earlyStoppable);
            if (obj is IStoppable stoppable)
                _stoppables.Add(stoppable);
            if (obj is ILateStoppable lateStoppable)
                _lateStoppables.Add(lateStoppable);
        }

        internal void Dispose()
        {
            _earlyStoppables.Clear();
            _stoppables.Clear();
            _lateStoppables.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnQuit()
        {
            var prefix = $"[{_name}.OnQuit()]";

            foreach (var obj in _earlyStoppables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to early stop", prefix);
                obj.OnEarlyStop();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is early stopped", prefix);
            }

            foreach (var obj in _stoppables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to stop", prefix);
                obj.OnStop();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is stopped", prefix);
            }

            foreach (var obj in _lateStoppables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to late stop", prefix);
                obj.OnLateStop();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is late stopped", prefix);
            }
        }

        internal void Order()
        {
            Helpers.CollectionTools.InsertionSort(_earlyStoppables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<EarlyStopOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_stoppables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<StopOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_lateStoppables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<LateStopOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });
        }

        #endregion
    }
}