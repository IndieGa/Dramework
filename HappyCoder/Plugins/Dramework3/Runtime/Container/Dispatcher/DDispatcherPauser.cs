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
    internal class DDispatcherPauser
    {
        #region ================================ FIELDS

        private readonly string _name;

        private readonly List<IEarlyPausable> _earlyPausables = new List<IEarlyPausable>();
        private readonly List<IPausable> _pausables = new List<IPausable>();
        private readonly List<ILatePausable> _latePausables = new List<ILatePausable>();

        private readonly List<IEarlyUnpausable> _earlyUnpausables = new List<IEarlyUnpausable>();
        private readonly List<IUnpausable> _unpausables = new List<IUnpausable>();
        private readonly List<ILateUnpausable> _lateUnpausables = new List<ILateUnpausable>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DDispatcherPauser(string name)
        {
            _name = name;
        }

        #endregion

        #region ================================ METHODS

        internal void Add(object obj)
        {
            if (obj is IEarlyPausable earlyPausable)
                _earlyPausables.Add(earlyPausable);
            if (obj is IPausable pausable)
                _pausables.Add(pausable);
            if (obj is ILatePausable latePausable)
                _latePausables.Add(latePausable);
            if (obj is IEarlyUnpausable earlyUnlockable)
                _earlyUnpausables.Add(earlyUnlockable);
            if (obj is IUnpausable unlockable)
                _unpausables.Add(unlockable);
            if (obj is ILateUnpausable lateUnlockable)
                _lateUnpausables.Add(lateUnlockable);
        }

        internal void Dispose()
        {
            _earlyPausables.Clear();
            _pausables.Clear();
            _latePausables.Clear();
            _earlyUnpausables.Clear();
            _unpausables.Clear();
            _lateUnpausables.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnContinue()
        {
            var prefix = $"[{_name}.OnContinue()]";

            foreach (var obj in _earlyUnpausables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to early unpause", prefix);
                obj.OnEarlyUnpause();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is early unpaused", prefix);
            }

            foreach (var obj in _unpausables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to unpause", prefix);
                obj.OnUnpause();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is unpaused", prefix);
            }

            foreach (var obj in _lateUnpausables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to late unpause", prefix);
                obj.OnLateUnpause();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is late unpaused", prefix);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnPause()
        {
            var prefix = $"[{_name}.OnPause()]";

            foreach (var obj in _earlyPausables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to early pause", prefix);
                obj.OnEarlyPause();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is early paused", prefix);
            }

            foreach (var obj in _pausables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to early pause", prefix);
                obj.OnPause();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is paused", prefix);
            }

            foreach (var obj in _latePausables)
            {
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to late pause", prefix);
                obj.OnLatePause();
                ConsoleLogger.Log($"Object «{obj.GetType()}» is late paused", prefix);
            }
        }

        internal void Order()
        {
            Helpers.CollectionTools.InsertionSort(_earlyPausables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<EarlyPauseOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_pausables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<PauseOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_latePausables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<LatePauseOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_earlyUnpausables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<EarlyUnpauseOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_unpausables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<UnpauseOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_latePausables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<LateUnpauseOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });
        }

        #endregion
    }
}