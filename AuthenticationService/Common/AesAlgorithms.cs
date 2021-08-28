using System;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Common
{
    public static class AesAlgorithms
    {
        private static readonly string KEY = "qwertyuiopasdfgh";
        private static readonly string IV = "~@!river9611#&53";

        public static string EncryptAes(string password)
        {
            var sourceBytes = Encoding.UTF8.GetBytes(password);
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(KEY);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                var transform = aes.CreateEncryptor();
                return Convert.ToBase64String(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));
            }
        }

        public static string DecryptAes(string password)
        {
            var encryptBytes = Convert.FromBase64String(password);
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(KEY);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                var transform = aes.CreateDecryptor();
                return Encoding.UTF8.GetString(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length));
            }
        }
    }
}
