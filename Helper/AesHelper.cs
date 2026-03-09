using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace TJM103.Helper
{
    public class AesHelper
    {
        // 設定加密編碼
        private static readonly Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// 加密字串
        /// </summary>
        /// <param name="plainText">原始純文字</param>
        /// <param name="key">32 字元的密鑰 (256-bit)</param>
        /// <param name="iv">16 字元的初始向量 (128-bit)</param>
        /// <returns>Base64 編碼後的加密字串</returns>
        public static string Encrypt(string plainText, string key, string iv)
        {
            if (string.IsNullOrEmpty(plainText)) return null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _encoding.GetBytes(key);
                aesAlg.IV = _encoding.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 解密字串
        /// </summary>
        /// <param name="cipherText">Base64 編碼後的加密字串</param>
        /// <param name="key">32 字元的密鑰 (256-bit)</param>
        /// <param name="iv">16 字元的初始向量 (128-bit)</param>
        /// <returns>原始純文字</returns>
        public static string Decrypt(string cipherText, string key, string iv)
        {
            if (string.IsNullOrEmpty(cipherText)) return null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _encoding.GetBytes(key);
                aesAlg.IV = _encoding.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
