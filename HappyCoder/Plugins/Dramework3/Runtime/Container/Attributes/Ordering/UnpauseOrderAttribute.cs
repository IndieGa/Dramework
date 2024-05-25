using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Ordering
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class UnpauseOrderAttribute : OrderAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public UnpauseOrderAttribute(int order, int offset = 0) : base(order, offset)
        {
        }

        #endregion
    }
}