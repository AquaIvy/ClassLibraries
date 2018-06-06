using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace DogSE.Library.Util
{
    /// <summary>
    /// 通过DES对称加密算法,完成对字符串的加密和解密操作。 
    /// </summary>
    public class StringCrypto
    {
        /// <summary>
        /// DES对称加密算法
        /// </summary>
        private SymmetricAlgorithm m_DESCrypto = new DESCryptoServiceProvider();
        /// <summary>
        /// 密钥
        /// </summary>
        private byte[] m_Base64IV = new byte[0];
        /// <summary>
        /// 初始化向量
        /// </summary>
        private byte[] m_Base64KEY = new byte[0];

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="strBase64KEY">密钥的 Base64 字符串</param>
        /// <param name="strBase64IV">初始化向量的 Base64 字符串</param>
        public StringCrypto(string strBase64KEY, string strBase64IV)
        {
            m_Base64KEY = Convert.FromBase64String(strBase64KEY);
            m_Base64IV = Convert.FromBase64String(strBase64IV);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strValue">加密的字符串</param>
        /// <returns>返回 Base64 字符串</returns>
        public string Encrypt(string strValue)
        {
            byte[] utf8Buffer = Encoding.UTF8.GetBytes(strValue);

            ICryptoTransform cryptoTransform = m_DESCrypto.CreateEncryptor(m_Base64KEY, m_Base64IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(utf8Buffer, 0, utf8Buffer.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strValue">Base64 字符串</param>
        /// <returns>返回 解密的字符串</returns>
        public string Decrypt(string strValue)
        {
            byte[] base64Buffer = Convert.FromBase64String(strValue);

            ICryptoTransform cryptoTransform = m_DESCrypto.CreateDecryptor(m_Base64KEY, m_Base64IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(base64Buffer, 0, base64Buffer.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strBase64KEY">密钥的 Base64 字符串</param>
        /// <param name="strBase64IV">初始化向量的 Base64 字符串</param>
        /// <param name="strValue">加密的字符串</param>
        /// <returns>返回 Base64 字符串</returns>
        public static string EncryptString(string strBase64KEY, string strBase64IV, string strValue)
        {
            using (SymmetricAlgorithm desCrypto = new DESCryptoServiceProvider())
            {
                ICryptoTransform cryptoTransform = desCrypto.CreateEncryptor(Convert.FromBase64String(strBase64KEY), Convert.FromBase64String(strBase64IV));
                byte[] utf8Buffer = Encoding.UTF8.GetBytes(strValue);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                    cryptoStream.Write(utf8Buffer, 0, utf8Buffer.Length);
                    cryptoStream.FlushFinalBlock();
                    cryptoStream.Close();

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strBase64KEY">密钥的 Base64 字符串</param>
        /// <param name="strBase64IV">初始化向量的 Base64 字符串</param>
        /// <param name="strValue">Base64 字符串</param>
        /// <returns>返回 解密的字符串</returns>
        public static string DecryptString(string strBase64KEY, string strBase64IV, string strValue)
        {
            using (SymmetricAlgorithm desCrypto = new DESCryptoServiceProvider())
            {
                ICryptoTransform cryptoTransform = desCrypto.CreateDecryptor(Convert.FromBase64String(strBase64KEY), Convert.FromBase64String(strBase64IV));
                byte[] base64Buffer = Convert.FromBase64String(strValue);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                    cryptoStream.Write(base64Buffer, 0, base64Buffer.Length);
                    cryptoStream.FlushFinalBlock();
                    cryptoStream.Close();

                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}
