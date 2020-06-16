using System;
using System.Text;
using System.Security.Cryptography;

namespace SQLviewer
{
    /// <summary>
    /// Klasa odpowiadająca za szyfrowanie i deszyfrowanie haseł.
    /// </summary>
    public class PasswordHash
    {
        static string Key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";

        /// <summary>
        /// Metoda która pobiera hasło jako argument po czym zaszyfrowuje je.
        /// </summary>
        /// <param name="text">Hasło które zostanie poddane szyfrowaniu.</param>
        /// <returns>Zaszyfrowane hasło.</returns>
        public static string Encrypt(string text)
        {
            using var md5 = new MD5CryptoServiceProvider();
            using var tdes = new TripleDESCryptoServiceProvider
            {
                Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key)),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            using var transform = tdes.CreateEncryptor();
            byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
            byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Metoda która pobiera zaszyfrowane hasło jako argument po czym rozszyfrowuje je.
        /// </summary>
        /// <param name="cipher">Zaszyfrowany ciąg znaków który zostanie poddany rozszyfrowaniu.</param>
        /// <returns>Rozszyfrowane hasło.</returns>
        public static string Decrypt(string cipher)
        {
            using var md5 = new MD5CryptoServiceProvider();
            using var tdes = new TripleDESCryptoServiceProvider
            {
                Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key)),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            using var transform = tdes.CreateDecryptor();
            byte[] cipherBytes = Convert.FromBase64String(cipher);
            byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return UTF8Encoding.UTF8.GetString(bytes);
        }

    }
}
