using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Validation.Attributes;
using IG.HappyCoder.Dramework3.Runtime.Validation.Interfaces;

using Sirenix.OdinInspector;

using UnityEngine;

using NotNullAttribute = IG.HappyCoder.Dramework3.Runtime.Validation.Attributes.NotNullAttribute;


namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    [HideMonoScript]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public abstract class DConfig : DScriptableObject, IValidable
    {
        #region ================================ METHODS

        public T Create<T>() where T : DConfig
        {
            return (T)Instantiate(this);
        }

        void IValidable.Validate()
        {
#if UNITY_EDITOR
            var fieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fieldInfo in fieldInfos)
            {
                // валидируем только приватные, сериализуемые поля
                if (fieldInfo.IsPrivate && fieldInfo.GetCustomAttribute<SerializeField>() == null) continue;

                ValidateStringField(fieldInfo);
                ValidateIntField(fieldInfo);
                ValidateFloatField(fieldInfo);
                ValidateBoolField(fieldInfo);
                ValidateVector2Field(fieldInfo);
                ValidateVector3Field(fieldInfo);
            }
            Validate();
#endif
        }

        #endregion

#if UNITY_EDITOR

        public static T GetConfig<T>(string configName = "", string[] defaultPath = null, bool skipLog = false) where T : DConfig
        {
            return Helpers.EditorTools.LoadAsset<T>(configName, defaultPath, skipLog);
        }

        public static IEnumerable<T> GetConfigs<T>(string[] defaultPaths = null) where T : DConfig
        {
            return Helpers.EditorTools.LoadAssets<T>(string.Empty, defaultPaths);
        }

        protected virtual void Validate()
        {
        }

        private void ValidateStringField(FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType != typeof(string)) return;
            var value = (string)fieldInfo.GetValue(this);

            var notNullOrEmptyAttribute = fieldInfo.GetCustomAttribute<NotNullOrEmptyAttribute>();
            if (notNullOrEmptyAttribute != null && value.IsNullOrEmpty())
            {
                Debug.LogError($"Field «{fieldInfo.Name}» is null or empty", this);
                return;
            }

            var equalAttribute = fieldInfo.GetCustomAttribute<EqualAttribute>();
            if (equalAttribute is { ValueType: TypeOfValue.String } && value.NotEqual(equalAttribute.StringValue))
                Debug.LogError($"Value of field «{fieldInfo.Name}» is not equal «{equalAttribute.StringValue}»", this);

            var notEqualAttribute = fieldInfo.GetCustomAttribute<NotEqualAttribute>();
            if (notEqualAttribute is { ValueType: TypeOfValue.String } && value.Equal(notEqualAttribute.StringValue))
                Debug.LogError($"Value of field «{fieldInfo.Name}» is equal «{notEqualAttribute.StringValue}»", this);
        }

        private void ValidateIntField(FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType != typeof(int)) return;

            var value = (int)fieldInfo.GetValue(this);
            NotNull();
            Equal();
            NotEqual();
            Less();
            LessOrEqual();
            Greater();
            GreaterOrEqual();

            void NotNull()
            {
                var notNullAttribute = fieldInfo.GetCustomAttribute<NotNullAttribute>();
                if (notNullAttribute != null && value.Equal(0))
                    Debug.LogError($"Field «{fieldInfo.Name}» is equal zero", this);
            }

            void Equal()
            {
                var equalAttribute = fieldInfo.GetCustomAttribute<EqualAttribute>();
                if (equalAttribute is { ValueType: TypeOfValue.Int } && value.NotEqual(equalAttribute.IntValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not equal «{equalAttribute.IntValue}»", this);
            }

            void NotEqual()
            {
                var notEqualAttribute = fieldInfo.GetCustomAttribute<NotEqualAttribute>();
                if (notEqualAttribute is { ValueType: TypeOfValue.Int } && value.Equal(notEqualAttribute.IntValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is equal «{notEqualAttribute.IntValue}»", this);
            }

            void Less()
            {
                var lessAttribute = fieldInfo.GetCustomAttribute<LessThanAttribute>();
                if (lessAttribute is { ValueType: TypeOfValue.Int } && value.GreaterOrEqual(lessAttribute.IntValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not less than «{lessAttribute.IntValue}»", this);
            }

            void LessOrEqual()
            {
                var lessOrEqualAttribute = fieldInfo.GetCustomAttribute<LessOrEqualAttribute>();
                if (lessOrEqualAttribute is { ValueType: TypeOfValue.Int } && value.GreaterThan(lessOrEqualAttribute.IntValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not less or equal «{lessOrEqualAttribute.IntValue}»", this);
            }

            void Greater()
            {
                var greaterAttribute = fieldInfo.GetCustomAttribute<GreaterThanAttribute>();
                if (greaterAttribute is { ValueType: TypeOfValue.Int } && value.LessOrEqual(greaterAttribute.IntValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not greater than «{greaterAttribute.IntValue}»", this);
            }

            void GreaterOrEqual()
            {
                var greaterOrEqualAttribute = fieldInfo.GetCustomAttribute<GreaterOrEqualAttribute>();
                if (greaterOrEqualAttribute is { ValueType: TypeOfValue.Int } && value.LessThan(greaterOrEqualAttribute.IntValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not greater or equal «{greaterOrEqualAttribute.IntValue}»", this);
            }
        }

        private void ValidateFloatField(FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType != typeof(float)) return;

            var value = (float)fieldInfo.GetValue(this);
            NotNull();
            Equal();
            NotEqual();
            Less();
            LessOrEqual();
            Greater();
            GreaterOrEqual();

            void NotNull()
            {
                var notNullAttribute = fieldInfo.GetCustomAttribute<NotNullAttribute>();
                if (notNullAttribute != null && value.Equal(0, notNullAttribute.FloatTolerance))
                    Debug.LogError($"Field «{fieldInfo.Name}» is equal zero", this);
            }

            void Equal()
            {
                var equalAttribute = fieldInfo.GetCustomAttribute<EqualAttribute>();
                if (equalAttribute is { ValueType: TypeOfValue.Int } && value.NotEqual(equalAttribute.FloatValue, equalAttribute.FloatTolerance))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not equal «{equalAttribute.FloatValue}»", this);
            }

            void NotEqual()
            {
                var notEqualAttribute = fieldInfo.GetCustomAttribute<NotEqualAttribute>();
                if (notEqualAttribute is { ValueType: TypeOfValue.Int } && value.Equal(notEqualAttribute.FloatValue, notEqualAttribute.FloatTolerance))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is equal «{notEqualAttribute.FloatValue}»", this);
            }

            void Less()
            {
                var lessAttribute = fieldInfo.GetCustomAttribute<LessThanAttribute>();
                if (lessAttribute is { ValueType: TypeOfValue.Int } && value.GreaterOrEqual(lessAttribute.FloatValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not less than «{lessAttribute.FloatValue}»", this);
            }

            void LessOrEqual()
            {
                var lessOrEqualAttribute = fieldInfo.GetCustomAttribute<LessOrEqualAttribute>();
                if (lessOrEqualAttribute is { ValueType: TypeOfValue.Int } && value.Greater(lessOrEqualAttribute.FloatValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not less or equal «{lessOrEqualAttribute.FloatValue}»", this);
            }

            void Greater()
            {
                var greaterAttribute = fieldInfo.GetCustomAttribute<GreaterThanAttribute>();
                if (greaterAttribute is { ValueType: TypeOfValue.Int } && value.LessOrEqual(greaterAttribute.FloatValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not greater than «{greaterAttribute.FloatValue}»", this);
            }

            void GreaterOrEqual()
            {
                var greaterOrEqualAttribute = fieldInfo.GetCustomAttribute<GreaterOrEqualAttribute>();
                if (greaterOrEqualAttribute is { ValueType: TypeOfValue.Int } && value.Less(greaterOrEqualAttribute.FloatValue))
                    Debug.LogError($"Value of field «{fieldInfo.Name}» is not greater or equal «{greaterOrEqualAttribute.FloatValue}»", this);
            }
        }

        private void ValidateBoolField(FieldInfo fieldInfo)
        {
            // ReSharper disable once RedundantJumpStatement
            if (fieldInfo.FieldType != typeof(bool)) return;
        }

        private void ValidateVector2Field(FieldInfo fieldInfo)
        {
            // ReSharper disable once RedundantJumpStatement
            if (fieldInfo.FieldType != typeof(Vector2)) return;
        }

        private void ValidateVector3Field(FieldInfo fieldInfo)
        {
            // ReSharper disable once RedundantJumpStatement
            if (fieldInfo.FieldType != typeof(Vector3)) return;
        }

#endif
    }
}