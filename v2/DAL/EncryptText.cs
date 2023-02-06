using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;


namespace DAL.Encrypt
{
    public class EncryptTextTool
    {
        private static byte[] passwordKeyByte = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("ConnectionString.txt"));
        public static string EncryptText(string theText)
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(theText);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordKeyByte);

            string encryptedResult = Convert.ToBase64String(bytesEncrypted);

            return Convert.ToBase64String(bytesEncrypted);

        }
        public static string DecryptText(string encryptedText)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedText);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordKeyByte);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }
        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
        
            byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };
        
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
        
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
        
                    AES.Mode = CipherMode.CBC;
        
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
        
            return encryptedBytes;
        }
        
        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
        
            byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };
        
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
        
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
        
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
        
            return decryptedBytes;
        }
    }
}
