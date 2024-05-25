using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class DInstallData
    {
        #region ================================ FIELDS

        public readonly object Object;
        public readonly bool Bind;
        public readonly string InstanceID;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DInstallData(object obj, bool bind, string instanceID)
        {
            Object = obj;
            Bind = bind;
            InstanceID = instanceID;
        }

        #endregion
    }
}