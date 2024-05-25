#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Container.Core.Bootstraps;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;

using UnityEngine;
using UnityEngine.SceneManagement;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static partial class Helpers
    {
        #region ================================ PROPERTIES AND INDEXERS

        public static NamedBuildTarget ActiveBuildTarget
        {
            get
            {
                var buildTarget = EditorUserBuildSettings.activeBuildTarget;
                var targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                return NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            }
        }

        public static bool IsDomainReloadDisabled => EditorSettings.enterPlayModeOptionsEnabled &&
                                                     EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload);

        #endregion

        #region ================================ NESTED TYPES

        public static partial class AddressablesTools
        {
            #region ================================ PROPERTIES AND INDEXERS

            public static IEnumerable<string> Groups => AddressableAssetSettingsDefaultObject.Settings.groups.Select(g => g.Name);
            private static AddressableAssetSettings Settings
            {
                get
                {
                    if (AddressableAssetSettingsDefaultObject.Settings != null)
                        return AddressableAssetSettingsDefaultObject.Settings;

                    AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
                        AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);
                    return AddressableAssetSettingsDefaultObject.Settings;
                }
            }

            #endregion

            #region ================================ METHODS

            public static AddressableAssetEntry CreateAssetEntry<T>(T source, string groupName, string label) where T : Object
            {
                var entry = CreateAssetEntry(source, groupName);
                if (source == null) return entry;
                source.AddAddressableAssetLabel(label);
                return entry;
            }

            public static AddressableAssetEntry CreateAssetEntry<T>(T source, string groupName) where T : Object
            {
                if (source == null || string.IsNullOrEmpty(groupName) || AssetDatabase.Contains(source) == false) return null;

                var sourcePath = AssetDatabase.GetAssetPath(source);
                var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
                var group = GroupExists(groupName) ? GetGroup(groupName) : CreateGroup(groupName);

                var entry = Settings.CreateOrMoveEntry(sourceGuid, group);
                entry.address = sourcePath;
                entry.SetAddress(Path.GetFileNameWithoutExtension(entry.address), false);

                Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

                return entry;
            }

            public static AddressableAssetEntry CreateAssetEntry<T>(T source) where T : Object
            {
                if (source == null || AssetDatabase.Contains(source) == false) return null;

                var sourcePath = AssetDatabase.GetAssetPath(source);
                var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
                var entry = Settings.CreateOrMoveEntry(sourceGuid, Settings.DefaultGroup);
                entry.address = sourcePath;
                entry.SetAddress(Path.GetFileNameWithoutExtension(entry.address), false);

                Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

                return entry;
            }

            public static AddressableAssetGroup CreateGroup(string groupName)
            {
                if (string.IsNullOrEmpty(groupName)) return null;

                var group = Settings.CreateGroup(groupName, false, false, false, Settings.DefaultGroup.Schemas);

                Settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupAdded, group, true);

                return group;
            }

            public static IEnumerable<string> GetAllKeys()
            {
                return Settings.groups.SelectMany(g => g.entries.Select(e => e.ToString()));
            }

            public static AddressableAssetEntry GetAssetEntry(Object source)
            {
                if (source == null || AssetDatabase.Contains(source) == false) return null;
                var sourcePath = AssetDatabase.GetAssetPath(source);
                var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
                return Settings.FindAssetEntry(sourceGuid);
            }

            public static AddressableAssetGroup GetGroup(string groupName)
            {
                return Settings.FindGroup(groupName);
            }

            public static AddressableAssetGroup GetGroup<T>(T source) where T : Object
            {
                foreach (var group in Settings.groups)
                {
                    var sourcePath = AssetDatabase.GetAssetPath(source);
                    var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
                    var entry = group.GetAssetEntry(sourceGuid);
                    if (entry != null)
                        return group;
                }
                return null;
            }

            public static IEnumerable<string> GetKeys(string groupName)
            {
                var keys = new List<string> { "None" };

                var group = Settings.groups.FirstOrDefault(g => g.Name == groupName);
                if (group == null)
                {
                    ConsoleLogger.LogError($"Addressables group {groupName} is not found");
                    return keys;
                }

                keys = group.entries.Select(e => e.ToString()).ToList();
                return keys;
            }

            public static bool GroupExists(string groupName)
            {
                return Settings.FindGroup(groupName) != null;
            }

            public static bool IsAssetAddressable(Object obj)
            {
                var entry = Settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
                return entry != null;
            }

            public static bool RemoveAssetEntry<T>(T source) where T : Object
            {
                if (source == null || AssetDatabase.Contains(source) == false) return false;
                var path = AssetDatabase.GetAssetPath(source);
                var guid = AssetDatabase.AssetPathToGUID(path);
                var result = Settings.RemoveAssetEntry(guid);
                return result;
            }

            public static void SimplifyAddresses()
            {
                foreach (var group in Settings.groups)
                {
                    foreach (var entry in group.entries)
                    {
                        entry.SetAddress(Path.GetFileNameWithoutExtension(entry.address), false);
                        group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, false, true);
                    }
                }

                Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, Settings.groups, true);
            }

            #endregion
        }

        public static class EditorTools
        {
            #region ================================ PROPERTIES AND INDEXERS

            public static IEnumerable<Type> AppTypes => DBootstrap.AppTypes;
            public static string ProjectPath => Application.dataPath.Replace("/Assets", "");

            #endregion

            #region ================================ METHODS

            public static void CopyFilesRecursively(string sourcePath, string targetPath)
            {
                //Now Create all of the directories
                foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                }
            }

            public static string FindAsset<T>(string assetName = "", string[] defaultPath = null, bool skipLog = false) where T : Object
            {
                var filter = string.IsNullOrEmpty(assetName) ? $"t:{typeof(T).Name}" : $"t:{typeof(T).Name} {assetName}";
                defaultPath ??= new[] { "Assets" };
                var guids = AssetDatabase.FindAssets(filter, defaultPath);
                if (guids.Length == 0)
                {
                    if (skipLog == false)
                        Debug.LogError(string.IsNullOrEmpty(assetName) ? $"No one asset type of \"{typeof(T).Name}\" was found" : $"No one asset type of \"{typeof(T).Name}\" and by name \"{assetName}\" was found");
                    return default;
                }

                if (guids.Length > 1)
                {
                    if (skipLog == false)
                        Debug.LogError(string.IsNullOrEmpty(assetName) ? $"More than one asset type of \"{typeof(T).Name}\" was found" : $"More than one asset \"{assetName}\" type of \"{typeof(T).Name}\" was found");
                    return default;
                }

                return guids[0];
            }

            public static string FindAsset(string assetName, bool skipLog = false)
            {
                var guids = AssetDatabase.FindAssets(assetName);
                if (guids.Length == 0)
                {
                    if (skipLog == false)
                        Debug.LogError($"No one asset by name \"{assetName}\" was found");
                    return default;
                }

                if (guids.Length > 1)
                {
                    if (skipLog == false)
                        Debug.LogError($"More than one asset by name \"{assetName}\" was found");
                    return default;
                }

                return guids[0];
            }

            public static IEnumerable<string> FindAssetGuids(string assetName)
            {
                var guids = AssetDatabase.FindAssets(assetName);
                if (guids.Length == 0)
                {
                    Debug.LogError($"No one asset by name \"{assetName}\" was found");
                    return default;
                }

                return guids;
            }

            public static IReadOnlyList<string> FindAssetGuids<T>(string assetName = "", string[] defaultPaths = null) where T : Object
            {
                var filter = string.IsNullOrEmpty(assetName) ? $"t:{typeof(T).Name}" : $"t:{typeof(T).Name} {assetName}";
                defaultPaths ??= new[] { "Assets" };
                var guids = AssetDatabase.FindAssets(filter, defaultPaths);
                if (guids.Length > 0) return guids;
                // Debug.LogWarning(string.IsNullOrEmpty(assetName) ? $"No one asset type of \"{typeof(T).Name}\" was found" : $"No one asset type of \"{typeof(T).Name}\" and by name \"{assetName}\" was found");
                return Array.Empty<string>();
            }

            public static Object FindAttributeOwnerInProject(object attribute, FindMode mode)
            {
                switch (mode)
                {
                    case FindMode.Everywhere:
                        var obj = FindAttributeOwnerInPrefabs(attribute);
                        if (obj == null)
                            obj = FindAttributeOwnerInScriptableObjects(attribute);
                        return obj;
                    case FindMode.Prefab:
                        return FindAttributeOwnerInPrefabs(attribute);
                    case FindMode.ScriptableObject:
                        return FindAttributeOwnerInScriptableObjects(attribute);
                    default:
                        return null;
                }
            }

            public static Object FindFieldOwnerInProject(object field, FindMode mode)
            {
                switch (mode)
                {
                    case FindMode.Everywhere:
                        var obj = FindFieldOwnerInPrefabs(field);
                        if (obj == null)
                            obj = FindFieldOwnerInScriptableObjects(field);
                        return obj;
                    case FindMode.Prefab:
                        return FindFieldOwnerInPrefabs(field);
                    case FindMode.ScriptableObject:
                        return FindFieldOwnerInScriptableObjects(field);
                    default:
                        return null;
                }
            }

            public static MonoBehaviour FindFieldOwnerOnOpenScenes(object field)
            {
                return Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
                    .FirstOrDefault(behaviour => behaviour.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Any(fieldInfo => fieldInfo.GetValue(behaviour) == field));
            }

            public static IEnumerable<Object> FindFieldOwnersInProject(Type fieldType, FindMode mode)
            {
                switch (mode)
                {
                    case FindMode.Everywhere:
                        var objects = FindFieldOwnersInPrefabs(fieldType).ToList();
                        objects.AddRange(FindFieldOwnersInScriptableObjects(fieldType));
                        return objects;
                    case FindMode.Prefab:
                        return FindFieldOwnersInPrefabs(fieldType).ToList();
                    case FindMode.ScriptableObject:
                        return FindFieldOwnersInScriptableObjects(fieldType);
                    default:
                        return null;
                }
            }

            public static IEnumerable<MonoBehaviour> FindFieldOwnersOnOpenScenes(Type fieldType)
            {
                return Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
                    .Where(behaviour => behaviour.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Any(fieldInfo => fieldInfo.FieldType == fieldType));
            }

            public static IEnumerable<T> FindFieldsTypeOfInProject<T>()
            {
                var result = new List<T>();
                var guids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets" });
                foreach (var path in guids.Select(AssetDatabase.GUIDToAssetPath))
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    foreach (var behaviour in prefab.GetComponentsInChildren<MonoBehaviour>(true))
                    {
                        result.AddRange(behaviour.GetType().GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                            .Where(fieldInfo => fieldInfo.FieldType == typeof(T))
                            .Select(fieldInfo => fieldInfo.GetValue(behaviour))
                            .Cast<T>());
                    }
                }

                guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" });
                foreach (var path in guids.Select(AssetDatabase.GUIDToAssetPath))
                {
                    var scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    result.AddRange(scriptableObject.GetType().GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(fieldInfo => fieldInfo.FieldType == typeof(T))
                        .Select(fieldInfo => fieldInfo.GetValue(scriptableObject))
                        .Cast<T>());
                }

                return result;
            }

            public static IEnumerable<T> FindFieldsTypeOfOnOpenScenes<T>()
            {
                var result = new List<T>();
                foreach (var behaviour in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
                {
                    result.AddRange(behaviour.GetType().GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(fieldInfo => fieldInfo.FieldType == typeof(T))
                        .Select(fieldInfo => fieldInfo.GetValue(behaviour))
                        .Cast<T>());
                }
                return result;
            }

            public static IEnumerable<T> GetAllComponentsFromActiveScene<T>(bool includeInactive)
            {
                var result = new List<T>();
                foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
                    result.AddRange(rootGameObject.GetComponentsInChildren<T>(includeInactive));
                return result;
            }

            public static IEnumerable<T> GetAllComponentsFromOpenScenes<T>(bool includeInactive)
            {
                var result = new List<T>();
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    foreach (var rootGameObject in scene.GetRootGameObjects())
                        result.AddRange(rootGameObject.GetComponentsInChildren<T>(includeInactive));
                }
                return result;
            }

            public static IEnumerable<string> GetAsmDefPaths(string rootFolder = "")
            {
                rootFolder = string.IsNullOrEmpty(rootFolder) ? "Assets" : rootFolder;
                var asmdefGuids = AssetDatabase.FindAssets("t:asmdef", new[] { rootFolder });
                return asmdefGuids.Select(AssetDatabase.GUIDToAssetPath);
            }

            public static string GetAssetPath<T>(string assetName = "") where T : Object
            {
                var guid = FindAsset<T>(assetName);
                return AssetDatabase.GUIDToAssetPath(guid);
            }

            public static string GetAssetPath(string assetName)
            {
                var guid = FindAsset(assetName);
                return AssetDatabase.GUIDToAssetPath(guid);
            }

            public static IEnumerable<string> GetAssetPaths<T>(string assetName = "") where T : Object
            {
                var guids = FindAssetGuids<T>(assetName);
                if (guids == null) return Array.Empty<string>();
                return guids.Select(AssetDatabase.GUIDToAssetPath);
            }

            public static IEnumerable<string> GetAssetPaths(string assetName)
            {
                var guids = FindAssetGuids(assetName);
                return guids.Select(AssetDatabase.GUIDToAssetPath);
            }

            public static IEnumerable<string> GetAssetPathsByFileExtension(string fileExtension, string startFolder = "")
            {
                startFolder = IOTools.GetRelativePath(string.IsNullOrEmpty(startFolder) ? Application.dataPath : Path.Combine(Application.dataPath, startFolder));
                return Directory.GetFiles(startFolder, $"*.{fileExtension}", SearchOption.AllDirectories);
            }

            public static FieldInfo GetFieldInfo(Type objType, string fieldName, BindingFlags bindingFlags)
            {
                while (true)
                {
                    var fieldInfo = objType.GetField(fieldName, bindingFlags);
                    if (fieldInfo != null || objType.BaseType == null) return fieldInfo;
                    objType = objType.BaseType;
                }
            }

            public static FieldInfo GetFieldInfo(object obj, string fieldName, BindingFlags bindingFlags)
            {
                return GetFieldInfo(obj.GetType(), fieldName, bindingFlags);
            }

            public static string GetFieldName(object owner, object field)
            {
                var fieldsInfo = owner.GetType().GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (var fieldInfo in fieldsInfo)
                {
                    if (fieldInfo.GetValue(owner) == field)
                        return fieldInfo.Name;
                }
                return string.Empty;
            }

            public static IEnumerable<TField> GetFields<TOwner, TField>(AssetType assetType, string fieldName, BindingFlags bindingFlags) where TOwner : Object
            {
                var result = new List<TField>();
                switch (assetType)
                {
                    case AssetType.Prefab:
                        var guids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets" });
                        foreach (var path in guids.Select(AssetDatabase.GUIDToAssetPath))
                        {
                            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            foreach (var behaviour in prefab.GetComponentsInChildren<TOwner>(true))
                            {
                                var fieldInfo = GetFieldInfo(behaviour.GetType(), fieldName, bindingFlags);
                                if (fieldInfo == null) continue;
                                result.Add((TField)fieldInfo.GetValue(behaviour));
                            }
                        }
                        break;
                    case AssetType.ScriptableObject:
                        foreach (var owner in LoadAssets<TOwner>(string.Empty))
                        {
                            var fieldInfo = GetFieldInfo(owner.GetType(), fieldName, bindingFlags);
                            if (fieldInfo == null) continue;
                            result.Add((TField)fieldInfo.GetValue(owner));
                        }
                        break;
                }

                return result;
            }

            public static IEnumerable<T> GetFieldValues<T>()
            {
                var result = new List<T>();
                var fieldType = typeof(T);
                var owners = new List<Object>();
                owners.AddRange(FindFieldOwnersInProject(fieldType, FindMode.Everywhere));
                owners.AddRange(FindFieldOwnersOnOpenScenes(fieldType));
                foreach (var owner in owners)
                {
                    result.AddRange(from fieldInfo in owner.GetType()
                            .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic)
                        where fieldInfo.FieldType == fieldType || fieldType.IsAssignableFrom(fieldInfo.FieldType)
                        select (T)fieldInfo.GetValue(owner));
                }
                return result;
            }

            public static Scene GetOpenScene(string sceneName)
            {
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    if (scene.name != sceneName) continue;
                    return scene;
                }
                return default;
            }

            public static int GetSceneIndex(string sceneName)
            {
                for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    if (Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path) == sceneName)
                        return i;
                }

                return int.MaxValue;
            }

            public static IEnumerable<string> GetSceneNames()
            {
                return EditorBuildSettings.scenes.Select(s => Path.GetFileNameWithoutExtension(s.path));
            }

            public static IReadOnlyList<T> GetStaticFieldsValues<T>(Type type, bool orderByAlphabet = true)
            {
                return GetStaticFieldsValues<T>(type.Name, orderByAlphabet);
            }

            public static IReadOnlyList<T> GetStaticFieldsValues<T>(string typeName, bool orderByAlphabet = true)
            {
                var result = new List<T>();
                foreach (var type in DBootstrap.AppTypes)
                {
                    if (IsStatic(type) == false || type.Name != typeName) continue;
                    var fieldInfos = type.GetFields(BindingFlags.Static | BindingFlags.Public).Where(i => i.FieldType == typeof(T));
                    result.AddRange(fieldInfos.Select(i => (T)i.GetValue(null)));
                }
                return orderByAlphabet ? result.OrderBy(i => i).ToArray() : result;
            }

            public static IReadOnlyList<TField> GetStaticPublicFieldValues<TField>(Type type, bool orderByAlphabet = true)
            {
                var result = type.GetFields(BindingFlags.Static | BindingFlags.Public).Select(f => (TField)f.GetValue(null)).ToArray();
                return orderByAlphabet ? result.OrderBy(i => i).ToArray() : result;
            }

            public static bool IsCreatedStaticType(string typeName)
            {
                var type = DBootstrap.AppTypes.FirstOrDefault(t => t.Name == typeName);
                return type != null && type.IsStatic();
            }

            public static bool IsStatic(Type type)
            {
                return type.IsAbstract && type.IsSealed;
            }

            public static Object LoadAsset(string assetName)
            {
                var guid = FindAsset(assetName);
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<Object>(path);
            }

            public static T LoadAsset<T>() where T : class
            {
                var guids = AssetDatabase.FindAssets("t:Object", new[] { "Assets" });
                var assets = guids.Select(guid => AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid)));
                foreach (var asset in assets)
                {
                    if (asset is T obj)
                        return obj;
                }
                return null;
            }

            public static T LoadAsset<T>(string assetName, string[] defaultPath = null, bool skipLog = false) where T : Object
            {
                var guid = FindAsset<T>(assetName, defaultPath, skipLog);
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }

            public static IEnumerable<Object> LoadAssets(string assetName)
            {
                var guids = FindAssetGuids(assetName);
                var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
                return paths.Select(AssetDatabase.LoadAssetAtPath<Object>);
            }

            public static IReadOnlyList<T> LoadAssets<T>(string assetName, string[] defaultPaths = null) where T : Object
            {
                var assets = new List<T>();
                var guids = FindAssetGuids<T>(assetName, defaultPaths);
                if (guids.Any() == false) return assets;
                var paths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
                foreach (var path in paths)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset == null)
                    {
                        Debug.LogError($"Asset at path «{path}» is null");
                        continue;
                    }
                    assets.Add(asset);
                }
                return assets;
            }

            public static T LoadPrefab<T>(string prefabName) where T : Component
            {
                var guids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets" });
                var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
                var path = paths.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p) == prefabName);
                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError($"Prefab «{prefabName}» is not found");
                    return null;
                }
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var component = prefab.GetComponent<T>();
                if (component != null) return component;
                Debug.LogError($"Component type of «{typeof(T)}» is not found on prefab «{prefabName}»");
                return null;
            }

            public static IReadOnlyList<T> LoadPrefabs<T>() where T : Component
            {
                var result = new List<T>();
                var guids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets" });
                if (guids.Any() == false) return result;
                var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
                foreach (var path in paths)
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    var component = prefab.GetComponent<T>();
                    if (component != null)
                        result.Add(component);
                }

                return result;
            }

            private static Object FindAttributeOwnerInPrefabs(object attribute)
            {
                return AssetDatabase.FindAssets("t:prefab", new[] { "Assets" })
                    .SelectMany(guid => AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid))
                        .Where(asset => asset is GameObject)
                        .Cast<GameObject>()
                        .Select(gameObject => gameObject.GetComponent<MonoBehaviour>())
                        .Where(behaviour => behaviour != null))
                    .FirstOrDefault(behaviour => behaviour.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .SelectMany(fieldInfo => fieldInfo.GetCustomAttributes(attribute.GetType()))
                        .Any(foundAttribute => Equals(foundAttribute, attribute)));
            }

            private static Object FindAttributeOwnerInScriptableObjects(object attribute)
            {
                return AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" })
                    .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                    .FirstOrDefault(scriptableObject => scriptableObject.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .SelectMany(fieldInfo => fieldInfo.GetCustomAttributes(attribute.GetType()))
                        .Any(foundAttribute => Equals(foundAttribute, attribute)));
            }

            private static Object FindFieldOwnerInPrefabs(object field)
            {
                return AssetDatabase.FindAssets("t:prefab", new[] { "Assets" })
                    .SelectMany(guid => AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid))
                        .Where(asset => asset is GameObject)
                        .Cast<GameObject>()
                        .Select(go => go.GetComponent<MonoBehaviour>())
                        .Where(behaviour => behaviour != null))
                    .FirstOrDefault(behaviour => behaviour.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Any(fieldInfo => fieldInfo.GetValue(behaviour) == field));
            }

            private static Object FindFieldOwnerInScriptableObjects(object field)
            {
                return AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" })
                    .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                    .FirstOrDefault(scriptableObject => scriptableObject.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Any(fieldInfo => fieldInfo.GetValue(scriptableObject) == field));
            }

            private static IEnumerable<Object> FindFieldOwnersInPrefabs(Type fieldType)
            {
                return AssetDatabase.FindAssets("t:prefab", new[] { "Assets" })
                    .SelectMany(guid => AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid))
                        .Where(asset => asset is GameObject)
                        .Cast<GameObject>()
                        .Select(gameObject => gameObject.GetComponent<MonoBehaviour>())
                        .Where(behaviour => behaviour != null))
                    .Where(behaviour => behaviour.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Any(fieldInfo => fieldInfo.FieldType == fieldType));
            }

            private static IEnumerable<Object> FindFieldOwnersInScriptableObjects(Type fieldType)
            {
                return AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" })
                    .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                    .Where(scriptableObject => scriptableObject.GetType()
                        .GetFieldsFromAll(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Any(fieldInfo => fieldInfo.FieldType == fieldType));
            }

            [MenuItem("Tools/Happy Coder/Dramework 3/Helpers/Show Selected Object Type", false, 100)]
            private static void ShowAssetType()
            {
                var selected = Selection.activeObject;
                if (selected == null)
                {
                    Debug.LogError("No one object is selected");
                    return;
                }

                Debug.Log($"Selected asset is type of {selected.GetType()}");
            }

            #endregion
        }

        #endregion
    }
}

#endif