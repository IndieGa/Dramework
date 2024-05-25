using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Dramework3.Runtime.Experimental.Interfaces.Common;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using LiteDB;


namespace IG.HappyCoder.Dramework3.Runtime.SaveManagement.SQLiteManagement
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public static class DSQLiteStorage
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetCollection<T>(string connectionString, string collectionID, out List<T> objects) where T : IIdentifiable
        {
            objects = new List<T>();
            try
            {
                using var db = new LiteDatabase(connectionString);
                var collection = new DSQLiteCollection<T>(db.GetCollection<T>(collectionID));
                if (collection.Value.Count() == 0) return false;
                objects.AddRange(collection.Value.FindAll());
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"Database loading error! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetCollection<T>(string connectionString, out List<T> objects) where T : IIdentifiable
        {
            objects = new List<T>();
            try
            {
                using var db = new LiteDatabase(connectionString);
                var collection = new DSQLiteCollection<T>(db.GetCollection<T>());
                objects.AddRange(collection.Value.FindAll());
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"Database loading error! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetRecord<T>(string connectionString, string dataID, out T data) where T : class, IIdentifiable
        {
            data = null;
            try
            {
                using var db = new LiteDatabase(connectionString);
                var collection = db.GetCollection<T>();
                data = collection.FindOne(s => s.ID == dataID);
                return data != null;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"Database loading error! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetRecord<T>(string connectionString, string collectionID, string dataID, out T data) where T : class, IIdentifiable
        {
            data = null;
            try
            {
                using var db = new LiteDatabase(connectionString);
                var collection = db.GetCollection<T>(collectionID);
                data = collection.FindOne(s => s.ID == dataID);
                return data != null;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"Database loading error! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySetRecord<T>(string connectionString, T data) where T : IIdentifiable
        {
            try
            {
                using var db = new LiteDatabase(connectionString);
                var collection = db.GetCollection<T>();
                collection.Upsert(data);
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"Database saving error! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySetRecord<T>(string connectionString, string collectionID, T data) where T : IIdentifiable
        {
            try
            {
                using var db = new LiteDatabase(connectionString);
                var collection = db.GetCollection<T>(collectionID);
                collection.Upsert(data);
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"Database saving error! {e.Message}");
                return false;
            }
        }

        #endregion
    }
}