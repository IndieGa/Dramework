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
    public class LessOrEqualAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly int IntValue;
        public readonly float FloatValue;

        internal readonly TypeOfValue ValueType;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public LessOrEqualAttribute(int intValue)
        {
            ValueType = TypeOfValue.Int;
            IntValue = intValue;
        }

        public LessOrEqualAttribute(float floatValue)
        {
            ValueType = TypeOfValue.Float;
            FloatValue = floatValue;
        }

        #endregion
    }
}