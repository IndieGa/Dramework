#if UNITY_EDITOR

using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Errors;

using Newtonsoft.Json;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Tools.Configurators.Editor
{
    [HideMonoScript]
    public abstract class FirebaseConfigurator<T> : FirebaseConfiguratorBase where T : class, new()
    {
        #region ================================ FIELDS

        [BoxGroup("Config", false)] [BoxGroup("Config/Name", false)]
        [LabelText("Name:")]
        [SerializeField] [ValueDropdown("Names")] [DisableIf("Inactive")]
        private string _name;

        [BoxGroup("Config", false)] [BoxGroup("Config/Mode", false)]
        [LabelText("Mode:")]
        [SerializeField] [ValueDropdown("Modes")] [DisableIf("Inactive")]
        private string _mode;

        [BoxGroup("Config", false)] [BoxGroup("Config/Version", false)]
        [LabelText("Version:")]
        [SerializeField] [ValueDropdown("Versions")] [DisableIf("Inactive")]
        private string _version;

        [BoxGroup("Config", false)] [BoxGroup("Config/Config", false)]
        [SerializeField] [HideLabel] [DisableIf("Inactive")]
        private T _config;

        private bool _processing;
        private CancellationTokenSource _cancellationTokenSource;
        private EPSignInResponseBody _authData;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private string Endpoint => $"{DProjectConfig.FirebaseUrl}/configs/" +
                                   $"{Helpers.Helpers.StringTools.ClearText(typeof(T).Name.Replace(".", "_")).ToLower()}s/" +
                                   $"{Helpers.Helpers.StringTools.ClearText(_name.Replace(".", "_")).ToLower()}/" +
                                   $"{Helpers.Helpers.StringTools.ClearText(_mode.Replace(".", "_")).ToLower()}/" +
                                   $"{_version.Replace(".", "_").Replace(" ", "_").ToLower()}.json";
        private bool Inactive => DProjectConfig.UseFirebase == false || _processing;
        private IEnumerable Modes => DProjectConfig.FirebaseConfiguratorOptions.FirstOrDefault(c => c.TypeName == GetType().Name)?.Modes;
        private IEnumerable Names => DProjectConfig.FirebaseConfiguratorOptions.FirstOrDefault(c => c.TypeName == GetType().Name)?.Names;
        private IEnumerable Versions => DProjectConfig.FirebaseConfiguratorOptions.FirstOrDefault(c => c.TypeName == GetType().Name)?.Versions;

        #endregion

        #region ================================ METHODS

        [BoxGroup("Config", false)] [HorizontalGroup("Config/Buttons")]
        [Button("Backup", ButtonSizes.Medium)] [PropertyOrder(-999)] [DisableIf("Inactive")]
        private async void Backup()
        {
            if (DProjectConfig.UseFirebase == false || _processing) return;

            var filename = $"{Helpers.Helpers.StringTools.ClearText(typeof(T).Name.Replace(".", "_")).ToLower()}s";
            var path = EditorUtility.SaveFilePanel("Backup", "", filename, "json");
            if (string.IsNullOrEmpty(path))
            {
                _processing = false;
                return;
            }

            if (LogEnabled)
                Debug.Log("Backup is started");
            TimeoutTimer().Forget();

            var requestOptions = new RequestOptions
            {
                Endpoint = $"{DProjectConfig.FirebaseUrl}/configs/{filename}.json",
                EnableLog = LogEnabled
            };
            requestOptions.Params.Add("auth", await DProjectConfig.GetIdToken());
            using var response = await FirebaseRestApi.RealtimeDatabase.Get(requestOptions);
            if (response.IsResponseSuccess)
            {
                var obj = JsonConvert.DeserializeObject(response.Text);
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                await File.WriteAllTextAsync(path, json);
                if (LogEnabled)
                    Debug.Log("Backup is completed");
            }
            else
            {
                var error = JsonConvert.DeserializeObject<FirebaseErrorResponseBody>(response.Text);
                if (error.error == "Permission denied")
                {
                    _processing = false;
                    _cancellationTokenSource.Cancel();
                    using var loginResponse = await DProjectConfig.LogIn();
                    if (loginResponse is { IsResponseSuccess: true })
                        Backup();
                }
            }

            _processing = false;
            _cancellationTokenSource.Cancel();
        }

        [BoxGroup("Config", false)] [HorizontalGroup("Config/Buttons")]
        [Button("Read", ButtonSizes.Medium)] [PropertyOrder(-1000)] [DisableIf("Inactive")]
        private async void Read()
        {
            if (DProjectConfig.UseFirebase == false || _processing) return;

            if (string.IsNullOrEmpty(_name))
            {
                Debug.LogError("Config ID is null or empty", this);
                return;
            }

            if (LogEnabled)
                Debug.Log("Loading is started");

            TimeoutTimer().Forget();

            var requestOptions = new RequestOptions
            {
                Endpoint = Endpoint,
                EnableLog = LogEnabled
            };

            requestOptions.Params.Add("auth", await DProjectConfig.GetIdToken());
            using var response = await FirebaseRestApi.RealtimeDatabase.Get<T>(requestOptions);
            if (response.IsResponseSuccess)
            {
                _config = response.Body;
                if (LogEnabled)
                    Debug.Log("Loading is completed");
            }
            else
            {
                var error = JsonConvert.DeserializeObject<FirebaseErrorResponseBody>(response.Text);
                if (error.error == "Permission denied")
                {
                    _processing = false;
                    _cancellationTokenSource.Cancel();
                    using var loginResponse = await DProjectConfig.LogIn();
                    if (loginResponse is { IsResponseSuccess: true })
                        Read();
                }
            }

            _processing = false;
            _cancellationTokenSource.Cancel();
        }

        private async UniTaskVoid TimeoutTimer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _processing = true;
            await UniTask.Delay(10000, cancellationToken: _cancellationTokenSource.Token);
            _processing = false;
        }

        [BoxGroup("Config", false)] [HorizontalGroup("Config/Buttons")]
        [Button("Write", ButtonSizes.Medium)] [PropertyOrder(-1000)] [DisableIf("Inactive")]
        private async void Write()
        {
            if (DProjectConfig.UseFirebase == false || _processing) return;

            if (string.IsNullOrEmpty(_name))
            {
                Debug.LogError("Config ID is null or empty", this);
                return;
            }

            if (LogEnabled)
                Debug.Log("Writing is started");
            TimeoutTimer().Forget();

            var requestOptions = new RequestOptions
            {
                Endpoint = Endpoint,
                Body = _config,
                EnableLog = LogEnabled,
                ForceErrors = false
            };

            requestOptions.Params.Add("auth", await DProjectConfig.GetIdToken());
            using var response = await FirebaseRestApi.RealtimeDatabase.Patch(requestOptions);
            if (response.IsResponseSuccess)
            {
                if (LogEnabled)
                    Debug.Log("Writing is completed");
            }
            else
            {
                var error = JsonConvert.DeserializeObject<FirebaseErrorResponseBody>(response.Text);
                if (error.error == "Permission denied")
                {
                    _processing = false;
                    _cancellationTokenSource.Cancel();
                    using var loginResponse = await DProjectConfig.LogIn();
                    if (loginResponse is { IsResponseSuccess: true })
                        Write();
                }
            }

            _processing = false;
            _cancellationTokenSource.Cancel();
        }

        #endregion
    }
}
#endif