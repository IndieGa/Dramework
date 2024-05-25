#if UNITY_EDITOR

using System;
using System.Collections;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Tools.Configurators.Editor;

using Newtonsoft.Json;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core
{
    [Serializable] [HideLabel]
    public class FirebaseConfiguratorOptions
    {
        #region ================================ FIELDS

        [FoldoutGroup("@_configuratorType")] [BoxGroup("@_configuratorType/Box", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Configurator Type:")]
        [SerializeField] [ValueDropdown("ConfiguratorTypes")] [JsonProperty]
        private string _configuratorType;

        [FoldoutGroup("@_configuratorType")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Config Names:")]
        [SerializeField] [ShowIf("@string.IsNullOrEmpty(_configuratorType) == false")] [JsonProperty]
        private string[] _configNames;

        [FoldoutGroup("@_configuratorType")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Config Modes:")]
        [SerializeField] [ShowIf("@string.IsNullOrEmpty(_configuratorType) == false")] [JsonProperty]
        private string[] _configModes;

        [FoldoutGroup("@_configuratorType")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Config Versions:")]
        [SerializeField] [ShowIf("@string.IsNullOrEmpty(_configuratorType) == false")] [JsonProperty]
        private string[] _configVersions;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public FirebaseConfiguratorOptions()
        {
        }

        public FirebaseConfiguratorOptions(string configuratorType)
        {
            _configuratorType = configuratorType;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string[] Modes => _configModes;
        public string[] Names => _configNames;
        public string TypeName => _configuratorType;
        public string[] Versions => _configVersions;

        private IEnumerable ConfiguratorTypes
        {
            get
            {
                var baseType = typeof(FirebaseConfigurator<>);
                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
                return (from type in types where type != baseType && type.IsAssignableToGenericType(baseType) select type.Name).ToList();
            }
        }

        #endregion
    }
}

#endif