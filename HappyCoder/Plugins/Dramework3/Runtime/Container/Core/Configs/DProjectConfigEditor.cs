#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using IG.HappyCoder.Dramework3.Runtime.Container.Attributes.Creation;
using IG.HappyCoder.Dramework3.Runtime.RemoteFiles;
using IG.HappyCoder.Dramework3.Runtime.ResourceManagement;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Tools.Configurators.Editor;

using Newtonsoft.Json;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    internal sealed partial class DProjectConfig
    {
        #region ================================ FIELDS

        private const string GAME_RESOURCES_PATH = "Assets/Plugins/Dramework 3/Runtime/Generated/Game Resources";
        private const string REMOTE_FILES_PATH = "Assets/Plugins/Dramework 3/Runtime/Generated/Remote Files";
        private const string CORE_NAMESPACE = "namespace IG.HappyCoder.Dramework3.Generated";
        private const string GAME_RESOURCES = "GR";
        private const string REMOTE_FILES = "RemoteFiles";
        private const string STREAMING_ASSETS_FOLDER = "Assets\\StreamingAssets";

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [BoxGroup("Settings/Box/Scenes Root Folder", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Project Root Folder:")]
        [SerializeField] [FolderPath] [PropertyOrder(-1)]
        private string _projectRootFolder = "Assets";

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Game Resources")]
        [VerticalGroup("Settings/Box/Game Resources/Vertical Resources")] [BoxGroup("Settings/Box/Game Resources/Vertical Resources/Resource Filter", false)]
        [HorizontalGroup("Settings/Box/Game Resources/Vertical Resources/Resource Filter/Horizontal")]
        [LabelWidth(126)] [LabelText("Search In Resources:")]
        [OnValueChanged("OnResourceFilterChange")]
        [SerializeField]
        private string _resourceFilter = "*";

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Game Resources")]
        [VerticalGroup("Settings/Box/Game Resources/Vertical Resources")] [BoxGroup("Settings/Box/Game Resources/Vertical Resources/Filtered Found Resources", false)]
        [LabelText("Found:")]
        [SerializeField] [ShowIf("@_filteredFoundResources.Count > 0")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)] [ReadOnly]
        // ReSharper disable once NotAccessedField.Local
        private List<DResource> _filteredFoundResources = new List<DResource>();

        [FoldoutGroup("Firebase Settings")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Firebase Configurators:")]
        [SerializeField] [ShowIf("FirebaseEnabled")]
        private FirebaseConfiguratorOptions[] _firebaseConfigurators = Array.Empty<FirebaseConfiguratorOptions>();

        [SerializeField] [HideInInspector]
        private GoogleServicesConfig _googleServicesConfig;

        private IEnumerable<Object> _assets;

#pragma warning disable CS0414
        private bool _isGoogleConfigFound;
#pragma warning restore CS0414

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static FirebaseConfiguratorOptions[] FirebaseConfiguratorOptions => FirebaseEnabled ? Instance._firebaseConfigurators : Array.Empty<FirebaseConfiguratorOptions>();
        private static IEnumerable<string> ConfiguratorTypes
        {
            get
            {
                var baseType = typeof(FirebaseConfigurator<>);
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
                return (from type in types where type != baseType && type.IsAssignableToGenericType(baseType) select type.Name).ToList();
            }
        }
        internal string ProjectRootFolder => _projectRootFolder;
        internal IReadOnlyList<string> Scenes => _scenes;
        private IEnumerable ScenePaths
        {
            get
            {
                var scenePaths = new ValueDropdownList<string>();
                foreach (var sceneAsset in Helpers.EditorTools.LoadAssets<SceneAsset>(string.Empty, new[] { _projectRootFolder }))
                {
                    var path = AssetDatabase.GetAssetPath(sceneAsset);
                    scenePaths.Add(path, path);
                }

                return scenePaths;
            }
        }
        private bool ShowSignUp => _useFirebase && (_auth == null || _auth.IsValid == false);

        #endregion

        #region ================================ METHODS

        internal static void ClearInstallers()
        {
            Instance._installers = Array.Empty<DInstallerConfig>();
        }

        internal static void InitializeProjectSettings()
        {
            Instance.InitializeAssets();
            Instance.InitializeFirebaseConfigurators();
            Instance.InitializeGameResources();
            Instance.InitializeRemoteFiles();
            Instance.InitializeInstallers();
            Instance.GenerateClasses();
            Instance.SetDefaultPlayerLoop();
        }

        private static void CreateDirectory(string path)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }

        private static void DeleteDirectories()
        {
            if (Directory.Exists(GAME_RESOURCES_PATH))
                AssetDatabase.DeleteAsset(Helpers.IOTools.GetRelativePath(GAME_RESOURCES_PATH));

            if (Directory.Exists(REMOTE_FILES_PATH))
                AssetDatabase.DeleteAsset(Helpers.IOTools.GetRelativePath(REMOTE_FILES_PATH));
        }

        private static string GetStringFile(string nameSpace, string className, IEnumerable<string> fields)
        {
            var file = nameSpace;
            file += "\n{";
            file += $"\n\tpublic static class {className}";
            file += "\n\t{";
            file = fields.Aggregate(file, (current, field) => current + $"\n\t\tpublic const string {Helpers.StringTools.ClearText(field)} = \"{field}\";");
            file += "\n\t}";
            file += "\n}";
            return file;
        }

        [MenuItem("Tools/Happy Coder/Dramework 3/Project/Settings %&INS")]
        private static void Show()
        {
            Selection.activeObject = Instance;
        }

        [ContextMenu("Reset Firebase Settings")]
        private void ContextMenuResetFirebaseSettings()
        {
            _auth = null;
            _googleServicesConfig = null;
            _firebaseAppKey = null;
            _firebaseUrl = null;
            _googleServicesConfig = null;
            _isProcessing = false;
            _isGoogleConfigFound = false;
        }

        private void GenerateClasses()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            DeleteDirectories();
            StaticInitializeGameResources();

            if (_gameResources.Count > 0)
            {
                CreateDirectory(GAME_RESOURCES_PATH);
                GenerateGameResourcesFile();
            }

            if (_remoteFileCatalog.Length > 0)
            {
                CreateDirectory(REMOTE_FILES_PATH);
                GenerateRemoteFilesFile();
            }
        }

        private void GenerateGameResourcesFile()
        {
            var scenesClassFile = "";
            foreach (var resource in _gameResources)
            {
                var groupsFile = "";
                foreach (var group in resource.Value)
                {
                    var members = $"\n\t\t\t\tpublic const string ID = \"{group.Key}\";";
                    var keys = group.Value.Select(r => r.Key);
                    members = keys.Aggregate(members, (current, field) => current + $"\n\t\t\t\tpublic const string {Helpers.StringTools.ClearText(field)} = \"{group.Key}~{field}\";");

                    var groupFile = ClassTemplates.EmptySubClass
                        .Replace("#TAB#", "\t\t\t")
                        .Replace("#CLASSNAME#", group.Key)
                        .Replace("#MEMBERS#", members);

                    groupsFile += $"{groupFile}";
                }

                var sceneClassFile = ClassTemplates.EmptySubClass
                    .Replace("#TAB#", "\t\t")
                    .Replace("#CLASSNAME#", resource.Key)
                    .Replace("#MEMBERS#", groupsFile);

                scenesClassFile += $"{sceneClassFile}\n\n";
            }

            var classFile = ClassTemplates.EmptyClass
                .Replace("#USINGS#", "")
                .Replace("#NAMESPACE#", "namespace IG.HappyCoder.Dramework3.Runtime.Generated.GameResources\n")
                .Replace("#CLASSNAME#", GAME_RESOURCES)
                .Replace("#MEMBERS#", scenesClassFile);

            var path = Path.Combine(GAME_RESOURCES_PATH, $"{GAME_RESOURCES}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == classFile) return;
            File.WriteAllText(path, classFile);
        }

        private void GenerateGroupKeysFile()
        {
            foreach (var resource in _gameResources)
            {
                var keys = resource.Value.Select(r => r.Key);
                var sceneName = Helpers.StringTools.ClearText(resource.Key);
                var className = $"{sceneName}{GAME_RESOURCES}";
                var file = GetStringFile(CORE_NAMESPACE, className, keys);
                var path = Path.Combine(GAME_RESOURCES_PATH, $"{className}.cs");
                File.WriteAllText(path, file);
                AssetDatabase.Refresh();
            }
        }

        private void GenerateRemoteFilesFile()
        {
            var sceneClasses = "";
            foreach (var sceneData in _remoteFileCatalog)
            {
                var members = "";
                foreach (var url in sceneData.Urls)
                {
                    var field = Path.GetFileNameWithoutExtension(url).Split('@')[2];
                    var filename = Path.GetFileName(url);
                    members += $"\n\t\t\t\tpublic const string {Helpers.StringTools.ClearText(field)} = \"{filename}\";";
                }

                var sceneClass = ClassTemplates.EmptySubClass
                    .Replace("#TAB#", "\t\t\t")
                    .Replace("#CLASSNAME#", sceneData.SceneID)
                    .Replace("#MEMBERS#", members);

                sceneClasses += $"{sceneClass}{(sceneData == _remoteFileCatalog.Last() ? string.Empty : "\n")}";
            }

            var classFile = ClassTemplates.EmptyClass
                .Replace("#USINGS#", "")
                .Replace("#NAMESPACE#", "namespace IG.HappyCoder.Dramework3.Runtime.Generated.RemoteFiles\n")
                .Replace("#CLASSNAME#", REMOTE_FILES)
                .Replace("#MEMBERS#", sceneClasses);

            var path = Path.Combine(REMOTE_FILES_PATH, $"{REMOTE_FILES}.cs");
            if (File.Exists(path) && File.ReadAllText(path) == classFile) return;
            File.WriteAllText(path, classFile);
        }

        private void GenerateResourceKeysFile()
        {
            foreach (var sceneData in _gameResources)
            {
                var sceneName = Helpers.StringTools.ClearText(sceneData.Key);
                foreach (var groupData in sceneData.Value)
                {
                    var groupName = Helpers.StringTools.ClearText(groupData.Key);
                    var className = $"{sceneName}{groupName}";
                    var file = GetStringFile(CORE_NAMESPACE, className, groupData.Value.Select(r => r.Key));
                    var path = Path.Combine(GAME_RESOURCES_PATH, $"{className}.cs");
                    File.WriteAllText(path, file);
                    AssetDatabase.Refresh();
                }
            }
        }

        private void InitializeAssets()
        {
            var assets = new List<Object>();
            var guids = AssetDatabase.FindAssets("t:Object", new[] { "Assets" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (asset == null) continue;
                assets.Add(asset);
            }
            _assets = assets;
        }

        private void InitializeFirebaseConfigurators()
        {
            var list = _firebaseConfigurators.ToList();
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (ConfiguratorTypes.Contains(list[i].TypeName)) continue;
                list.RemoveAt(i);
            }
            _firebaseConfigurators = list.ToArray();

            list = _firebaseConfigurators.ToList();
            foreach (var typeName in ConfiguratorTypes)
            {
                if (list.Any(c => c.TypeName == typeName)) continue;
                list.Add(new FirebaseConfiguratorOptions(typeName));
            }
            _firebaseConfigurators = list.ToArray();
        }

        private void InitializeGameResources()
        {
            _resourceFilter = "*";
            var resourcesFromAddressables = RemoveResourcesFromAddressables();
            _foundResources = new List<DResource>();

            foreach (var asset in _assets)
            {
                if (asset.name.StartsWith("#") == false || (asset.name.CharCount('#') != 3 && asset.name.CharCount('#') != 4)) continue;
                var ids = asset.name.Split('#');

                if (string.IsNullOrEmpty(ids[1]))
                {
                    Debug.LogError($"[{asset.name}]. Scene ID is null or empty. Fix it!", asset);
                    continue;
                }

                if (string.IsNullOrEmpty(ids[2]))
                {
                    Debug.LogError($"[{asset.name}]. Group ID is null or empty. Fix it!", asset);
                    continue;
                }

                if (string.IsNullOrEmpty(ids[3]))
                {
                    Debug.LogError($"[{asset.name}]. ID is null or empty. Fix it!", asset);
                    continue;
                }

                if (asset != null && Helpers.AddressablesTools.IsAssetAddressable(asset) == false)
                {
                    if (resourcesFromAddressables.TryGetValue(asset, out var groupName))
                        Helpers.AddressablesTools.CreateAssetEntry(asset, groupName);
                    else
                        Helpers.AddressablesTools.CreateAssetEntry(asset);
                }

                _foundResources.Add(new DResource(ids[1], ids[2], ids[3], ids.Length == 5, asset));
            }

            OnResourceFilterChange();
        }

        private void InitializeGoogleConfig()
        {
            var googleConfigAsset = Helpers.EditorTools.LoadAsset<TextAsset>("google-services", skipLog: true);
            if (googleConfigAsset == null)
            {
                _isGoogleConfigFound = false;
                _googleServicesConfig = null;
                _firebaseAppKey = null;
                _firebaseUrl = null;
            }
            else
            {
                _googleServicesConfig = JsonConvert.DeserializeObject<GoogleServicesConfig>(googleConfigAsset.text);
                _firebaseAppKey = _googleServicesConfig.client[0].api_key[0].current_key;
                _firebaseUrl = _googleServicesConfig.project_info.firebase_url;
                _isGoogleConfigFound = true;
            }

            if (_useFirebase) return;
            _googleServicesConfig = null;
            _firebaseAppKey = null;
            _firebaseUrl = null;
        }

        private void InitializeInstallers()
        {
            var result = new List<DInstallerConfig>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(a => a.GetTypes());
            foreach (var type in types)
            {
                if (typeof(DInstaller).IsAssignableFrom(type) == false) continue;
                var installerAttribute = type.GetCustomAttribute<InstallerAttribute>();
                if (installerAttribute == null) continue;
                result.Add(new DInstallerConfig(installerAttribute.SceneID, installerAttribute.Order, type.AssemblyQualifiedName, installerAttribute.ModuleName));
            }
            _installers = result.OrderBy(s => s.SceneBuildOrder).ThenBy(s => s.Order).ThenBy(s => s.SceneID).ToArray();
        }

        private void InitializeRemoteFiles()
        {
            var remoteFilesCatalog = new List<RemoteFileSceneData>();

            foreach (var asset in _assets)
            {
                var path = AssetDatabase.GetAssetPath(asset);
                if (Path.GetDirectoryName(path) != STREAMING_ASSETS_FOLDER) continue;
                var filename = Path.GetFileName(path);
                if (filename.StartsWith("@") == false || filename.CharCount('@') != 2) continue;

                var parts = filename.Split('@');
                var url = string.IsNullOrEmpty(_remoteFilesUrl)
                    ? filename
                    : Path.Combine(_remoteFilesUrl, parts[1], filename).Replace("\\", "/");

                var sceneData = remoteFilesCatalog.FirstOrDefault(d => d.SceneID == parts[1]);
                if (sceneData == null)
                {
                    sceneData = new RemoteFileSceneData
                    {
                        SceneID = parts[1],
                        Urls = new List<string> { url }
                    };
                    remoteFilesCatalog.Add(sceneData);
                }
                else
                {
                    sceneData.Urls.Add(url);
                }
            }

            _remoteFileCatalog = remoteFilesCatalog.ToArray();
        }

        private void OnResourceFilterChange()
        {
            if (string.IsNullOrEmpty(_resourceFilter))
            {
                _filteredFoundResources = new List<DResource>();
                return;
            }

            if (_resourceFilter == "*")
            {
                _filteredFoundResources = _foundResources;
                return;
            }

            _filteredFoundResources = _foundResources.Where(r => r.Title.ToLower().Contains(_resourceFilter.ToLower())).ToList();
        }

        private Dictionary<Object, string> RemoveResourcesFromAddressables()
        {
            var info = new Dictionary<Object, string>();
            foreach (var resource in _foundResources)
            {
                var group = Helpers.AddressablesTools.GetGroup(resource.EditorAsset);
                if (group == null) continue;
                info.Add(resource.EditorAsset, group.Name);
                Helpers.AddressablesTools.RemoveAssetEntry(resource.EditorAsset);
            }
            return info;
        }

        [FoldoutGroup("Firebase Settings")] [BoxGroup("Firebase Settings/Sign Up", false)]
        [Button] [ShowIf("ShowSignUp")] [DisableIf("_isProcessing")]
        private async void SignUp()
        {
            if (_isProcessing) return;
            TimeoutTimer().Forget();

            using var response = await FirebaseRestApi.Authentication.SignUp(EMAIL, _password);
            if (response.IsResponseSuccess)
            {
                _auth = response.Body;
            }

            _cancellationTokenSource.Cancel();
            _isProcessing = false;
        }

        #endregion
    }
}

#endif