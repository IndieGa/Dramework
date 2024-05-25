using System.Collections.Generic;
using System.IO;

using IG.HappyCoder.Dramework3.Runtime.Core;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Editor
{
    internal class EditorStorage : DConfig
    {
        #region ================================ FIELDS

        private static EditorStorage _storage;

        [SerializeField] [ReadOnly]
        private List<Object> _bookmarks;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static EditorStorage Storage => GetStorage();
        internal List<Object> Bookmarks => _bookmarks;

        #endregion

        #region ================================ METHODS

        internal static void AddBookmark(Object obj)
        {
            if (Storage._bookmarks.Contains(obj)) return;
            Storage._bookmarks.Add(obj);
            EditorUtility.SetDirty(Storage);
            AssetDatabase.SaveAssetIfDirty(Storage);
        }

        internal static EditorStorage GetStorage()
        {
            if (_storage != null) return _storage;

            _storage = GetConfig<EditorStorage>(skipLog: true);
            if (_storage != null) return _storage;

            var logo = Resources.Load<Texture>("D Framework 3 Logo Dark");
            var path = AssetDatabase.GetAssetPath(logo).Replace("D Framework 3 Logo Dark.png", "");
            _storage = CreateInstance<EditorStorage>();
            AssetDatabase.CreateAsset(_storage, Path.Combine(path, "Editor Storage.asset"));
            EditorUtility.SetDirty(_storage);
            AssetDatabase.SaveAssetIfDirty(_storage);

            return _storage;
        }

        #endregion
    }
}