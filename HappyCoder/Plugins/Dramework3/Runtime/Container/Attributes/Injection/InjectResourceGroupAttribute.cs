using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class InjectResourceGroupAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string GroupID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public InjectResourceGroupAttribute(string groupID)
        {
            GroupID = groupID;
        }

        #endregion
    }
}