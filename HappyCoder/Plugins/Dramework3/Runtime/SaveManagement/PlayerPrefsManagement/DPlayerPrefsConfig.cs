using IG.HappyCoder.Dramework3.Runtime.Core;
using IG.HappyCoder.Dramework3.Runtime.Experimental.Types;
using IG.HappyCoder.Dramework3.Runtime.SaveManagement.Base;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.SaveManagement.PlayerPrefsManagement
{
    [HideMonoScript]
    public abstract class DPlayerPrefsConfig : DConfig
    {
        #region ================================ FIELDS

        [FoldoutGroup("Settings", 40)] [BoxGroup("Settings/Encryption Type", false)]
        [LabelWidth(ConstantValues.Int_140)]
        [SerializeField]
        private DEnumField<DEncryptionType> _encryptionType;

        [FoldoutGroup("Settings", 40)]
        [LabelWidth(ConstantValues.Int_140)]
        [SerializeField]
        private DSecuredFieldHorizontalButtons<byte> _xorKey;

        [FoldoutGroup("Settings", 40)]
        [LabelWidth(ConstantValues.Int_140)]
        [SerializeField]
        private DSecuredFieldHorizontalButtons<string> _aesKey;

        [FoldoutGroup("Settings", 40)]
        [LabelWidth(ConstantValues.Int_140)]
        [SerializeField]
        private DSecuredFieldHorizontalButtons<string> _aesIV;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        protected DPlayerPrefsConfig()
        {
            _encryptionType = new DEnumField<DEncryptionType>(this, "Encryption:");
            _xorKey = new DSecuredFieldHorizontalButtons<byte>(this, "Xor Key:");
            _aesKey = new DSecuredFieldHorizontalButtons<string>(this, "Aes Key:");
            _aesIV = new DSecuredFieldHorizontalButtons<string>(this, "Aes IV:");
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string AesIV => _aesIV.Value;
        public string AesKey => _aesKey.Value;
        public DEncryptionType EncryptionType => _encryptionType;
        public byte XorKey => _xorKey.Value;

        #endregion

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_aesKey.Value) == false && _aesKey.Value.Length > 32)
                _aesKey.OnSetValue(_aesKey.Value.Remove(32));
            if (string.IsNullOrEmpty(_aesIV.Value) == false && _aesIV.Value.Length > 16)
                _aesIV.OnSetValue(_aesIV.Value.Remove(16));
        }

        [FoldoutGroup("Editor")] [BoxGroup("Editor/Buttons", false)] [HorizontalGroup("Editor/Buttons/Horizontal")]
        [Button(ButtonSizes.Medium)] [PropertyOrder(9998)] [DisableIf("@_aesKey.Locked")]
        private void GenerateAesKey()
        {
            if (_aesKey.Locked) return;
            _aesKey.OnSetValue(Helpers.StringTools.GetRandomString(32));
        }

        [FoldoutGroup("Editor")] [BoxGroup("Editor/Buttons", false)] [HorizontalGroup("Editor/Buttons/Horizontal")]
        [Button(ButtonSizes.Medium)] [PropertyOrder(9999)] [DisableIf("@_aesIV.Locked")]
        private void GenerateAesIV()
        {
            if (_aesIV.Locked) return;
            _aesIV.OnSetValue(Helpers.StringTools.GetRandomString(16));
        }

#endif
    }
}