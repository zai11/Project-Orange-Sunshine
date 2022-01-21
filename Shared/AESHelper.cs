using System;
using System.Security.Cryptography;
using System.Text;

namespace ProjectOrangeSunshine.Shared
{
    public static class AESHelper
    {

        static  readonly UnicodeEncoding byteConverter = new();

        public static byte[] Encrypt(string dataToEncrypt, string key)
        {
            byte[] dataAsBytes = byteConverter.GetBytes(dataToEncrypt);
            return Encrypt(dataAsBytes, key);
        }

        public static byte[] Encrypt(byte[] dataToEncrypt, string key)
        {

            Aes aes = Aes.Create();
            SetupAesObject(ref aes, key);

            ICryptoTransform transform = aes.CreateEncryptor();
            return transform.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
        }

        public static string Decrypt(byte[] dataToDecrypt, string key)
        {
            Aes aes = Aes.Create();
            SetupAesObject(ref aes, key);

            byte[] textBytes = aes.CreateDecryptor().TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            return byteConverter.GetString(textBytes);
        }

        public static string GenerateKey()
        {
            byte[] data = RandomNumberGenerator.GetBytes(32);
            return byteConverter.GetString(data);
        }

        private static void SetupAesObject(ref Aes aes, string key)
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 256;
            aes.BlockSize = 128;

            byte[] keyBytes = byteConverter.GetBytes(key);
            byte[] ivBytes = new byte[16];
            int len = keyBytes.Length > ivBytes.Length ? ivBytes.Length : keyBytes.Length;
            Array.Copy(keyBytes, ivBytes, len);

            aes.Key = ivBytes;
            aes.IV = ivBytes;
        }
    }
}
