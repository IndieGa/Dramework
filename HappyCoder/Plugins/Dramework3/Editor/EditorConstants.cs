using System.IO;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Editor
{
    internal static class EditorConstants
    {
        #region ================================ FIELDS

        internal static readonly string CONFIGS_DB_FOLDER = Path.Combine(Application.dataPath, Application.productName, "Databases");
        internal static readonly string CONFIGS_DB_FILENAME = "configs.db";

        #endregion
    }
}