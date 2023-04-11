using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace LockAndKey.Helpers
{
    public class CryptoHelper
    {
        #region Constants

        private const Byte _byteLength = 16;

        #endregion

        public static void CreateHashAndSalt(String password, out Byte[] hash, out Byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static Byte[] EncryptPasswordValue(String password, Byte[] secret, Byte[] iv)
        {
            Byte[] encrypted;
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = secret;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(password);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public static String DecryptPasswordValue(Byte[] enryptedPassword, Byte[] secret, Byte[] iv)
        {
            var decryptedString = String.Empty;
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = secret;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(enryptedPassword))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedString = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return decryptedString;
        }

        public static Byte[] CreateKey(String password)
        {
            return GetSuitableSizeByteArray(password);
        }

        public static Byte[] CreateIV(String username)
        {
            return GetSuitableSizeByteArray(username);
        }

        private static byte[] GetSuitableSizeByteArray(String value)
        {
            var currentBytes = Encoding.ASCII.GetBytes(value);
            if (currentBytes.Length != _byteLength)
            {
                var byteArrayList = new ArrayList();
                if (currentBytes.Length < _byteLength)
                {
                    var index = 1;
                    byteArrayList.AddRange(currentBytes);
                    while (byteArrayList.Count < _byteLength)
                    {
                        byteArrayList.Add((Byte)index);
                        index++;
                    }
                }
                else
                {
                    byteArrayList.AddRange(currentBytes);
                    byteArrayList.RemoveRange(_byteLength, byteArrayList.Count - _byteLength);
                }
                return (Byte[])byteArrayList.ToArray(typeof(Byte));
            }
            return currentBytes;
        }
    }
}
