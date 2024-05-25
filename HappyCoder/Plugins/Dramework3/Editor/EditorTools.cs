using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Container.Core.Bootstraps;
using IG.HappyCoder.Dramework3.Runtime.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Validation.Interfaces;

using MemoryPack;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Editor
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class EditorTools
    {
        #region ================================ FIELDS

        private const string STREAMING_ASSETS_FOLDER = "Assets/StreamingAssets";

        #endregion

        #region ================================ EVENTS AND DELEGATES

        public static event Action<Action> onBeforePlay;

        #endregion

        #region ================================ METHODS

        public static T LoadRemoteFile<T>(string filename)
        {
            var filePath = Path.Combine(STREAMING_ASSETS_FOLDER, filename);
            try
            {
                var bytes = File.ReadAllBytes(filePath);
                return MemoryPackSerializer.Deserialize<T>(bytes);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void CheckForNullObjectsFields(IEnumerable<object> objects)
        {
            foreach (var obj in objects)
            {
                var type = obj.GetType();
                var fieldInfos = new List<FieldInfo>();

                while (type != null && type != typeof(MonoBehaviour) && type != typeof(Object))
                {
                    fieldInfos.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic));
                    type = type.BaseType;
                }

                foreach (var fieldInfo in fieldInfos.Where(fi => fi.GetCustomAttribute<SerializeField>() != null))
                {
                    if (fieldInfo.GetValue(obj) == null)
                        Debug.LogError($"Field {fieldInfo.Name} is null ({obj.GetType()})", obj as Object);
                }
            }
        }

        private static void ExecuteValidateMethod(IEnumerable<IValidable> objects)
        {
            foreach (var obj in objects)
                obj.Validate();
        }

        private static IEnumerable<T> GetAssets<T>(string assetName = "", string defaultFolder = "Assets") where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name} {assetName}", new[] { defaultFolder });
            if (guids.Length > 0)
                return guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>);

            Debug.LogWarning($"[EditorTools.GetAssets<T>()] - Assets by name \"{assetName}\" and type of \"{typeof(T).Name}\" in folder \"{defaultFolder}\" are not found");
            return Array.Empty<T>();
        }

        [DidReloadScripts]
        private static void OnRecompile()
        {
            if (DBootstrap.IsProjectStarted == false) return;

            ValidateConfigs();
            ValidateSceneObjects();

            EditorSceneManager.MarkAllScenesDirty();
            EditorApplication.ExecuteMenuItem("File/Save");
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Project/Play %p", false, 180)]
        private static void Play()
        {
            if (onBeforePlay != null)
                onBeforePlay.Invoke(() => EditorApplication.ExecuteMenuItem("Edit/Play"));
            else
                EditorApplication.ExecuteMenuItem("Edit/Play");
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Validation/Validate Configs", false, 260)]
        private static void ValidateConfigs()
        {
            Debug.Log("Validating of configs is started");

            var configs = Helpers.EditorTools.LoadAssets<DConfig>(string.Empty).Cast<IValidable>().ToArray();
            ExecuteValidateMethod(configs);
            CheckForNullObjectsFields(configs);

            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();

            Debug.Log("Validating of configs is completed");
        }

        private static void ValidateImageComponents(IEnumerable<Image> images)
        {
            foreach (var image in images.Where(i => i.sprite == null))
                Debug.LogError($"Sprite in Image is null ({image.name})", image);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Validation/Validate Scene Objects", false, 260)]
        private static void ValidateSceneObjects()
        {
            Debug.Log("Validating of scene objects is started");

            var sceneObjects = Helpers.EditorTools.GetAllComponentsFromOpenScenes<IValidable>(false).ToArray();
            ExecuteValidateMethod(sceneObjects);
            CheckForNullObjectsFields(sceneObjects);

            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();

            Debug.Log("Validating of scene objects is completed");
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Validation/Validate Selected Object", false, 260)]
        private static void ValidateSelectedObject()
        {
            var selected = Selection.activeObject;
            if (selected == null)
            {
                Debug.LogError("No one object is selected");
                return;
            }

            if (selected is GameObject gameObject)
            {
                var dBehaviours = gameObject.GetComponents<IValidable>();
                foreach (var dBehaviour in dBehaviours)
                    dBehaviour.Validate();
            }

            if (selected is IValidable validable)
            {
                validable.Validate();
            }
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Validation/Validate Static Classes", false, 260)]
        private static void ValidateStaticClasses()
        {
            Debug.Log("Validating of static classes is started");

            var staticTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsStatic());


            foreach (var staticType in staticTypes)
            {
                var methodInfo = staticType.GetMethod("OnValidate", BindingFlags.Static | BindingFlags.NonPublic);
                if (methodInfo == null) continue;
                methodInfo.Invoke(null, null);
            }

            AssetDatabase.Refresh();

            Debug.Log("Validating of static classes is completed");
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Validation/Validate Image Components", false, 260)]
        private static void ValidateUnityComponents()
        {
            Debug.Log("Validating Unity components is started");

            ValidateImageComponents(Object.FindObjectsByType<Image>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID));
            ValidateImageComponents(Helpers.EditorTools.LoadPrefabs<Image>());

            Debug.Log("Validating Unity components is completed");
        }

        #endregion

        // [MenuItem("Tools/Localization/PullAllTablesFromGoogle")]
        // private static void PullAllTablesFromGoogle()
        // {
        //     var collections = Resources.FindObjectsOfTypeAll<StringTableCollection>();
        //     foreach (var collection in collections)
        //     {
        //         foreach (var collectionExtension in collection.Extensions)
        //         {
        //             if (collectionExtension is GoogleSheetsExtension googleExtension)
        //             {
        //                 var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider);
        //                 googleSheets.SpreadSheetId = googleExtension.SpreadsheetId;
        //                 googleSheets.PullIntoStringTableCollection(googleExtension.SheetId, collection, googleExtension.Columns);
        //             }
        //         }
        //     }
        // }
    }
}