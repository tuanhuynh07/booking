using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Microsoft.SqlServer.Server;

namespace Classes
{
    public class Security
    {
        public static string EncryptMd5(string strPlain)
        {
            MD5 md = MD5.Create();
            var enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(strPlain);
            byte[] hash = md.ComputeHash(buffer);
            var strHex = Convert.ToBase64String(hash);
            return strHex;

        }
        public static string RandomString(int size, bool isLower)
        {
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            char chuoi;
            for (int i = 0; i < size; i++)
            {
                if (isLower)
                    chuoi = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 96)));
                else
                {
                    if (rand.Next(0, 2) == 1)
                        chuoi = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 96)));
                    else
                    {
                        chuoi = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                    }
                }
                sb.Append(chuoi);
            }
            return sb.ToString();
        }
        public static string EncryptMd5Standard(string strPlain)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strPlain, "MD5");
        }

        public static string EncryptSha1(string strPlain)
        {

            SHA1 sha1 = SHA1.Create();
            var enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(strPlain);
            byte[] hash = sha1.ComputeHash(buffer);
            string strHex = Convert.ToBase64String(hash);
            return strHex;
        }
        public static string EncryptSha256(string strPlain)
        {

            SHA256 sha256 = SHA256.Create();
            var enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(strPlain);
            byte[] hash = sha256.ComputeHash(buffer);
            string strHex = Convert.ToBase64String(hash);
            return strHex;
        }

        public static string EncryptString(string yourString, string encrytKey)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(yourString);

            var hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(encrytKey));
            hashmd5.Clear();
            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string EncryptStringCbc(string textToEncrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
        public static string DecryptStringCbc(string textToDecrypt, string key)
        {
            try
            {
                var rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;
                byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string DecryptString(string yourString, string encrytKey)
        {
            string rs;
            try
            {
                byte[] toEncryptArray = Convert.FromBase64String(yourString);
                var hashmd5 = new MD5CryptoServiceProvider();
                byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(encrytKey));
                hashmd5.Clear();

                var tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                rs = Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                rs = "";

            }

            return rs;
        }

        //
        public static string DecryptStringAes(string inputEncrypted, string strKey, string strIv)
        {
            var keybytes = Encoding.UTF8.GetBytes(strKey);
            var iv = Encoding.UTF8.GetBytes(strIv);

            //c# encrrption
            //var encryptStringToBytes = EncryptStringToBytes("It works", keybytes, iv);

            // Decrypt the bytes to a string.
            //var roundtrip = DecryptStringFromBytes(encryptStringToBytes, keybytes, iv);

            //DECRYPT FROM CRIPTOJS
            var encrypted = Convert.FromBase64String(inputEncrypted);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return decriptedFromJavascript;
            //return string.Format(
            //    "roundtrip reuslt:{0}{1}Javascript result:{2}",
            //    roundtrip,
            //    Environment.NewLine,
            //    decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        //

        public static String EncryptIt(String s, byte[] key, byte[] IV)
        {
            String result;
            RijndaelManaged rijn = new RijndaelManaged();
            rijn.Mode = CipherMode.ECB;
            rijn.Padding = PaddingMode.Zeros;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (ICryptoTransform encryptor = rijn.CreateEncryptor(key, IV))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(s);
                        }
                    }
                }
                result = Convert.ToBase64String(msEncrypt.ToArray());
            }
            rijn.Clear();
            return result;
        }
        public static String DecryptIt(String s, byte[] key, byte[] IV)
        {
            String result;
            RijndaelManaged rijn = new RijndaelManaged();
            rijn.Mode = CipherMode.ECB;
            rijn.Padding = PaddingMode.Zeros;
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(s)))
            {
                using (ICryptoTransform decryptor = rijn.CreateDecryptor(key, IV))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                        {
                            result = swDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            rijn.Clear();
            return result;
        }
    }

}
