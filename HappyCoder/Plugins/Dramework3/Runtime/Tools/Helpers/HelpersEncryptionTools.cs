using System.Security.Cryptography;
using System.Text;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class EncryptionTools
        {
            #region ================================ METHODS

            public static void Xor(ref byte[] input, byte key)
            {
                var length = input.Length;
                for (var i = 0; i < length; i++)
                    input[i] = (byte)(input[i] ^ key);
            }

            public static void Xor(ref string input, byte key)
            {
                var length = input.Length;
                var sb = new StringBuilder(length);
                for (var i = 0; i < length; i++)
                {
                    var c = (char)(input[i] ^ key);
                    sb.Append(c);
                }
                input = sb.ToString();
            }

            #endregion

            #region ================================ NESTED TYPES

            public static class Aes
            {
                #region ================================ METHODS

                public static byte[] Decrypt(byte[] bytes, string key, string iv)
                {
                    var decryptor = GetCryptoProvider(key, iv).CreateDecryptor();
                    return decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                }

                public static byte[] Encrypt(byte[] bytes, string key, string iv)
                {
                    var cryptoProvider = GetCryptoProvider(key, iv);
                    var encryptor = cryptoProvider.CreateEncryptor(cryptoProvider.Key, cryptoProvider.IV);
                    return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                }

                private static AesCryptoServiceProvider GetCryptoProvider(string key, string iv)
                {
                    var cryptoProvider = new AesCryptoServiceProvider();
                    cryptoProvider.BlockSize = 128;
                    cryptoProvider.KeySize = 256;
                    cryptoProvider.Key = Encoding.UTF8.GetBytes(key);
                    cryptoProvider.IV = Encoding.UTF8.GetBytes(iv);
                    cryptoProvider.Mode = CipherMode.CBC;
                    cryptoProvider.Padding = PaddingMode.PKCS7;
                    return cryptoProvider;
                }

                #endregion
            }

            public static class MD5
            {
                #region ================================ METHODS

                public static byte[] Decrypt(byte[] bytes, string key)
                {
                    var md5 = new MD5CryptoServiceProvider();
                    var cryptoProvider = new TripleDESCryptoServiceProvider();
                    cryptoProvider.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                    cryptoProvider.Mode = CipherMode.ECB;
                    var decryptor = cryptoProvider.CreateDecryptor();
                    return decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                }

                public static byte[] Encrypt(byte[] bytes, string key)
                {
                    var md5 = new MD5CryptoServiceProvider();
                    var cryptoProvider = new TripleDESCryptoServiceProvider();
                    cryptoProvider.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                    cryptoProvider.Mode = CipherMode.ECB;
                    var encryptor = cryptoProvider.CreateEncryptor();
                    return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}