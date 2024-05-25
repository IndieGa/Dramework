using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateFactoryAttribute : CreateAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public CreateFactoryAttribute() : base(0)
        {
        }

        public CreateFactoryAttribute(int order) : base(order)
        {
        }

        #endregion
    }
}