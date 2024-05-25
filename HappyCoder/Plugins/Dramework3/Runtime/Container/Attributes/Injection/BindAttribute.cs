using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class BindAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string InstanceID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public BindAttribute()
        {
        }

        public BindAttribute(string instanceID = null)
        {
            InstanceID = instanceID;
        }

        #endregion
    }
}