using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Factories
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class DContainerFactoryObject
    {
        #region ================================ FIELDS

        internal readonly bool Bind;
        internal readonly object Object;
        internal readonly string InstanceID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DContainerFactoryObject(object obj, bool bind, string instanceID)
        {
            Object = obj;
            Bind = bind;
            InstanceID = instanceID;
        }

        #endregion
    }
}