using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class NotNullAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly float FloatTolerance;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public NotNullAttribute(float tolerance)
        {
            FloatTolerance = tolerance;
        }

        #endregion
    }
}