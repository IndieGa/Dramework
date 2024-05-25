using System;
using System.Collections.Generic;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;


namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    public class DAction<TEnum> where TEnum : Enum
    {
        #region ================================ FIELDS

        private readonly Dictionary<TEnum, Action> _listeners = new Dictionary<TEnum, Action>();
        private Action[] _invocationList = Array.Empty<Action>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DAction()
        {
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                _listeners.Add((TEnum)value, null);
            }
        }

        #endregion

        #region ================================ METHODS

        public void AddListener(TEnum order, Action action)
        {
            if (_listeners[order] != null)
            {
                ConsoleLogger.LogError($"Trying to add the listener {GetActionInfo(action)} in order {order}. But it is already busy by {GetActionInfo(_listeners[order])}");
                return;
            }

            _listeners[order] = action;
            UpdateInvocationList();
        }

        public void Invoke()
        {
            foreach (var action in _invocationList)
                action.Invoke();
        }

        public void RemoveAllListeners()
        {
            foreach (var value in Enum.GetValues(typeof(TEnum)))
                _listeners[(TEnum)value] = null;

            UpdateInvocationList();
        }

        public void RemoveListener(TEnum order, Action action)
        {
            if (_listeners[order] == null)
            {
                ConsoleLogger.LogError($"Trying to removing the listener {GetActionInfo(action)} from order {order}. But it is not busy");
                return;
            }

            if (_listeners[order] != action)
            {
                ConsoleLogger.LogError($"Trying to removing the listener {GetActionInfo(action)} from order {order}. But the order {order} is busy by {GetActionInfo(_listeners[order])}");
                return;
            }

            _listeners[order] = null;
            UpdateInvocationList();
        }

        private string GetActionInfo(Action action)
        {
            return $"{action.Target.GetType()}.{action.Method.Name}()";
        }

        private void UpdateInvocationList()
        {
            _invocationList = _listeners.Values.Where(v => v != null).ToArray();
        }

        #endregion
    }
}