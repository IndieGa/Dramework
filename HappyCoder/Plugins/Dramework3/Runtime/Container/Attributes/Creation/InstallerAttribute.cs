using System;
using System.Diagnostics.CodeAnalysis;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation
{
    [AttributeUsage(AttributeTargets.Class)]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class InstallerAttribute : Attribute
    {
        #region ================================ FIELDS

        public readonly string SceneID;
        public readonly int Order;
        public readonly string ModuleName;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public InstallerAttribute(string sceneID, int order, string moduleName)
        {
            SceneID = sceneID;
            Order = order;
            ModuleName = moduleName;
        }

        #endregion
    }
}