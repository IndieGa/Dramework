using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class ContainerExtensions
    {
        #region ================================ METHODS

        public static T Bind<T>(this T obj, string containerName = null, string instanceID = null)
        {
            DCore.Bind(obj, containerName, instanceID);
            return obj;
        }

        public static T Initialize<T>(this T obj)
        {
            DCore.InitializeContainerObject(obj);
            ConsoleLogger.Log($"Object type of «{obj.GetType()}» is initialized", $"[{nameof(ContainerExtensions)}.{nameof(Initialize)}()]");
            return obj;
        }

        public static T Unbind<T>(this T obj, string containerName)
        {
            DCore.Unbind(obj, containerName);
            return obj;
        }

        #endregion
    }
}