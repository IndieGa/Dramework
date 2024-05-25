#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Core;
using IG.HappyCoder.Dramework3.Runtime.RemoteFiles;
using IG.HappyCoder.Dramework3.Runtime.ResourceManagement;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core;

using Sirenix.OdinInspector;

using UnityEngine;

using DResource = IG.HappyCoder.Dramework3.Runtime.ResourceManagement.DResource;
using Object = UnityEngine.Object;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    internal sealed partial class DProjectConfig : DConfig
    {
        #region ================================ FIELDS

        private const string EMAIL = "project_admin@mail.com";
        private static DProjectConfig _instance;

        private const string FOLDER = "Assets/Plugins/Dramework 3/Resources";
        private const string FILENAME = "Project Config.asset";
        private const string REMOTE_FILES_FOLDER = "Remote Files";

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Scenes:")]
        [SerializeField] [ValueDropdown("ScenePaths")]
        private string[] _scenes;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Modules:")]
        [SerializeField] [ListDrawerSettings(HideAddButton = true)]
        private DInstallerConfig[] _installers;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)]
        [FoldoutGroup("Settings/Box/Remote Files")] [HorizontalGroup("Settings/Box/Remote Files/Horizontal")] [BoxGroup("Settings/Box/Remote Files/Horizontal/Box", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Remote URL:")]
        [SerializeField] [PropertyOrder(0)]
        private string _remoteFilesUrl;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)]
        [FoldoutGroup("Settings/Box/Remote Files")] [PropertyOrder(2)]
        [LabelWidth(ConstantValues.Int_80)] [LabelText("Remote Files Catalog:")]
        [SerializeField] [ReadOnly]
        private RemoteFileSceneData[] _remoteFileCatalog;

        [FoldoutGroup("Firebase Settings")] [HorizontalGroup("Firebase Settings/Horizontal")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Use Firebase:")]
        [SerializeField] [OnValueChanged("InitializeGoogleConfig")] [ShowIf("@_isGoogleConfigFound")]
        private bool _useFirebase;

        [FoldoutGroup("Firebase Settings")] [BoxGroup("Firebase Settings/Sign Up", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Password:")]
        [SerializeField] [ShowIf("ShowSignUp")] [DisableIf("_isProcessing")]
        private string _password;

        [SerializeField] [HideInInspector]
        private string _firebaseAppKey;

        [SerializeField] [HideInInspector]
        private string _firebaseUrl;

        [SerializeField] [HideInInspector]
        private AuthResponseBody _auth;

        [SerializeField] [HideInInspector]
        private List<DResource> _foundResources = new List<DResource>();

        private bool _isProcessing;
        private CancellationTokenSource _cancellationTokenSource;

        private Dictionary<string, Dictionary<string, Dictionary<string, IResource>>> _gameResources;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static string FirebaseAppKey => FirebaseEnabled ? Instance._firebaseAppKey : null;
        public static string FirebaseUrl => FirebaseEnabled ? Instance._firebaseUrl : null;
        public static bool UseFirebase => Instance._useFirebase;
        internal static DProjectConfig Instance
        {
            get
            {
                if (_instance != null) return _instance;
                StaticInitialize();
                return _instance;
            }
        }
        private static bool FirebaseEnabled => Instance._useFirebase
                                               && string.IsNullOrEmpty(Instance._firebaseAppKey) == false
                                               && string.IsNullOrEmpty(Instance._firebaseUrl) == false;

        #endregion

        #region ================================ METHODS

        internal static async UniTask<string> GetIdToken()
        {
            if (FirebaseEnabled == false || Instance._auth == null || Instance._auth.IsValid == false) return null;
            await Instance._auth.Refresh();
            return Instance._auth.IdToken;
        }

        internal static IEnumerable<string> GetInstallerTypes(string sceneID)
        {
            var result = new List<string>();
            foreach (var installer in Instance._installers)
            {
                if (installer.Enabled == false || installer.SceneID != sceneID) continue;
                result.Add(installer.AssemblyQualifiedName);
            }
            return result;
        }

        internal static byte[] GetRemoteFile(string filename)
        {
            return DCore.GetRemoteFile(filename);
        }

        internal static string GetRemoteFilesDirectory(string sceneID)
        {
            return Path.Combine(Application.persistentDataPath, REMOTE_FILES_FOLDER, sceneID).Replace("\\", "/");
        }

        internal static async UniTask<IEnumerable<Object>> LoadSceneResources(string sceneID)
        {
            var assets = new List<Object>();
            if (TryGetSceneResources(sceneID, out var resources) == false) return assets;

            foreach (var resource in resources.Where(r => ((DResource)r).IsLoadAtStart))
            {
                await ((DResource)resource).LoadAtStartAsync();
                assets.Add(resource.GetAsset<Object>());
            }
            return assets;
        }

        internal static async UniTask<Response<EPSignInResponseBody>> LogIn()
        {
            if (Instance._isProcessing) return null;
            Instance.TimeoutTimer().Forget();

            var response = await FirebaseRestApi.Authentication.SignIn(EMAIL, Instance._password);
            if (response.IsResponseSuccess)
            {
                Instance._auth = response.Body;
            }

            Instance._cancellationTokenSource.Cancel();
            Instance._isProcessing = false;
            return response;
        }

        internal static void ReleaseResources(string sceneID)
        {
            if (TryGetSceneResources(sceneID, out var resources) == false) return;
            foreach (var resource in resources)
                resource.Release();
        }

        internal static bool TryGetRemoteFileUrls(string sceneID, out IReadOnlyCollection<string> urls)
        {
            foreach (var sceneData in Instance._remoteFileCatalog)
            {
                if (sceneData.SceneID != sceneID) continue;
                urls = sceneData.Urls;
                return true;
            }

            urls = null;
            return false;
        }

        internal static bool TryGetResource(string sceneID, string groupID, string resourceID, out IResource resource)
        {
            if (Instance._gameResources.TryGetValue(sceneID, out var groups))
            {
                if (groups.TryGetValue(groupID, out var resources))
                    return resources.TryGetValue(resourceID, out resource);

                Instance.LogError($"The resource group «{groupID}» is not found", $"[{nameof(DProjectConfig)}.{nameof(TryGetResource)}]", Instance);
            }

            Instance.LogError($"Scene «{sceneID}» is not found", $"[{nameof(DProjectConfig)}.{nameof(TryGetResource)}]", Instance);
            resource = null;
            return false;
        }

        internal static bool TryGetResourceGroup(string sceneID, string groupID, out Dictionary<string, IResource> resourceGroup)
        {
            var result = new Dictionary<string, IResource>();
            if (Instance._gameResources.TryGetValue(sceneID, out var resourceGroups))
            {
                if (resourceGroups.TryGetValue(groupID, out var group))
                {
                    foreach (var resource in group)
                        result.Add($"{groupID}~{resource.Key}", resource.Value);

                    resourceGroup = result;
                    return true;
                }
            }

            resourceGroup = null;
            return false;
        }

        private static void StaticInitialize()
        {
            _instance = Resources.Load<DProjectConfig>(Path.GetFileNameWithoutExtension(FILENAME));

#if UNITY_EDITOR

            if (_instance == null)
            {
                if (Directory.Exists(FOLDER) == false)
                    Directory.CreateDirectory(FOLDER);

                _instance = CreateInstance<DProjectConfig>();
                AssetDatabase.CreateAsset(_instance, Path.Combine(FOLDER, FILENAME));
                EditorUtility.SetDirty(_instance);

                if (AddressableAssetSettingsDefaultObject.Settings == null)
                    AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
                        AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                _instance.InitializeGoogleConfig();
            }

            if (_instance._isGoogleConfigFound)
                LogIn().Forget();
#endif

            StaticInitializeGameResources();
        }

        private static void StaticInitializeGameResources()
        {
            Instance._gameResources = new Dictionary<string, Dictionary<string, Dictionary<string, IResource>>>();
            foreach (var gameResource in Instance._foundResources)
            {
                if (Instance._gameResources.TryGetValue(gameResource.SceneID, out var groups) == false)
                {
                    Instance._gameResources.Add(gameResource.SceneID, new Dictionary<string, Dictionary<string, IResource>>());
                    Instance._gameResources[gameResource.SceneID].Add(gameResource.GroupID, new Dictionary<string, IResource>());
                }
                else
                {
                    if (groups.ContainsKey(gameResource.GroupID) == false)
                        groups.Add(gameResource.GroupID, new Dictionary<string, IResource>());
                }

                Instance._gameResources[gameResource.SceneID][gameResource.GroupID].TryAdd(gameResource.ID, gameResource);
            }
        }

        private static bool TryGetSceneResources(string sceneID, out IEnumerable<IResource> resources)
        {
            if (Instance._gameResources.TryGetValue(sceneID, out var resourceGroups))
            {
                resources = resourceGroups.Values.SelectMany(g => g.Values);
                return true;
            }

            resources = null;
            return false;
        }

        private async UniTaskVoid TimeoutTimer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _isProcessing = true;
            await UniTask.Delay(10000, cancellationToken: _cancellationTokenSource.Token);
            _isProcessing = false;
        }

        #endregion
    }
}