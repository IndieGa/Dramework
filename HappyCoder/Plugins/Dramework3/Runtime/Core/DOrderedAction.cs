using System;
using System.Collections.Generic;
using System.Linq;


namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    public class DOrderedAction<TKey> where TKey : Enum
    {
        #region ================================ FIELDS

        private Dictionary<TKey, Action> _listeners = new Dictionary<TKey, Action>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DOrderedAction(ref Action invoker)
        {
            invoker += Invoke;
        }

        #endregion

        #region ================================ METHODS

        public void AddListener(TKey key, Action listener)
        {
            _listeners.TryAdd(key, listener);
            _listeners = _listeners.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void RemoveListener(TKey key)
        {
            _listeners.Remove(key);
            _listeners = _listeners.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void Invoke()
        {
            foreach (var listener in _listeners.Values)
                listener.Invoke();
        }

        #endregion
    }

    public class DOrderedAction<TKey, TValue> where TKey : Enum
    {
        #region ================================ FIELDS

        private Dictionary<TKey, Action<TValue>> _listeners = new Dictionary<TKey, Action<TValue>>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DOrderedAction(ref Action<TValue> invoker)
        {
            invoker += Invoke;
        }

        #endregion

        #region ================================ METHODS

        public void AddListener(TKey key, Action<TValue> listener)
        {
            _listeners.TryAdd(key, listener);
            _listeners = _listeners.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void RemoveListener(TKey key)
        {
            _listeners.Remove(key);
            _listeners = _listeners.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void Invoke(TValue value)
        {
            foreach (var listener in _listeners.Values)
                listener.Invoke(value);
        }

        #endregion
    }

    public class DOrderedAction<TKey, TValue1, TValue2> where TKey : Enum
    {
        #region ================================ FIELDS

        private Dictionary<TKey, Action<TValue1, TValue2>> _listeners = new Dictionary<TKey, Action<TValue1, TValue2>>();

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DOrderedAction(ref Action<TValue1, TValue2> invoker)
        {
            invoker += Invoke;
        }

        #endregion

        #region ================================ METHODS

        public void AddListener(TKey key, Action<TValue1, TValue2> listener)
        {
            _listeners.TryAdd(key, listener);
            _listeners = _listeners.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void RemoveListener(TKey key)
        {
            _listeners.Remove(key);
            _listeners = _listeners.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void Invoke(TValue1 value, TValue2 value2)
        {
            foreach (var listener in _listeners.Values)
                listener.Invoke(value, value2);
        }

        #endregion
    }
}