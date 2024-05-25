using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Ordering
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class OrderAttribute : Attribute
    {
        #region ================================ FIELDS

        private readonly int _order;
        private readonly int _offset;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        protected OrderAttribute(int order, int offset = 0)
        {
            _order = order;
            _offset = offset;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public int Order => _order + _offset;

        #endregion
    }
}