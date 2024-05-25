using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class GetComponentInChildrenAttribute : GetComponentAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GetComponentInChildrenAttribute()
        {
        }

        public GetComponentInChildrenAttribute(bool log = true) : base(log)
        {
        }

        public GetComponentInChildrenAttribute(bool ignoreName = false, bool log = true) : base(ignoreName, log)
        {
        }

        public GetComponentInChildrenAttribute(bool includingThisObject, bool ignoreName = false, bool log = true) : base(includingThisObject, ignoreName, log)
        {
        }

        public GetComponentInChildrenAttribute(string objectName, bool log = true) : base(objectName, log)
        {
        }

        public GetComponentInChildrenAttribute(bool includingThisObject, string objectName, bool log = true) : base(includingThisObject, objectName, log)
        {
        }

        #endregion
    }
}