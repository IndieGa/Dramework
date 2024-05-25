using System.Reflection;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Bootstraps
{
    public static partial class DBootstrap
    {
        #region ================================ NESTED TYPES

        private class SetFieldData
        {
            #region ================================ FIELDS

            public readonly FieldInfo FieldInfo;
            public readonly object Value;

            #endregion

            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            public SetFieldData(FieldInfo fieldInfo, object value)
            {
                FieldInfo = fieldInfo;
                Value = value;
            }

            #endregion
        }

        private class SetPropertyData
        {
            #region ================================ FIELDS

            public readonly PropertyInfo PropertyInfo;
            public readonly object Value;

            #endregion

            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            public SetPropertyData(PropertyInfo propertyInfo, object value)
            {
                PropertyInfo = propertyInfo;
                Value = value;
            }

            #endregion
        }

        #endregion
    }
}