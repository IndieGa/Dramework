using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CreateModelAttribute : CreateAttribute
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public CreateModelAttribute() : base(0)
        {
        }

        public CreateModelAttribute(int order) : base(order)
        {
        }

        #endregion
    }
}