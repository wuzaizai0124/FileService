using System;
using System.Security.Cryptography;
using System.Text;

namespace Risfond.File.Infrastructure
{
    public class EncryptionHelper
    {
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSha1Hash(string input)
        {
            byte[] inputBytes = Encoding.Default.GetBytes(input);

            SHA1 sha = new SHA1CryptoServiceProvider();

            byte[] result = sha.ComputeHash(inputBytes);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// 验证SHA1密文
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifySha1Hash(string input, string hash)
        {
            string hashOfInput = GetSha1Hash(input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;            
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Aes解密（密文、秘钥、和向量均传base64格式）
        /// </summary>
        /// <param name="text">密文</param>
        /// <param name="aseKey">秘钥</param>
        /// <param name="aesIv">向量</param>
        /// <returns></returns>
        public static string AesDecrypt(string text, string aseKey, string aesIv)
        {
            try
            {
                byte[] encryptedData = Convert.FromBase64String(text);  // strToToHexByte(text);
                var rijndaelCipher = new RijndaelManaged
                {
                    Key = Convert.FromBase64String(aseKey),
                    IV = Convert.FromBase64String(aesIv),
                    BlockSize = 128,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.Default.GetString(plainText);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}