using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class InjectInstanceAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string InstanceID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public InjectInstanceAttribute(string instanceID)
        {
            InstanceID = instanceID;
        }

        #endregion
    }
}