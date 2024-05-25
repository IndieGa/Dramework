using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using IG.HappyCoder.Dramework3.Runtime.SaveManagement.Base;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Newtonsoft.Json;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.SaveManagement.PlayerPrefsManagement
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class DPlayerPrefsStorage
    {
        #region ================================ METHODS

        public static bool Load<T>(DPlayerPrefsConfig config, out T data)
        {
            data = default;

            try
            {
                var save = PlayerPrefs.GetString(config.name);
                switch (config.EncryptionType)
                {
                    case DEncryptionType.None:
                        data = JsonConvert.DeserializeObject<T>(save);
                        break;
                    case DEncryptionType.Xor:
                        Helpers.EncryptionTools.Xor(ref save, config.XorKey);
                        data = JsonConvert.DeserializeObject<T>(save);
                        break;
                    case DEncryptionType.Aes:
                        var bytes = Convert.FromBase64String(save);
                        bytes = Helpers.EncryptionTools.Aes.Decrypt(bytes, config.AesKey, config.AesIV);
                        data = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
                        break;
                }

                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"PlayerPrefs load error! {e.Message}");
                return false;
            }
        }

        public static bool Save<T>(DPlayerPrefsConfig config, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                switch (config.EncryptionType)
                {
                    case DEncryptionType.None:
                        PlayerPrefs.SetString(config.name, json);
                        break;
                    case DEncryptionType.Xor:
                        Helpers.EncryptionTools.Xor(ref json, config.XorKey);
                        PlayerPrefs.SetString(config.name, json);
                        break;
                    case DEncryptionType.Aes:
                        var bytes = Encoding.UTF8.GetBytes(json);
                        bytes = Helpers.EncryptionTools.Aes.Encrypt(bytes, config.AesKey, config.AesIV);
                        PlayerPrefs.SetString(config.name, Convert.ToBase64String(bytes));
                        break;
                }

                PlayerPrefs.Save();
                return true;
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError($"PlayerPrefs save error! {e.Message}");
                return false;
            }
        }

        #endregion
    }
}