using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class ObjectExtensions
    {
        #region ================================ METHODS

        public static T DeepCopy<T>(this T self)
        {
            if (typeof(T).IsSerializable == false)
                throw new ArgumentException("Type must be serializable!");

            if (ReferenceEquals(self, null))
                throw new Exception("Object is null!");

            var formatter = new BinaryFormatter();
            using var memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, self);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(memoryStream);
        }

        public static void IfNotNull<T>(this T obj, Action<T> callback)
        {
            if (obj != null) callback.Invoke(obj);
        }

        public static void IfNull(this object obj, Action callback)
        {
            if (obj == null) callback.Invoke();
        }

        #endregion
    }
}