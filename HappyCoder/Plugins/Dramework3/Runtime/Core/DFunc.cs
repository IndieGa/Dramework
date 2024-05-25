using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;


namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    public class DFunc<T>
    {
        #region ================================ FIELDS

        private readonly List<Func<T, T>> _invocationList = new List<Func<T, T>>();

        #endregion

        #region ================================ METHODS

        public void AddListener(Func<T, T> func)
        {
            if (_invocationList.Contains(func))
            {
                ConsoleLogger.LogError($"Trying to add the listener {GetFuncInfo(func)}. But the invocation list is already contains it");
                return;
            }

            _invocationList.Add(func);
        }

        public T Invoke(T value)
        {
            foreach (var func in _invocationList)
                value = func.Invoke(value);
            return value;
        }

        public void RemoveAllListeners()
        {
            _invocationList.Clear();
        }

        public void RemoveListener(Func<T, T> func)
        {
            if (_invocationList.Contains(func) == false)
            {
                ConsoleLogger.LogError($"Trying to removing the listener {GetFuncInfo(func)}. But the invocation list is not contains it");
                return;
            }
            _invocationList.Remove(func);
        }

        private string GetFuncInfo(Func<T, T> func)
        {
            return $"{func.Target.GetType()}.{func.Method.Name}()";
        }

        #endregion
    }

    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class DFunc<TEnum, T> where TEnum : Enum
    {
        #region ================================ FIELDS

        private readonly Dictionary<TEnum, Func<T, T>> _listeners = new Dictionary<TEnum, Func<T, T>>();
        private Func<T, T>[] _invocationList = Array.Empty<Func<T, T>>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DFunc()
        {
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                _listeners.Add((TEnum)value, null);
            }
        }

        #endregion

        #region ================================ METHODS

        public void AddListener(TEnum order, Func<T, T> func)
        {
            if (_listeners[order] != null)
            {
                ConsoleLogger.LogError($"Trying to add the listener {GetFuncInfo(func)} in order {order}. But it is already busy by {GetFuncInfo(_listeners[order])}");
                return;
            }

            _listeners[order] = func;
            UpdateInvocationList();
        }

        public T Invoke(T value)
        {
            foreach (var func in _invocationList)
                value = func.Invoke(value);
            return value;
        }

        public void RemoveAllListeners()
        {
            foreach (var value in Enum.GetValues(typeof(TEnum)))
                _listeners[(TEnum)value] = null;

            UpdateInvocationList();
        }

        public void RemoveListener(TEnum order, Func<T, T> func)
        {
            if (_listeners[order] == null)
            {
                ConsoleLogger.LogError($"Trying to removing the listener {GetFuncInfo(func)} from order {order}. But it is not busy");
                return;
            }

            if (_listeners[order] != func)
            {
                ConsoleLogger.LogError($"Trying to removing the listener {GetFuncInfo(func)} from order {order}. But the order {order} is busy by {GetFuncInfo(_listeners[order])}");
                return;
            }

            _listeners[order] = null;
            UpdateInvocationList();
        }

        private string GetFuncInfo(Func<T, T> func)
        {
            return $"{func.Target.GetType()}.{func.Method.Name}()";
        }

        private void UpdateInvocationList()
        {
            _invocationList = _listeners.Values.Where(v => v != null).ToArray();
        }

        #endregion
    }
}