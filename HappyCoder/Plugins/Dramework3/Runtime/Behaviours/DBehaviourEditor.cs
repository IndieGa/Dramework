#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;
using IG.HappyCoder.Dramework3.Runtime.Validation.Attributes;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Behaviours
{
    public partial class DBehaviour
    {
        #region ================================ FIELDS

        [SerializeField] [HideInInspector]
        private bool _isInitializedInEditor;
        [SerializeField] [HideInInspector]
        private Texture _logoTexture;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private Texture LogoTexture
        {
            get
            {
                if (_logoTexture != null) return _logoTexture;
                return _logoTexture = Resources.Load<Texture>(EditorGUIUtility.isProSkin ? "D Framework 3 Logo Dark" : "D Framework 3 Logo Light");
            }
        }

        #endregion

        #region ================================ METHODS

        [ContextMenu("Initialize")]
        protected void InitializeFields()
        {
            InitializeObject(GetType());
            OnEditorInitialize();
            ApplyPrefabInstance();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        protected virtual void OnEditorInitialize()
        {
        }

        private void ApplyPrefabInstance()
        {
            if (gameObject == null
                || gameObject.activeInHierarchy == false
                || PrefabUtility.IsPartOfAnyPrefab(gameObject) == false) return;

            PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.UserAction);
        }

        [OnInspectorGUI]
        [PropertyOrder(-10000)]
        private void DrawHeader()
        {
            GUILayout.Label(LogoTexture);
        }

        private void InitializeArray(FieldInfo fieldInfo, Type fieldType, GetComponentAttribute attribute)
        {
            var elementType = fieldType.GetElementType();
            if (elementType == null) return;
            var components = attribute switch
            {
                GetComponentInChildrenAttribute => GetComponentsInChildren(elementType, true)
                    .Where(component =>
                    {
                        if (attribute.IncludingThisObject) return component;
                        return component.gameObject != gameObject;
                    })
                    .ToArray(),
                GetComponentInParentAttribute => GetComponentsInParent(elementType, true)
                    .Where(component =>
                    {
                        if (attribute.IncludingThisObject) return component;
                        return component.gameObject != gameObject;
                    })
                    .ToArray(),
                GetComponentOnSceneAttribute => gameObject.activeInHierarchy
                    ? gameObject.scene.GetRootGameObjects()
                        .SelectMany(ro => ro.GetComponentsInChildren(elementType, true))
                        .Where(component =>
                        {
                            if (attribute.IncludingThisObject) return component;
                            return component.gameObject != gameObject;
                        })
                        .ToArray()
                    : null,
                not null => GetComponents(elementType),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), null, null)
            };

            if (components == null) return;

            if (components.Length == 0)
            {
                if (attribute.Log)
                    ConsoleLogger.LogError($"No one component type of «{elementType}» for field «{fieldInfo.Name}» in object type of «{GetType()}» is not found", "", this);

                fieldInfo.SetValue(this, null);
                return;
            }

            var array = (IList)Array.CreateInstance(elementType, components.Length);
            for (var i = 0; i < array.Count; i++)
                array[i] = components[i];
            fieldInfo.SetValue(this, array);
        }

        private void InitializeList(FieldInfo fieldInfo, Type fieldType, GetComponentAttribute attribute)
        {
            var elementType = fieldType.GetGenericArguments()[0];
            var components = attribute switch
            {
                GetComponentInChildrenAttribute => GetComponentsInChildren(elementType, true)
                    .Where(component =>
                    {
                        if (attribute.IncludingThisObject) return component;
                        return component.gameObject != gameObject;
                    })
                    .ToArray(),
                GetComponentInParentAttribute => GetComponentsInParent(elementType, true)
                    .Where(component =>
                    {
                        if (attribute.IncludingThisObject) return component;
                        return component.gameObject != gameObject;
                    })
                    .ToArray(),
                GetComponentOnSceneAttribute => gameObject.activeInHierarchy
                    ? gameObject.scene.GetRootGameObjects()
                        .SelectMany(ro => ro.GetComponentsInChildren(elementType, true))
                        .Where(component =>
                        {
                            if (attribute.IncludingThisObject) return component;
                            return component.gameObject != gameObject;
                        })
                        .ToArray()
                    : Array.Empty<Component>(),
                not null => GetComponents(elementType),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), null, null)
            };

            if (components.Length == 0)
            {
                if (attribute.Log)
                    ConsoleLogger.LogError($"No one component type of «{fieldInfo.FieldType}» for field «{fieldInfo.Name}» in object type of «{GetType()}» is not found", "", this);

                fieldInfo.SetValue(this, null);
                return;
            }

            var constructedListType = typeof(List<>).MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(constructedListType);
            for (var i = 0; i < components.Length; i++)
                list.Add(components[i]);
            fieldInfo.SetValue(this, list);
        }

        private void InitializeObject()
        {
            if (this == null || (gameObject != null && gameObject.scene.name == null) || _isInitializedInEditor) return;
            InitializeObject(GetType());
            OnEditorInitialize();
            _isInitializedInEditor = true;
        }

        private void InitializeObject(Type type)
        {
            var baseType = type.BaseType;
            if (baseType != null && baseType != typeof(DBehaviour))
                InitializeObject(baseType);

            var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fieldInfo in fieldInfos)
            {
                // инициализируем и валидируем только приватные, сериализуемые поля
                if (fieldInfo.IsPrivate && fieldInfo.GetCustomAttribute<SerializeField>() == null) continue;

                // пробуем получить любой атрибут, унаследованный от FindOnAttribute
                var findOnAttribute = fieldInfo.GetCustomAttribute<GetComponentAttribute>();
                if (findOnAttribute != null)
                {
                    var fieldType = fieldInfo.FieldType;
                    if (fieldType.IsArray)
                        InitializeArray(fieldInfo, fieldType, findOnAttribute);
                    else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                        InitializeList(fieldInfo, fieldType, findOnAttribute);
                    else
                        InitializeObject(fieldInfo, findOnAttribute);
                }

                // валидируем значение поля
                ValidateStringField(fieldInfo);
                ValidateIntField(fieldInfo);
                ValidateFloatField(fieldInfo);
                // ValidateBoolField(fieldInfo);
                // ValidateVector2Field(fieldInfo);
                // ValidateVector3Field(fieldInfo);
            }
        }

        private void InitializeObject(FieldInfo fieldInfo, GetComponentAttribute attribute)
        {
            if (gameObject == null) return;

            var component = attribute switch
            {
                GetComponentInChildrenAttribute => GetComponentsInChildren(fieldInfo.FieldType, true)
                    .Where(c =>
                    {
                        if (attribute.IncludingThisObject) return true;
                        return c.gameObject != gameObject;
                    })
                    .FirstOrDefault(c =>
                    {
                        if (attribute.IgnoreName) return c;
                        return string.Equals(Helpers.StringTools.ClearText(c.name), string.IsNullOrEmpty(attribute.ObjectName)
                            ? Helpers.StringTools.ClearText(fieldInfo.Name)
                            : Helpers.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase);
                    }),

                GetComponentInParentAttribute => GetComponentsInParent(fieldInfo.FieldType, true)
                    .Where(c =>
                    {
                        if (attribute.IncludingThisObject) return true;
                        return c.gameObject != gameObject;
                    })
                    .FirstOrDefault(c =>
                    {
                        if (attribute.IgnoreName) return c;
                        return string.Equals(Helpers.StringTools.ClearText(c.name), string.IsNullOrEmpty(attribute.ObjectName)
                            ? Helpers.StringTools.ClearText(fieldInfo.Name)
                            : Helpers.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase);
                    }),

                GetComponentOnSceneAttribute => gameObject.scene.GetRootGameObjects()
                    .SelectMany(ro => ro.GetComponentsInChildren(fieldInfo.FieldType, true))
                    .Where(c =>
                    {
                        if (attribute.IncludingThisObject) return true;
                        return c.gameObject != gameObject;
                    })
                    .FirstOrDefault(c =>
                    {
                        if (attribute.IgnoreName) return c;
                        return string.Equals(Helpers.StringTools.ClearText(c.name), string.IsNullOrEmpty(attribute.ObjectName)
                            ? Helpers.StringTools.ClearText(fieldInfo.Name)
                            : Helpers.StringTools.ClearText(attribute.ObjectName), StringComparison.CurrentCultureIgnoreCase);
                    }),

                not null => GetComponent(fieldInfo.FieldType),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), null, null)
            };

            if (component == null)
            {
                if (attribute.Log)
                    ConsoleLogger.LogError($"Component type of «{fieldInfo.FieldType}» for field «{fieldInfo.Name}» " +
                                           $"in object type of «{GetType()}» is not found " +
                                           $"by name «{(string.IsNullOrEmpty(attribute.ObjectName) ? fieldInfo.Name : attribute.ObjectName)}»", "", this);
                return;
            }
            fieldInfo.SetValue(this, component);
        }

        private void Reset()
        {
            InitializeFields();
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

        #endregion
    }
}

#endif