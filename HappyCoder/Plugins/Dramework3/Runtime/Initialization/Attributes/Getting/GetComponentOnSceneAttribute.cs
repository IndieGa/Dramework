using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class GetComponentOnSceneAttribute : GetComponentAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GetComponentOnSceneAttribute()
        {
        }

        public GetComponentOnSceneAttribute(bool log = true) : base(log)
        {
        }

        public GetComponentOnSceneAttribute(bool ignoreName = false, bool log = true) : base(ignoreName, log)
        {
        }

        public GetComponentOnSceneAttribute(bool includingThisObject, bool ignoreName = false, bool log = true) : base(includingThisObject, ignoreName, log)
        {
        }

        public GetComponentOnSceneAttribute(string objectName, bool log = true) : base(objectName, log)
        {
        }

        public GetComponentOnSceneAttribute(bool includingThisObject, string objectName, bool log = true) : base(includingThisObject, objectName, log)
        {
        }

        #endregion
    }
}