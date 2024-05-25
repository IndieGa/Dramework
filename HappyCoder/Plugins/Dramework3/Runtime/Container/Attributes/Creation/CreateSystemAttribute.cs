using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateSystemAttribute : CreateAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public CreateSystemAttribute() : base(0)
        {
        }

        public CreateSystemAttribute(int order) : base(order)
        {
        }

        #endregion
    }
}