using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ProjectOrangeSunshine.Shared
{
    public static class RSAHelper
    {

        private static readonly RSACryptoServiceProvider rsa = new();
        private static RSAParameters keyInfo;

        private static readonly UnicodeEncoding byteConverter = new();

        public static byte[] Encrypt(string textToEncrypt)
        {
            byte[] data = byteConverter.GetBytes(textToEncrypt);
            return Encrypt(data);
        }

        public static byte[] Encrypt(byte[] dataToEncrypt)
        {
            return rsa.Encrypt(dataToEncrypt, false);
        }

        public static string Decrypt(byte[] dataToDecrypt)
        {
            byte[] decrypted = Array.Empty<byte>();
            try
            {
                decrypted = rsa.Decrypt(dataToDecrypt, false);
            }
            catch(Exception e)
            {
                Logger.LogServerError(100, e.ToString());
            }

            return byteConverter.GetString(decrypted);
        }

        public static void ExportPublicKey(string publicKeyFilePath)
        {
            string publicKey = GetPublicKey();
            using StreamWriter writer = new(publicKeyFilePath);
            writer.Write(publicKey);
        }

        public static void ExportPrivateKey(string privateKeyFilePath)
        {
            string privateKey = GetPrivateKey();
            using StreamWriter writer = new(privateKeyFilePath);
            writer.Write(privateKey);
        }

        public static void ImportPublicKeyFromFile(string publicKeyFilePath)
        {
            string publicKey;

            using (StreamReader reader = new(publicKeyFilePath))
            {
                publicKey = reader.ReadToEnd();
            }

            ImportPublicKey(publicKey);
        }

        public static void ImportPublicKey(string publicKey)
        {
            publicKey = publicKey[52..^52];

            string[] parts = publicKey.Split("~");

            keyInfo.Modulus = Convert.FromBase64String(parts[0]);
            keyInfo.Exponent = Convert.FromBase64String(parts[1]);

            rsa.ImportParameters(keyInfo);
        }

        public static void ImportPrivateKeyFromFile(string privateKeyFilePath)
        {
            string privateKey;

            keyInfo = new();

            using (StreamReader reader = new(privateKeyFilePath))
            {
                privateKey = reader.ReadToEnd();
            }

            ImportPrivateKey(privateKey);
        }

        public static void ImportPrivateKey(string privateKey)
        {
            privateKey = privateKey[52..^52];
               
            string[] parts = privateKey.Split("~");

            keyInfo.Modulus = Convert.FromBase64String(parts[0]);
            keyInfo.Exponent = Convert.FromBase64String(parts[1]);
            keyInfo.D = Convert.FromBase64String(parts[2]);
            keyInfo.DP = Convert.FromBase64String(parts[3]);
            keyInfo.DQ = Convert.FromBase64String(parts[4]);
            keyInfo.InverseQ = Convert.FromBase64String(parts[5]);
            keyInfo.P = Convert.FromBase64String(parts[6]);
            keyInfo.Q = Convert.FromBase64String(parts[7]);

            rsa.ImportParameters(keyInfo);
        }

        public static string GetPublicKey()
        {
            keyInfo = rsa.ExportParameters(false);
            byte[] modulus = keyInfo.Modulus ?? Array.Empty<byte>();
            byte[] exponent = keyInfo.Exponent ?? Array.Empty<byte>();
            string mod_64 = Convert.ToBase64String(modulus);
            string exp_64 = Convert.ToBase64String(exponent);
            StringBuilder sb = new();
            sb.Append("#---------------- Begin Public Key ---------------#\n");
            sb.Append(mod_64 + "~");
            sb.Append(exp_64 + "\n");
            sb.Append("#----------------- End Public Key ----------------#");
            return sb.ToString();
        }

        private static string GetPrivateKey()
        {
            keyInfo = rsa.ExportParameters(true);
            byte[] mod = keyInfo.Modulus ?? Array.Empty<byte>();
            byte[] exponent = keyInfo.Exponent ?? Array.Empty<byte>();
            byte[] d = keyInfo.D ?? Array.Empty<byte>();
            byte[] dp = keyInfo.DP ?? Array.Empty<byte>();
            byte[] dq = keyInfo.DQ ?? Array.Empty<byte>();
            byte[] inverseQ = keyInfo.InverseQ ?? Array.Empty<byte>();
            byte[] p = keyInfo.P ?? Array.Empty<byte>();
            byte[] q = keyInfo.Q ?? Array.Empty<byte>();
            string mod_64 = Convert.ToBase64String(mod);
            string exp_64 = Convert.ToBase64String(exponent);
            string d_64 = Convert.ToBase64String(d);
            string dp_64 = Convert.ToBase64String(dp);
            string dq_64 = Convert.ToBase64String(dq);
            string inv_64 = Convert.ToBase64String(inverseQ);
            string p_64 = Convert.ToBase64String(p);
            string q_64 = Convert.ToBase64String(q);
            StringBuilder sb = new();
            sb.Append("#--------------- Begin Private Key ---------------#\n");
            sb.Append(mod_64 + "~");
            sb.Append(exp_64 + "~");
            sb.Append(d_64 + "~");
            sb.Append(dp_64 + "~");
            sb.Append(dq_64 + "~");
            sb.Append(inv_64 + "~");
            sb.Append(p_64 + "~");
            sb.Append(q_64 + "\n");
            sb.Append("#---------------- End Private Key ----------------#");
            return sb.ToString();
        }
    }
}
