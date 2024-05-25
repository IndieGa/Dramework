using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Experimental.Providers;

using Sirenix.OdinInspector;

using UnityEngine;

using Object = UnityEngine.Object;


#pragma warning disable 414


namespace IG.HappyCoder.Dramework3.Runtime.Experimental.Types
{
    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DSecuredFieldHorizontalButtons<T> : DValueContainer<T>
    {
        #region ================================ FIELDS

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal")]
        [EnableIf("@_locked == false")] [ShowIf("@_locking == false && _unlocking == false")]
        [LabelText("@_label")] [OnValueChanged("@Value = _value")]
        [SerializeField]
        private T _value;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DSecuredFieldHorizontalButtons()
        {
        }

        public DSecuredFieldHorizontalButtons(Object owner, string label = "Value", string description = "") : base(owner, label, description)
        {
        }

        #endregion

        #region ================================ METHODS

        public static implicit operator T(DSecuredFieldHorizontalButtons<T> field)
        {
            return field._value;
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal")]
        [LabelText("Password:")]
        [SerializeField] [ShowIf("@(_locking == true || _unlocking == true)")]
        private string _password;

        [SerializeField] [HideInInspector]
        private string _storedPassword;

        [SerializeField] [HideInInspector]
        private bool _locking;

        [SerializeField] [HideInInspector]
        private bool _unlocking;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal override event Action<T> onChange;
        protected internal override T Value
        {
            get => _value;
            set
            {
                _value = value;
                onChange?.Invoke(_value);
            }
        }

        #endregion

        #region ================================ METHODS

        public void Lock(string password)
        {
            _password = password;
            SubmitLock();
        }

        public void Unlock(string password)
        {
            _password = password;
            SubmitUnlock();
        }

        protected internal override void OnSetValue(T value)
        {
            _value = value;
        }

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 20)]
        [ShowIf("@_locked == false && _locking == false")]
        private void Lock()
        {
            _locking = true;
        }

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal", 60)]
        [Button("...", ButtonSizes.Medium, ButtonHeight = 20)] [PropertyOrder(2)]
        private void ShowDescription()
        {
            _showDescription = !_showDescription;
        }

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 20)]
        [ShowIf("@_locking == true")]
        [LabelText("OK")]
        private void SubmitLock()
        {
            _locking = false;
            _storedPassword = _password;
            _password = string.Empty;
            _locked = true;
        }

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 20)]
        [ShowIf("@_unlocking == true")]
        [LabelText("OK")]
        private void SubmitUnlock()
        {
            _unlocking = false;
            if (_password != _storedPassword)
            {
                _password = string.Empty;
                return;
            }
            _password = string.Empty;
            _storedPassword = string.Empty;
            _locked = false;
        }

        [BoxGroup("Box", false)] [HorizontalGroup("Box/Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 20)]
        [ShowIf("@_locked == true && _unlocking == false")]
        private void Unlock()
        {
            _unlocking = true;
        }

        #endregion

#endif
    }

    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DSecuredListFieldHorizontalButtons<T, T1> : DValueContainer<T>, IEnumerable<T1> where T : List<T1>
    {
        #region ================================ FIELDS

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [EnableIf("@_locked == false")] [ShowIf("@_locking == false && _unlocking == false")]
        [LabelText("@_label")] [OnValueChanged("@Value = _value")]
        [SerializeField]
        private T _value;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public T1 this[int index] => _value[index];

        #endregion

        #region ================================ METHODS

        public static implicit operator T(DSecuredListFieldHorizontalButtons<T, T1> field)
        {
            return field._value;
        }

        public void Add(T1 value)
        {
            _value.Add(value);
        }

        public void Clear()
        {
            _value.Clear();
        }

        public IEnumerator<T1> GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [SerializeField] [ShowIf("@(_locking == true || _unlocking == true)")]
        private string _password;

        [SerializeField] [HideInInspector]
        private string _storedPassword;

        [SerializeField] [HideInInspector]
        private bool _locking;

        [SerializeField] [HideInInspector]
        private bool _unlocking;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal override event Action<T> onChange;
        protected internal override T Value
        {
            get => _value;
            set
            {
                _value = value;
                onChange?.Invoke(_value);
            }
        }

        #endregion

        #region ================================ METHODS

        public void Lock(string password)
        {
            _password = password;
            SubmitLock();
        }

        public void Unlock(string password)
        {
            _password = password;
            SubmitUnlock();
        }

        protected internal override void OnSetValue(T value)
        {
            _value = value;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == false && _locking == false")]
        private void Lock()
        {
            _locking = true;
        }

        [HorizontalGroup("Horizontal", 20)]
        [Button("...", ButtonSizes.Medium, ButtonHeight = 23)] [PropertyOrder(2)]
        private void ShowDescription()
        {
            _showDescription = !_showDescription;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locking == true")]
        [LabelText("OK")]
        private void SubmitLock()
        {
            _locking = false;
            _storedPassword = _password;
            _password = string.Empty;
            _locked = true;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_unlocking == true")]
        [LabelText("OK")]
        private void SubmitUnlock()
        {
            _unlocking = false;
            if (_password != _storedPassword)
            {
                _password = string.Empty;
                return;
            }
            _password = string.Empty;
            _storedPassword = string.Empty;
            _locked = false;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == true && _unlocking == false")]
        private void Unlock()
        {
            _unlocking = true;
        }

        #endregion

#endif
    }

    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DSecuredFieldHorizontalButtonsNoBoxGroup<T>
    {
        #region ================================ FIELDS

        [HorizontalGroup("Horizontal")]
        [EnableIf("@_locked == false")] [ShowIf("@_locking == false && _unlocking == false && _labelEditing == false")]
        [LabelText("@_label")]
        [SerializeField]
        private T _value;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public T Value
        {
            get => _value;
#if UNITY_EDITOR
            set
            {
                _value = value;
                onChange?.Invoke();
            }
#endif
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        [SerializeField] [ShowIf("@_showDescription")]
        [PropertyOrder(-10)] [HideLabel] [TextArea]
        [DisableIf("_locked")]
        private string _description = "Field description";

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [SerializeField] [ShowIf("@(_locking == true || _unlocking == true)  && _labelEditing == false")] [LabelWidth(150)]
        private string _password;

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [ShowIf("@_labelEditing == true")] [LabelText("Enter label name...")] [LabelWidth(150)]
        [SerializeField]
        private string _label = "Field Value";

        [SerializeField] [HideInInspector]
        private bool _locked;

        [SerializeField] [HideInInspector]
        private string _storedPassword;

        [SerializeField] [HideInInspector]
        private bool _locking;

        [SerializeField] [HideInInspector]
        private bool _unlocking;

        [SerializeField] [HideInInspector]
        private bool _labelEditing;

        [SerializeField] [HideInInspector]
        private bool _showDescription;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DSecuredFieldHorizontalButtonsNoBoxGroup()
        {
        }

        public DSecuredFieldHorizontalButtonsNoBoxGroup(string label = "", string description = "")
        {
            _label = label;
            _description = description;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string Label
        {
            set => _label = value;
        }

        public string Description
        {
            set => _description = value;
        }
        public bool Locked => _locked;

        public Action onChange { get; set; }

        #endregion

        #region ================================ METHODS

        [HorizontalGroup("Horizontal", 20)]
        [Button("*", ButtonSizes.Medium, ButtonHeight = 23)] [PropertyOrder(1)]
        [DisableIf("@_locking || _unlocking || _locked")]
        private void EditLabel()
        {
            _labelEditing = !_labelEditing;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == false && _locking == false")]
        [DisableIf("@_labelEditing")]
        private void Lock()
        {
            _locking = true;
        }

        [HorizontalGroup("Horizontal", 20)]
        [Button("...", ButtonSizes.Medium, ButtonHeight = 23)] [PropertyOrder(2)]
        private void ShowDescription()
        {
            _showDescription = !_showDescription;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locking == true")]
        [LabelText("OK")]
        private void SubmitLock()
        {
            _locking = false;
            _storedPassword = _password;
            _password = string.Empty;
            _locked = true;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_unlocking == true")]
        [LabelText("OK")]
        private void SubmitUnlock()
        {
            _unlocking = false;
            if (_password != _storedPassword)
            {
                _password = string.Empty;
                return;
            }
            _password = string.Empty;
            _storedPassword = string.Empty;
            _locked = false;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == true && _unlocking == false")]
        [DisableIf("@_labelEditing")]
        private void Unlock()
        {
            _unlocking = true;
        }

        #endregion

#endif
    }

    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DSecuredFieldVerticalButtons<T>
    {
        #region ================================ FIELDS

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [EnableIf("@_locked == false")] [ShowIf("@_locking == false && _unlocking == false && _labelEditing == false")]
        [LabelText("@_label")]
        [SerializeField]
        private T _value;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public T Value
        {
            get => _value;
#if UNITY_EDITOR
            set
            {
                _value = value;
                onChange?.Invoke();
            }
#endif
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        [SerializeField] [ShowIf("@_showDescription")]
        [PropertyOrder(-10)] [HideLabel] [TextArea]
        [DisableIf("_locked")]
        private string _description = "Field description";

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [SerializeField] [ShowIf("@(_locking == true || _unlocking == true)  && _labelEditing == false")] [LabelWidth(150)]
        private string _password;

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [ShowIf("@_labelEditing == true")] [LabelText("Enter label name...")] [LabelWidth(150)]
        [SerializeField]
        private string _label = "Field Value";

        [SerializeField] [HideInInspector]
        private bool _locked;

        [SerializeField] [HideInInspector]
        private string _storedPassword;

        [SerializeField] [HideInInspector]
        private bool _locking;

        [SerializeField] [HideInInspector]
        private bool _unlocking;

        [SerializeField] [HideInInspector]
        private bool _labelEditing;

        [SerializeField] [HideInInspector]
        private bool _showDescription;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DSecuredFieldVerticalButtons()
        {
        }

        public DSecuredFieldVerticalButtons(string label = "", string description = "")
        {
            _label = label;
            _description = description;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string Label
        {
            set => _label = value;
        }
        public string Description
        {
            set => _description = value;
        }
        public bool Locked => _locked;
        public Action onChange { get; set; }

        #endregion

        #region ================================ METHODS

        [HorizontalGroup("Horizontal", 20)]
        [Button("*", ButtonSizes.Medium, ButtonHeight = 23)] [PropertyOrder(1)]
        [DisableIf("@_locking || _unlocking || _locked")]
        private void EditLabel()
        {
            _labelEditing = !_labelEditing;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == false && _locking == false")]
        [DisableIf("@_labelEditing")]
        private void Lock()
        {
            _locking = true;
        }

        [HorizontalGroup("Horizontal", 20)]
        [Button("...", ButtonSizes.Medium, ButtonHeight = 23)] [PropertyOrder(2)]
        private void ShowDescription()
        {
            _showDescription = !_showDescription;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locking == true")]
        [LabelText("OK")]
        private void SubmitLock()
        {
            _locking = false;
            _storedPassword = _password;
            _password = string.Empty;
            _locked = true;
            onChange?.Invoke();
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_unlocking == true")]
        [LabelText("OK")]
        private void SubmitUnlock()
        {
            _unlocking = false;
            if (_password != _storedPassword)
            {
                _password = string.Empty;
                return;
            }
            _password = string.Empty;
            _storedPassword = string.Empty;
            _locked = false;
            onChange?.Invoke();
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == true && _unlocking == false")]
        [DisableIf("@_labelEditing")]
        private void Unlock()
        {
            _unlocking = true;
        }

        #endregion

#endif
    }

    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DSecuredFieldNoLabelHorizontalButtons<T>
    {
        #region ================================ FIELDS

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [EnableIf("@_locked == false")] [ShowIf("@_locking == false && _unlocking == false")]
        [SerializeField] [HideLabel]
        private T _value;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
#if UNITY_EDITOR
                onChange?.Invoke();
#endif
            }
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        [SerializeField] [ShowIf("@_showDescription")]
        [PropertyOrder(-10)] [HideLabel] [TextArea]
        [DisableIf("_locked")]
        private string _description = "Field description";

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [SerializeField] [ShowIf("@_locking == true || _unlocking == true")] [LabelWidth(150)]
        private string _password;

        [SerializeField] [HideInInspector]
        private bool _locked;

        [SerializeField] [HideInInspector]
        private string _storedPassword;

        [SerializeField] [HideInInspector]
        private bool _locking;

        [SerializeField] [HideInInspector]
        private bool _unlocking;

        [SerializeField] [HideInInspector]
        private bool _showDescription;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DSecuredFieldNoLabelHorizontalButtons()
        {
        }

        public DSecuredFieldNoLabelHorizontalButtons(T defaultValue = default, string description = "")
        {
            _value = defaultValue;
            _description = description;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string Description
        {
            set => _description = value;
        }
        public bool Locked => _locked;
        public Action onChange { get; set; }

        #endregion

        #region ================================ METHODS

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == false && _locking == false")]
        private void Lock()
        {
            _locking = true;
        }

        [HorizontalGroup("Horizontal", 43)]
        [Button("...", ButtonSizes.Medium, ButtonHeight = 23)] [PropertyOrder(1)]
        private void ShowDescription()
        {
            _showDescription = !_showDescription;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locking == true")]
        [LabelText("OK")]
        private void SubmitLock()
        {
            _locking = false;
            _storedPassword = _password;
            _password = string.Empty;
            _locked = true;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_unlocking == true")]
        [LabelText("OK")]
        private void SubmitUnlock()
        {
            _unlocking = false;
            if (_password != _storedPassword)
            {
                _password = string.Empty;
                return;
            }
            _password = string.Empty;
            _storedPassword = string.Empty;
            _locked = false;
        }

        [HorizontalGroup("Horizontal", 60)]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == true && _unlocking == false")]
        private void Unlock()
        {
            _unlocking = true;
        }

        #endregion

#endif
    }

    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DSecuredFieldNoLabelVerticalButtons<T>
    {
        #region ================================ FIELDS

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [EnableIf("@_locked == false")] [ShowIf("@_locking == false && _unlocking == false")]
        [HideLabel]
        [SerializeField]
        private T _value;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public T Value
        {
            get => _value;
#if UNITY_EDITOR
            set
            {
                _value = value;
                onChanged?.Invoke();
            }
#endif
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        [SerializeField] [ShowIf("@_showDescription")]
        [PropertyOrder(-10)] [HideLabel] [TextArea]
        [DisableIf("_locked")]
        private string _description = "Field description";

        [HorizontalGroup("Horizontal")] [BoxGroup("Horizontal/Box", false)]
        [SerializeField] [ShowIf("@_locking == true || _unlocking == true")] [LabelWidth(150)]
        private string _password;

        [SerializeField] [HideInInspector]
        private bool _locked;

        [SerializeField] [HideInInspector]
        private string _storedPassword;

        [SerializeField] [HideInInspector]
        private bool _locking;

        [SerializeField] [HideInInspector]
        private bool _unlocking;

        [SerializeField] [HideInInspector]
        private bool _showDescription;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DSecuredFieldNoLabelVerticalButtons()
        {
        }

        public DSecuredFieldNoLabelVerticalButtons(T defaultValue = default, string description = "")
        {
            _value = defaultValue;
            _description = description;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string Description
        {
            set => _description = value;
        }
        public bool Locked => _locked;
        public Action onChanged { get; set; }

        #endregion

        #region ================================ METHODS

        [HorizontalGroup("Horizontal", 60)] [VerticalGroup("Horizontal/Vertical")]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == false && _locking == false")]
        private void Lock()
        {
            _locking = true;
        }

        [HorizontalGroup("Horizontal", 60)] [VerticalGroup("Horizontal/Vertical")]
        [Button("...", ButtonSizes.Medium, ButtonHeight = 23)] [PropertyOrder(1)]
        private void ShowDescription()
        {
            _showDescription = !_showDescription;
        }

        [HorizontalGroup("Horizontal", 60)] [VerticalGroup("Horizontal/Vertical")]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locking == true")]
        [LabelText("OK")]
        private void SubmitLock()
        {
            _locking = false;
            _storedPassword = _password;
            _password = string.Empty;
            _locked = true;
            onChanged?.Invoke();
        }

        [HorizontalGroup("Horizontal", 60)] [VerticalGroup("Horizontal/Vertical")]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_unlocking == true")]
        [LabelText("OK")]
        private void SubmitUnlock()
        {
            _unlocking = false;
            if (_password != _storedPassword)
            {
                _password = string.Empty;
                return;
            }
            _password = string.Empty;
            _storedPassword = string.Empty;
            _locked = false;
            onChanged?.Invoke();
        }

        [HorizontalGroup("Horizontal", 60)] [VerticalGroup("Horizontal/Vertical")]
        [Button(ButtonSizes.Medium, ButtonHeight = 23)]
        [ShowIf("@_locked == true && _unlocking == false")]
        private void Unlock()
        {
            _unlocking = true;
        }

        #endregion

#endif
    }
}