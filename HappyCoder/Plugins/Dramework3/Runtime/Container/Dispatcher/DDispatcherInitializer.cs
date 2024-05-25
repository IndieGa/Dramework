using System.Collections.Generic;
using System.Reflection;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Ordering;
using IG.HappyCoder.Dramework3.Runtime.Container.Interfaces.Initialization;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;
#if DRAMEWORK_LOG
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;
#endif


namespace IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher
{
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal class DDispatcherInitializer
    {
        #region ================================ FIELDS

        // ReSharper disable once NotAccessedField.Local
        private readonly string _name;

        private readonly List<IEarlyInitializable> _earlyInitializables = new List<IEarlyInitializable>();
        private readonly List<IInitializable> _initializables = new List<IInitializable>();
        private readonly List<ILateInitializable> _lateInitializables = new List<ILateInitializable>();
        private readonly List<ILastInitializable> _lastInitializables = new List<ILastInitializable>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DDispatcherInitializer(string name)
        {
            _name = name;
        }

        #endregion

        #region ================================ METHODS

        internal void Add(object obj)
        {
            if (obj is IEarlyInitializable earlyInitializable)
                _earlyInitializables.Add(earlyInitializable);
            if (obj is IInitializable initializable)
                _initializables.Add(initializable);
            if (obj is ILateInitializable lateInitializable)
                _lateInitializables.Add(lateInitializable);
            if (obj is ILastInitializable lastInitializable)
                _lastInitializables.Add(lastInitializable);
        }

        internal void Dispose()
        {
            _earlyInitializables.Clear();
            _initializables.Clear();
            _lateInitializables.Clear();
            _lastInitializables.Clear();
        }

        internal async UniTask InitializeAsync()
        {
#if DRAMEWORK_LOG
            var prefix = $"[{_name}.InitializeAsync()]";
#endif

            foreach (var obj in _earlyInitializables)
            {
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to early initialize", prefix);
#endif
                await obj.OnEarlyInitialize();
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» early initialization is completed", prefix);
#endif
            }

            foreach (var obj in _initializables)
            {
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to initialize", prefix);
#endif
                await obj.OnInitialize();
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» initialization is completed", prefix);
#endif
            }

            foreach (var obj in _lateInitializables)
            {
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to late initialize", prefix);
#endif
                await obj.OnLateInitialize();
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» late initialization is completed", prefix);
#endif
            }

            foreach (var obj in _lastInitializables)
            {
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» is going to last initialize", prefix);
#endif
                await obj.OnLastInitialize();
#if DRAMEWORK_LOG
                ConsoleLogger.Log($"Object «{obj.GetType()}» last initialization is completed", prefix);
#endif
            }
        }

        internal void Order()
        {
            Helpers.CollectionTools.InsertionSort(_earlyInitializables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<EarlyInitializeOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_initializables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<InitializeOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_lateInitializables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<LateInitializeOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });

            Helpers.CollectionTools.InsertionSort(_lastInitializables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<LastInitializeOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });
        }

        #endregion
    }
}