using System;

using Sirenix.OdinInspector;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Experimental.Providers
{
    [Serializable]
    public abstract class DValueContainer<T>
    {
        #region ================================ FIELDS

        [SerializeField] [ShowIf("@_showDescription")]
        [PropertyOrder(-10)] [HideLabel] [TextArea]
        [DisableIf("_locked")]
        protected string _description;

        [SerializeField] [HideInInspector]
        protected string _label;

        [SerializeField] [HideInInspector]
        protected Object _owner;

        [SerializeField] [HideInInspector]
        protected bool _locked;

        [SerializeField] [HideInInspector]
        protected bool _showDescription;

        private bool _isSettingLabel;
        private bool _isSettingDescription;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        protected DValueContainer()
        {
        }

        protected DValueContainer(Object owner, string label = "Value", string description = "")
        {
            _owner = owner;
            _label = label;
            _description = description;
        }

        #endregion

        #region ================================ EVENTS AND DELEGATES

#if UNITY_EDITOR
        internal abstract event Action<T> onChange;

#endif

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public bool Locked => _locked;

        protected internal virtual T Value { get; set; }

        #endregion

        #region ================================ METHODS

        protected internal virtual void OnSetValue(T value)
        {
        }

        #endregion
    }
}