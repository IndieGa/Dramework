using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.SaveManagement.Base;


namespace IG.HappyCoder.Dramework3.Runtime.SaveManagement.FileManagement
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class DFileSaveSettings
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DFileSaveSettings(
            string fullPath,
            DSerializationType serializationType,
            DEncryptionType encryptionType,
            string aesKey,
            string aesIv,
            byte xorKey)
        {
            FullPath = fullPath;
            SerializationType = serializationType;
            EncryptionType = encryptionType;
            AesKey = aesKey;
            AesIV = aesIv;
            XorKey = xorKey;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string AesIV { get; }
        public string AesKey { get; }
        public DEncryptionType EncryptionType { get; }
        public string FullPath { get; }
        public DSerializationType SerializationType { get; }
        public byte XorKey { get; }

        #endregion
    }
}