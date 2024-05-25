using System;
using System.Collections.Generic;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Ordering;
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
    internal class DDispatcherDisposer
    {
        #region ================================ FIELDS

        private readonly string _name;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DDispatcherDisposer(string name)
        {
            _name = name;
        }

        #endregion

        #region ================================ METHODS

        internal void Add(object obj)
        {
            if (obj is IDisposable disposable)
                _disposables.Add(disposable);
        }

        internal void Dispose()
        {
            var prefix = $"[{_name}.Dispose()]";

            foreach (var disposable in _disposables)
            {
                ConsoleLogger.Log($"Object «{disposable.GetType()}» is going to dispose", prefix);
                disposable.Dispose();
                ConsoleLogger.Log($"Object «{disposable.GetType()}» is disposed", prefix);
            }
        }

        internal void Order()
        {
            Helpers.CollectionTools.InsertionSort(_disposables, o =>
            {
                var orderAttribute = o.GetType().GetCustomAttribute<DisposeOrderAttribute>();
                return orderAttribute?.Order ?? 0;
            });
        }

        #endregion
    }
}