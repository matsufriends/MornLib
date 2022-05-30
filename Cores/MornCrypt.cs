using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace MornLib.Cores {
    public class MornCrypt {
        /// <summary>
        /// 暗号化
        /// </summary>
        /// <param name="text">平文</param>
        /// <param name="iv">128bit ブロックサイズ</param>
        /// <param name="key">256bit</param>
        /// <returns></returns>
        public static string Encrypt(string text,string iv,string key) {
            using var myRijndael = new RijndaelManaged {
                BlockSize = 128
               ,KeySize   = 256
               ,Mode      = CipherMode.CBC
               ,Padding   = PaddingMode.PKCS7
               ,IV        = Encoding.UTF8.GetBytes(iv)
               ,Key       = Encoding.UTF8.GetBytes(key)
            };

            var encryptor = myRijndael.CreateEncryptor(myRijndael.Key,myRijndael.IV);
            using var mStream = new MemoryStream();
            using var ctStream = new CryptoStream(mStream,encryptor,CryptoStreamMode.Write);
            using(var sw = new StreamWriter(ctStream)) {
                sw.Write(text);
            }
            var encrypted = mStream.ToArray();
            return Convert.ToBase64String(encrypted);
        }
        /// <summary>
        /// 復号
        /// </summary>
        /// <param name="cipher">暗号文</param>
        /// <param name="iv">128bit ブロックサイズ</param>
        /// <param name="key">256bit</param>
        /// <returns></returns>
        public static string Decrypt(string cipher,string iv,string key) {
            using var rijndael = new RijndaelManaged {
                BlockSize = 128
               ,KeySize   = 256
               ,Mode      = CipherMode.CBC
               ,Padding   = PaddingMode.PKCS7
               ,IV        = Encoding.UTF8.GetBytes(iv)
               ,Key       = Encoding.UTF8.GetBytes(key)
            };
            var decryotor = rijndael.CreateDecryptor(rijndael.Key,rijndael.IV);
            using var mStream = new MemoryStream(Convert.FromBase64String(cipher));
            using var ctStream = new CryptoStream(mStream,decryotor,CryptoStreamMode.Read);
            using var sr = new StreamReader(ctStream);
            var plain = sr.ReadLine();
            return plain;
        }
    }
}