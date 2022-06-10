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
    public class PasswordHashTests
    {
        [TestMethod()]
        public void GetSha256HashTest()
        {
            //Arrange
            string password = "test1234";
            string expectedHash = "g0pwm6JTTr4+4Tl/1Pe9KIsqzB0goI1shi3NmbbwRAA=";

            //act
            string hash = PasswordHash.GetSha256Hash(password);

            //assert
            
            Assert.AreEqual(expectedHash, hash);
        }
    }
}