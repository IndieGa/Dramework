using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class TypeExtensions
    {
        #region ================================ METHODS

        public static bool EqualOrBase(this Type objectType, Type type)
        {
            return type == objectType || objectType.IsAssignableFrom(type);
        }

        public static bool EqualOrHeir(this Type objectType, Type type)
        {
            return type == objectType || type.IsAssignableFrom(objectType);
        }

        public static Type GetBaseType(this Type type)
        {
            while (true)
            {
                if (type.BaseType == null) return type;
                type = type.BaseType;
            }
        }

        public static FieldInfo GetFieldFromAll(this Type type, string fieldName, BindingFlags bindingFlags)
        {
            FieldInfo fieldInfo = null;
            while (type != null)
            {
                fieldInfo = type.GetField(fieldName, bindingFlags);
                if (fieldInfo != null) break;
                type = type.BaseType;
            }
            return fieldInfo;
        }

        public static FieldInfo[] GetFieldsFromAll(this Type type, BindingFlags bindingFlags)
        {
            var fieldInfos = new List<FieldInfo>();
            while (type != null)
            {
                fieldInfos.AddRange(type.GetFields(bindingFlags));
                type = type.BaseType;
            }
            return fieldInfos.ToArray();
        }

        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                return true;

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }

        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        #endregion
    }
}