using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Injection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class InstantiateAssetAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string GroupID;
        public readonly string ResourceID;
        public readonly bool Active;
        public readonly bool InstantiateInWorldSpace;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public InstantiateAssetAttribute(string resourceID, bool instantiateInWorldSpace = true, bool active = true)
        {
            var split = resourceID.Split('~');
            GroupID = split[0];
            ResourceID = split[1];
            InstantiateInWorldSpace = instantiateInWorldSpace;
            Active = active;
        }

        #endregion
    }
}