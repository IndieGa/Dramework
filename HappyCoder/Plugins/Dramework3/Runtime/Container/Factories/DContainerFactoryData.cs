using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class DContainerFactoryData
    {
        #region ================================ FIELDS

        public readonly int CreationOrder;
        public readonly Func<DContainerFactoryObject> FactoryObject;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DContainerFactoryData(int creationOrder, Func<DContainerFactoryObject> factoryObject)
        {
            CreationOrder = creationOrder;
            FactoryObject = factoryObject;
        }

        #endregion
    }
}