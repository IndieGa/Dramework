using System;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;


namespace IG.HappyCoder.Dramework3.Runtime.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class EqualAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string StringValue;
        public readonly int IntValue;
        public readonly float FloatValue;
        public readonly float FloatTolerance;

        internal readonly TypeOfValue ValueType;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public EqualAttribute(string stringValue)
        {
            ValueType = TypeOfValue.String;
            StringValue = stringValue;
        }

        public EqualAttribute(int intValue)
        {
            ValueType = TypeOfValue.Int;
            IntValue = intValue;
        }

        public EqualAttribute(float floatValue, float tolerance)
        {
            ValueType = TypeOfValue.Float;
            FloatValue = floatValue;
            FloatTolerance = tolerance;
        }

        #endregion
    }
}