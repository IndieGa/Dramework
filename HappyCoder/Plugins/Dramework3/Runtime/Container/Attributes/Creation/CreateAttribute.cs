using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly int Order;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        protected CreateAttribute(int order)
        {
            Order = order;
        }

        #endregion
    }
}