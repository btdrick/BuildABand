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
        public void GenerateKeyTest()
        {
            byte[] rndkey = Crypto.GenerateKey();
            Assert.AreEqual(rndkey.Length, 32);

        }

        [TestMethod()]
        public void EncryptTest()
        {
            byte[] rndkey = Crypto.GenerateKey();
            String planText = "Things we do";

            byte[] cypherBytes = Crypto.Encrypt(planText, rndkey);

            string cypherText = Convert.ToBase64String(cypherBytes);

            Assert.IsNotNull(cypherText);
            Assert.AreNotEqual(planText, cypherText);

        }

        [TestMethod()]
        public void DecryptTest()
        {
            byte[] rndkey = Crypto.GenerateKey();
            String planText = "Things we do";

            byte[] cypherBytes = Crypto.Encrypt(planText, rndkey);
            string decryptedPlainText = Crypto.Decrypt(cypherBytes, rndkey);

            Assert.IsNotNull(decryptedPlainText);
            Assert.AreNotEqual(planText, decryptedPlainText);

        }
    }
}