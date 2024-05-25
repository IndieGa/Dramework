using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class GetComponentAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly bool IncludingThisObject;
        public readonly string ObjectName;
        public readonly bool IgnoreName;

        public readonly bool Log;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GetComponentAttribute()
        {
        }

        public GetComponentAttribute(bool log = true)
        {
            Log = log;
        }

        public GetComponentAttribute(bool ignoreName = false, bool log = true)
        {
            IgnoreName = ignoreName;
            Log = log;
        }

        public GetComponentAttribute(bool includingThisObject, bool ignoreName = false, bool log = true)
        {
            IncludingThisObject = includingThisObject;
            IgnoreName = ignoreName;
            Log = log;
        }

        public GetComponentAttribute(string objectName, bool log = true)
        {
            ObjectName = objectName;
            Log = log;
        }

        public GetComponentAttribute(bool includingThisObject, string objectName, bool log = true)
        {
            IncludingThisObject = includingThisObject;
            ObjectName = objectName;
            Log = log;
        }

        #endregion
    }
}