using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    /// <summary>
    /// Model a password hash generator
    /// </summary>
    public class PasswordHash
    {
        private static byte[] GenerateSalt()
        {
            const int SaltLength = 64;
            byte[] salt = new byte[SaltLength];
            var rngRand = new RNGCryptoServiceProvider();

            rngRand.GetBytes(salt);

            return salt;
        }

        private static byte[] GenerateSha256Hash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] saltedPassword =
                new byte[salt.Length + passwordBytes.Length];

            using (var hash = new SHA256CryptoServiceProvider())
            {
                return hash.ComputeHash(saltedPassword);
            }
        }


        /// <summary>
        /// Generate Sha256 hash from a string using 64bits salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hash string</returns>
        public static string GetSha256Hash(String password)
        {
            byte[] salt = GenerateSalt();
            byte[] sha256Hash = GenerateSha256Hash(password, salt);
            string sha256HashString = Convert.ToBase64String(sha256Hash);
            return sha256HashString;
        }
    }
}
