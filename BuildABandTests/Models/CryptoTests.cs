using Microsoft.VisualStudio.TestTools.UnitTesting;
using BuildABand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildABand.Models.Tests
{
    [TestClass()]
    public class CryptoTests
    {
        /// <summary>
        /// Test to ensure that Generatekey method generates
        /// a 32bit random key
        /// </summary>
      
        [TestMethod()]
        public void EncryptTest()
        {
            string key = "8y/B?E(H+MbQeThW";
            String plainText = "Things we do";

            string cypherText = Crypto.Encrypt(key, plainText);

            Assert.IsNotNull(cypherText);
            Assert.AreNotEqual(plainText, cypherText);

        }

        [TestMethod()]
        public void DecryptTest()
        {
            String plainText = "Things we do";
            String key = "8y/B?E(H+MbQeThW";

            string cypherText = Crypto.Encrypt(key, plainText);
            string decryptedPlainText = Crypto.Decrypt(key, cypherText);

            Assert.IsNotNull(decryptedPlainText);
            Assert.AreEqual(plainText, decryptedPlainText);

        }
    }
}