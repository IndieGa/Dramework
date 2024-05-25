using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Container.Dispatcher;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace IG.HappyCoder.Dramework3.Editor
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class ScenesTool
    {
        #region ================================ METHODS

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Close Scene 1 %&1", false, 3)]
        public static void CloseScene1()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != DProjectConfigEditorProxy.Scenes[0]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Close Scene 2 %&2", false, 3)]
        public static void CloseScene2()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 2)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 2");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != DProjectConfigEditorProxy.Scenes[1]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Close Scene 3 %&3", false, 3)]
        public static void CloseScene3()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 3)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 3");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != DProjectConfigEditorProxy.Scenes[2]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Close Scene 4 %&4", false, 3)]
        public static void CloseScene4()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 4)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 4");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != DProjectConfigEditorProxy.Scenes[3]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Close Scene 5 %&5", false, 3)]
        public static void CloseScene5()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 5)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 5");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != DProjectConfigEditorProxy.Scenes[4]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Close Scene 6 %&6", false, 3)]
        public static void CloseScene6()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 6)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 6");
                return;
            }

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.path != DProjectConfigEditorProxy.Scenes[5]) continue;
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 1 as Additive %#1", false, 3)]
        public static void OpenScene1AsAdditive()
        {
            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[0], OpenSceneMode.Additive);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 1 as Single %1", false, 3)]
        public static void OpenScene1AsSingle()
        {
            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[0], OpenSceneMode.Single);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 2 as Additive %#2", false, 3)]
        public static void OpenScene2AsAdditive()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 2)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 2");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[1], OpenSceneMode.Additive);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 2 as Single %2", false, 3)]
        public static void OpenScene2AsSingle()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 2)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 2");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[1], OpenSceneMode.Single);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 3 as Additive %#3", false, 3)]
        public static void OpenScene3AsAdditive()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 3)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 3");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[2], OpenSceneMode.Additive);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 3 as Single %3", false, 3)]
        public static void OpenScene3AsSingle()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 3)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 3");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[2], OpenSceneMode.Single);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 4 as Additive %#4", false, 3)]
        public static void OpenScene4AsAdditive()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 4)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 4");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[3], OpenSceneMode.Additive);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 4 as Single %4", false, 3)]
        public static void OpenScene4AsSingle()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 4)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 4");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[3], OpenSceneMode.Single);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 5 as Additive %#5", false, 3)]
        public static void OpenScene5AsAdditive()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 5)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 5");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[4], OpenSceneMode.Additive);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 5 as Single %5", false, 3)]
        public static void OpenScene5AsSingle()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 5)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 5");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[4], OpenSceneMode.Single);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 6 as Additive %#6", false, 3)]
        public static void OpenScene6AsAdditive()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 6)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 6");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[5], OpenSceneMode.Additive);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Open Scene 6 as Single %6", false, 3)]
        public static void OpenScene6AsSingle()
        {
            if (DProjectConfigEditorProxy.Scenes.Count < 6)
            {
                ConsoleLogger.LogError("Editor Project Settings Config is not found or scenes count < 6");
                return;
            }

            EditorSceneManager.OpenScene(DProjectConfigEditorProxy.Scenes[5], OpenSceneMode.Single);
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Add Open Scenes To Build Settings", false, 2)]
        private static void AddOpenScenesToBuildSettings()
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var path = SceneManager.GetSceneAt(i).path;
                if (scenes.Any(s => s.path == path)) return;
                scenes.Add(new EditorBuildSettingsScene(path, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Add Selected Scene To Build Settings", false, 2)]
        private static void AddSelectedSceneToBuildSettings()
        {
            if (Selection.activeObject == null || Selection.activeObject is SceneAsset == false) return;
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.Any(s => s.path == path)) return;
            scenes.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Cleanup Build Settings Scenes List", false, 1)]
        private static void CleanupBuildSettingsSceneList()
        {
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Where(s => File.Exists(s.path)).ToArray();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void ClearBuildSettingsScenes()
        {
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Where(s => File.Exists(s.path)).ToArray();
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Create New Scene", false, 0)]
        private static void CreateNewScene()
        {
            // Create path
            var path = EditorUtility.SaveFilePanel("Save scene", "Assets", "Scene", "unity");
            if (string.IsNullOrEmpty(path)) return;

            // Create scene, dispatcher and root objects
            var scenePath = Helpers.IOTools.GetRelativePath(path);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = Path.GetFileNameWithoutExtension(scenePath);
            var go = new GameObject("Dispatcher");
            go.AddComponent<DDispatcher>();
            SaveScene(scene, scenePath);
            Debug.Log($"[ProjectTools.CreateNewScene()] - Scene is created and saved at path \"{scenePath}\"");
        }

        [InitializeOnLoadMethod]
        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Refresh #r", false, 140)]
        private static void EditorInitialize()
        {
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
            ClearBuildSettingsScenes();
        }

        private static void OnSceneListChanged()
        {
            ClearBuildSettingsScenes();
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Scenes/Remove Scene From Build Settings", false, 3)]
        private static void RemoveSceneFromBuildSettings()
        {
            if (Selection.activeObject == null || Selection.activeObject is SceneAsset == false) return;
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var scenes = EditorBuildSettings.scenes.ToList();
            if (scenes.All(s => s.path != path)) return;
            scenes.RemoveAll(s => s.path == path);
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static void SaveScene(Scene scene, string path)
        {
            path = Helpers.IOTools.GetRelativePath(path);
            EditorSceneManager.SaveScene(scene, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}