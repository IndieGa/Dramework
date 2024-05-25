using System;
using System.Collections;
using System.Linq;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Tools.Configurators.Runtime
{
    [Serializable] [HideLabel]
    public class FirebaseConfigLoader<T> where T : class, new()
    {
        #region ================================ FIELDS

        [SerializeField] [HideInInspector]
        private string _endpoint;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public T Data { get; private set; }

        #endregion

        #region ================================ METHODS

        public async UniTask Load(DBehaviour host)
        {
            if (string.IsNullOrEmpty(_endpoint))
            {
                host.LogError("Endpoint is null or empty", "[Firebase Config]", host);
                return;
            }

            host.Log($"Loading of {typeof(T)} config is started", "[Firebase Config]", host);
            var requestOptions = new RequestOptions
            {
                Endpoint = _endpoint,
                EnableLog = true
            };

            requestOptions.Params.Add("auth", await DProjectConfig.GetIdToken());
            using var response = await FirebaseRestApi.RealtimeDatabase.Get<T>(requestOptions);
            if (response.IsResponseSuccess)
            {
                Data = response.Body;
                host.Log($"Loading of {typeof(T)} config is completed", "[Firebase Config]", host);
            }
            else
            {
                host.LogError($"Error of loading of {typeof(T)} config. {response.Text}", "[Firebase Config]", host);
            }
        }

        #endregion


#if UNITY_EDITOR

        #region ================================ FIELDS

        [BoxGroup]
        [LabelText("Configurator:")]
        [SerializeField] [ValueDropdown("Configurators")] [OnValueChanged("OnConfiguratorChange")]
        private string _configurator;

        [BoxGroup]
        [LabelText("Config Name:")]
        [SerializeField] [ValueDropdown("Names")] [OnValueChanged("SetEndpoint")]
        private string _name;

        [BoxGroup]
        [LabelText("Config Mode:")]
        [SerializeField] [ValueDropdown("Modes")] [OnValueChanged("SetEndpoint")]
        private string _mode;

        [BoxGroup]
        [LabelText("Config Version:")]
        [SerializeField] [ValueDropdown("Versions")] [OnValueChanged("SetEndpoint")]
        private string _version;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private IEnumerable Configurators => DProjectConfig.FirebaseConfiguratorOptions.Select(o => o.TypeName);
        private IEnumerable Names => DProjectConfig.FirebaseConfiguratorOptions.FirstOrDefault(c => c.TypeName == _configurator)?.Names;
        private IEnumerable Modes => DProjectConfig.FirebaseConfiguratorOptions.FirstOrDefault(c => c.TypeName == _configurator)?.Modes;
        private IEnumerable Versions => DProjectConfig.FirebaseConfiguratorOptions.FirstOrDefault(c => c.TypeName == _configurator)?.Versions;

        private void OnConfiguratorChange()
        {
            _name = null;
            _mode = null;
            _version = null;
            _endpoint = null;
        }

        private void SetEndpoint()
        {
            if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_mode) || string.IsNullOrEmpty(_version)) return;
            _endpoint = $"{DProjectConfig.FirebaseUrl}/configs/" +
                        $"{Helpers.Helpers.StringTools.ClearText(typeof(T).Name.Replace(".", "_")).ToLower()}s/" +
                        $"{Helpers.Helpers.StringTools.ClearText(_name.Replace(".", "_")).ToLower()}/" +
                        $"{Helpers.Helpers.StringTools.ClearText(_mode.Replace(".", "_")).ToLower()}/" +
                        $"{_version.Replace(".", "_").Replace(" ", "_").ToLower()}.json";
        }

        #endregion

#endif
    }
}