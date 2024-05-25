using System.Diagnostics.CodeAnalysis;
using System.IO;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.SaveManagement.SQLiteManagement
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class DSQLiteStorageSettings
    {
        #region ================================ FIELDS

        private readonly string _directory;
        private readonly string _databaseName;
        private readonly bool _encrypt;
        private readonly string _password;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DSQLiteStorageSettings(
            string directory,
            string databaseName,
            bool encrypt,
            string password = ""
        )
        {
            _directory = directory;
            _databaseName = databaseName;
            _encrypt = encrypt;
            _password = password;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string ConnectionString => _encrypt ? $"Filename={FullPath};Password={_password}" : $"Filename={FullPath}";
        private string FullPath => Path.Combine(Application.persistentDataPath, _directory, _databaseName);

        #endregion
    }
}