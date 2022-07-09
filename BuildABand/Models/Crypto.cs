using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    public class Crypto
    {
        public static byte[] Encrypt(string plainText, byte[] key)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;

            using var memStream = new MemoryStream();
            memStream.Write(aes.IV, 0, aes.IV.Length);

            using var cryptoStream = new CryptoStream(
                memStream,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            cryptoStream.Write(plainTextBytes);
            cryptoStream.FlushFinalBlock();

            memStream.Position = 0;

            return memStream.ToArray();

        }

        public static string Decrypt(byte[] cypherBytes, byte[] key)
        {
            using var memStream = new MemoryStream();
            memStream.Write(cypherBytes);
            memStream.Position = 0;

            using var aes = Aes.Create();

            byte[] iv = new byte[aes.IV.Length];
            memStream.Read(iv, 0, iv.Length);

            using var cryptoStream = new CryptoStream(
                memStream,
                aes.CreateDecryptor(key, iv),
                CryptoStreamMode.Read);


            int plainTextByteLength = cypherBytes.Length - iv.Length;
            var plainTextBytes = new byte[plainTextByteLength];
            cryptoStream.Read(plainTextBytes, 0, plainTextByteLength);

            return Encoding.UTF8.GetString(plainTextBytes);
        }

        public static byte[] GenerateKey()
        {
            const int KeyLength = 32;

            byte[] key = new byte[KeyLength];
            var rngRand = new RNGCryptoServiceProvider();

            rngRand.GetBytes(key);

            return key;
        }
    }
}
