using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace JiaSyuanLibrary.Helper
{
    public class KeyEntity
    {
        public KeyEntity(string key, string iv)
        {
            KeyIV = key;
            KeyValue = iv;
        }

        public string KeyIV { get; }
        public string KeyValue { get; }
    }
    public class EncryptHelper
    {
        #region Encrypt
        /// <summary>
        ///MD5字串加密
        /// </summary>
        /// <param name="original">原字串</param>
        /// <returns>加密後字串</returns>
        public static string GetMD5(string original)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
            return BitConverter.ToString(b).Replace("-", string.Empty);
        }

        /// <summary>
        ///DES字串加密
        /// </summary>
        /// <param name="original">原字串</param>
        /// <param name="desKey">DES金鑰</param>
        /// <returns>加密後字串</returns>
        public static string GetDESEncrypt(string original, string desKey)
        {
            if (string.IsNullOrEmpty(original))
                throw new ArgumentNullException("original");
            if (desKey == null || desKey.Length <= 0)
                throw new ArgumentNullException("Key");
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(desKey, new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(desKey)));
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.Key = rfc2898.GetBytes(desProvider.KeySize / 8);
            desProvider.IV = rfc2898.GetBytes(desProvider.BlockSize / 8);

            string encrypt = "";
            byte[] originalByteArray = Encoding.UTF8.GetBytes(original);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(originalByteArray, 0, originalByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encrypt;
        }

        /// <summary>
        ///AES字串加密
        /// </summary>
        /// <param name="original">原字串</param>
        /// <param name="aesKey">AES金鑰</param>
        /// <returns>加密後字串</returns>
        public static string GetAESEncrypt(string original, string aesKey, string aesIV)
        {
            if (string.IsNullOrEmpty(original))
                throw new ArgumentNullException("original");
            if (string.IsNullOrEmpty(aesKey))
                throw new ArgumentNullException("Key");
            AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider()
            {
                Key = Convert.FromBase64String(aesKey),
                IV = Convert.FromBase64String(aesIV)
            };
            string encrypt = "";
            byte[] originalByteArray = Encoding.UTF8.GetBytes(original);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aesProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(originalByteArray, 0, originalByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encrypt;
        }
        #endregion

        # region Decrypt
        /// <summary>,
        ///DES字串解密
        /// </summary>
        /// <param name="encryptString">解密前字串</param>
        /// <param name="desKey">DES金鑰</param>
        /// <returns>解密後字串</returns>
        public static string GetDESDecrypt(string encryptString, string desKey)
        {
            if (string.IsNullOrEmpty(encryptString))
                throw new ArgumentNullException("encryptString");
            if (string.IsNullOrEmpty(desKey))
                throw new ArgumentNullException("Key");
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(desKey, new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(desKey)));
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.Key = rfc2898.GetBytes(desProvider.KeySize / 8);
            desProvider.IV = rfc2898.GetBytes(desProvider.BlockSize / 8);

            byte[] dataByteArray = Convert.FromBase64String(encryptString);
            string decrypt = "";
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, desProvider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            return decrypt;
        }

        /// <summary>
        ///AES字串解密
        /// </summary>
        /// <param name="encryptString">解密前字串</param>
        /// <param name="aesKey">AES金鑰</param>
        /// <returns>解密後字串</returns>
        public static string GetAESDecrypt(string encryptString, string aesKey, string aesIV)
        {
            if (string.IsNullOrEmpty(encryptString))
                throw new ArgumentNullException("encryptString");
            if (string.IsNullOrEmpty(aesKey))
                throw new ArgumentNullException("Key");
            AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider()
            {
                Key = Convert.FromBase64String(aesKey),
                IV = Convert.FromBase64String(aesIV)
            };
            byte[] dataByteArray = Convert.FromBase64String(encryptString);
            string decrypt = "";
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aesProvider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            return decrypt;
        }
        #endregion


        /// <summary>
        /// 透過AesManaged產生AES的Key與IV，並經過Base64轉換成字串
        /// </summary>
        /// <returns>帶有Key與IV的KeyEntity</returns>
        public static KeyEntity GetKeyEntity()
        {
            AesManaged generator = new AesManaged();
            var key = Convert.ToBase64String(generator.Key);
            var iv = Convert.ToBase64String(generator.IV);
            var result = new KeyEntity(key, iv);
            return result;
        }
    }
}
