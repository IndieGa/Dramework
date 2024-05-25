using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using IG.HappyCoder.Dramework3.Runtime.SaveManagement.Base;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Newtonsoft.Json;


namespace IG.HappyCoder.Dramework3.Runtime.SaveManagement.FileManagement
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class DFile
    {
        #region ================================ FIELDS

        private static readonly BinaryFormatter _formatter = new BinaryFormatter();

        #endregion

        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Load<T>(DFileSaveSettings saveSettings, out T data)
        {
#if UNITY_EDITOR
            return EditorLoad(saveSettings, out data);
#else
            data = default;
            return saveSettings.SerializationType switch
            {
                DSerializationType.Json => LoadJson(saveSettings, out data),
                DSerializationType.Binary => LoadBinary(saveSettings, out data),
                _ => false
            };
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Save<T>(DFileSaveSettings saveSettings, T data)
        {
#if UNITY_EDITOR
            return EditorSave(saveSettings, data);
#else
            return saveSettings.SerializationType switch
            {
                DSerializationType.Binary => SaveBinary(saveSettings, data),
                DSerializationType.Json => SaveJson(saveSettings, data),
                _ => false
            };
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LoadBinary<T>(DFileSaveSettings saveSettings, out T data)
        {
            data = default;
            try
            {
                var bytes = File.ReadAllBytes(saveSettings.FullPath);

                switch (saveSettings.EncryptionType)
                {
                    case DEncryptionType.Xor:
                        Helpers.EncryptionTools.Xor(ref bytes, saveSettings.XorKey);
                        break;
                    case DEncryptionType.Aes:
                        bytes = Helpers.EncryptionTools.Aes.Decrypt(bytes, saveSettings.AesKey, saveSettings.AesIV);
                        break;
                }

                using var ms = new MemoryStream(bytes);
                data = (T)_formatter.Deserialize(ms);
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"File load error at path «{saveSettings.FullPath}»! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LoadJson<T>(DFileSaveSettings saveSettings, out T data)
        {
            data = default;

            try
            {
                var bytes = File.ReadAllBytes(saveSettings.FullPath);
                switch (saveSettings.EncryptionType)
                {
                    case DEncryptionType.None:
                        data = JsonConvert.DeserializeObject<T>(File.ReadAllText(saveSettings.FullPath));
                        break;
                    case DEncryptionType.Xor:
                        Helpers.EncryptionTools.Xor(ref bytes, saveSettings.XorKey);
                        data = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
                        break;
                    case DEncryptionType.Aes:
                        bytes = Helpers.EncryptionTools.Aes.Decrypt(bytes, saveSettings.AesKey, saveSettings.AesIV);
                        data = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"File load error at path «{saveSettings.FullPath}»! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SaveBinary<T>(DFileSaveSettings saveSettings, T data)
        {
            try
            {
                using var ms = new MemoryStream();
                _formatter.Serialize(ms, data);
                var bytes = ms.ToArray();

                switch (saveSettings.EncryptionType)
                {
                    case DEncryptionType.Xor:
                        Helpers.EncryptionTools.Xor(ref bytes, saveSettings.XorKey);
                        break;
                    case DEncryptionType.Aes:
                        bytes = Helpers.EncryptionTools.Aes.Encrypt(ms.ToArray(), saveSettings.AesKey, saveSettings.AesIV);
                        break;
                }

                File.WriteAllBytes(saveSettings.FullPath, bytes);
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"File save error at path «{saveSettings.FullPath}»! {e.Message}");
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SaveJson<T>(DFileSaveSettings saveSettings, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var bytes = Encoding.UTF8.GetBytes(json);
                switch (saveSettings.EncryptionType)
                {
                    case DEncryptionType.None:
                        File.WriteAllText(saveSettings.FullPath, json);
                        break;
                    case DEncryptionType.Xor:
                        Helpers.EncryptionTools.Xor(ref bytes, saveSettings.XorKey);
                        File.WriteAllBytes(saveSettings.FullPath, bytes);
                        break;
                    case DEncryptionType.Aes:
                        bytes = Helpers.EncryptionTools.Aes.Encrypt(bytes, saveSettings.AesKey, saveSettings.AesIV);
                        File.WriteAllBytes(saveSettings.FullPath, bytes);
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"File save error at path «{saveSettings.FullPath}»! {e.Message}");
                return false;
            }
        }

        #endregion

#if UNITY_EDITOR

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EditorLoad<T>(DFileSaveSettings saveSettings, out T data)
        {
            try
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(saveSettings.FullPath));
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"File load error at path «{saveSettings.FullPath}»! {e.Message}");
                data = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EditorSave<T>(DFileSaveSettings saveSettings, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(saveSettings.FullPath, json);
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"File save error at path «{saveSettings.FullPath}»! {e.Message}");
                return false;
            }
        }

#endif
    }
}