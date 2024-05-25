using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;
using IG.HappyCoder.Dramework3.Runtime.Validation.Interfaces;

using Sirenix.OdinInspector;

using UnityEngine;

using Object = UnityEngine.Object;


#pragma warning disable 414


namespace IG.HappyCoder.Dramework3.Runtime.Experimental.Types
{
    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public class DEnumField<T> : IValidable where T : Enum
    {
        #region ================================ FIELDS

        [BoxGroup("Box", false)]
        [LabelText("@_label")]
        [SerializeField] [OnValueChanged("OnValueChanged")]
        private T _value;

        [SerializeField] [HideInInspector]
        private Object _owner;
        [SerializeField] [HideInInspector]
        private string _valueName;
        [SerializeField] [HideInInspector]
        private string _label = "Value";

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DEnumField()
        {
        }

        public DEnumField(Object owner, string label = "Value")
        {
            _owner = owner;
            _label = label;
        }

        #endregion

        #region ================================ METHODS

        public static implicit operator T(DEnumField<T> field)
        {
            return field._value;
        }

        void IValidable.Validate()
        {
#if UNITY_EDITOR
            Validate();
#endif
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ METHODS

        private void Validate()
        {
            var valueNames = Enum.GetNames(typeof(T));
            if (valueNames.All(n => n != _valueName))
            {
                var fieldName = Helpers.EditorTools.GetFieldName(_owner, this);
                ConsoleLogger.LogError($"The enum field «{fieldName}» in object «{_owner}» has incorrect value", "", _owner);
                return;
            }

            var values = Enum.GetValues(typeof(T));
            for (var i = 0; i < valueNames.Length; i++)
            {
                if (valueNames[i] != _valueName) continue;
                _value = (T)values.GetValue(i);
                _valueName = _value.ToString();
                break;
            }
        }

        private void OnValueChanged()
        {
            _valueName = _value.ToString();
        }

        #endregion

#endif
    }
}