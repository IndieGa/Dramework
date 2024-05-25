using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class InjectResourceAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string GroupID;
        public readonly string ResourceID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public InjectResourceAttribute(string resourceID)
        {
            var split = resourceID.Split('~');
            GroupID = split[0];
            ResourceID = split[1];
        }

        #endregion
    }
}