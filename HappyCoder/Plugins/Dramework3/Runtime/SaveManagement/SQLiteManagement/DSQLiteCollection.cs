using System.Diagnostics.CodeAnalysis;

using LiteDB;


namespace IG.HappyCoder.Dramework3.Runtime.SaveManagement.SQLiteManagement
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DSQLiteCollection<T>
    {
        #region ================================ FIELDS

        public readonly ILiteCollection<T> Value;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DSQLiteCollection(ILiteCollection<T> collection)
        {
            Value = collection;
        }

        #endregion
    }
}