using System;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;


namespace IG.HappyCoder.Dramework3.Runtime.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class NotEqualAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string StringValue;
        public readonly int IntValue;
        public readonly float FloatValue;
        public readonly float FloatTolerance;

        internal readonly TypeOfValue ValueType;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public NotEqualAttribute(string stringValue)
        {
            ValueType = TypeOfValue.String;
            StringValue = stringValue;
        }

        public NotEqualAttribute(int intValue)
        {
            ValueType = TypeOfValue.Int;
            IntValue = intValue;
        }

        public NotEqualAttribute(float floatValue, float tolerance)
        {
            ValueType = TypeOfValue.Float;
            FloatValue = floatValue;
            FloatTolerance = tolerance;
        }

        #endregion
    }
}