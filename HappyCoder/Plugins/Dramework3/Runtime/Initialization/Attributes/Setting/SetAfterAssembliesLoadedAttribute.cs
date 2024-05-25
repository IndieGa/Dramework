using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Setting
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class SetAfterAssembliesLoadedAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly object Value;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public SetAfterAssembliesLoadedAttribute(object value)
        {
            Value = value;
        }

        #endregion
    }
}