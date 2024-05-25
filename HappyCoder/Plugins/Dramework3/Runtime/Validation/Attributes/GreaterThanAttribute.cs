using System;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;


namespace IG.HappyCoder.Dramework3.Runtime.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class GreaterThanAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly int IntValue;
        public readonly float FloatValue;

        internal readonly TypeOfValue ValueType;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public GreaterThanAttribute(int intValue)
        {
            ValueType = TypeOfValue.Int;
            IntValue = intValue;
        }

        public GreaterThanAttribute(float floatValue)
        {
            ValueType = TypeOfValue.Float;
            FloatValue = floatValue;
        }

        #endregion
    }
}