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
            string key = "Where we are warriors";
            String planText = "Things we do";

            string cypherText = Crypto.Encrypt(planText, key);

            Assert.IsNotNull(cypherText);
            Assert.AreNotEqual(planText, cypherText);

        }

        [TestMethod()]
        public void DecryptTest()
        {
            String planText = "Things we do";
            String key = "Where we are warriors";

            string cypherText = Crypto.Encrypt(planText, key);
            string decryptedPlainText = Crypto.Decrypt(cypherText, key);

            Assert.IsNotNull(decryptedPlainText);
            Assert.AreEqual(planText, decryptedPlainText);

        }
    }
}