using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class GetComponentInParentAttribute : GetComponentAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GetComponentInParentAttribute()
        {
        }

        public GetComponentInParentAttribute(bool log = true) : base(log)
        {
        }

        public GetComponentInParentAttribute(bool ignoreName = false, bool log = true) : base(ignoreName, log)
        {
        }

        public GetComponentInParentAttribute(bool includingThisObject, bool ignoreName = false, bool log = true) : base(includingThisObject, ignoreName, log)
        {
        }

        public GetComponentInParentAttribute(string objectName, bool log = true) : base(objectName, log)
        {
        }

        public GetComponentInParentAttribute(bool includingThisObject, string objectName, bool log = true) : base(includingThisObject, objectName, log)
        {
        }

        #endregion
    }
}