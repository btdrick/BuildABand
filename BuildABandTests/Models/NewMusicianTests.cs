using System;
using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the NewMusician class.
    /// </summary>
    [TestClass]
    public class NewMusicianTests
    {
        private NewMusician _musician;
        private DateTime _testDate;

        /// <summary>
        /// Initializes _musician for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _musician = new NewMusician();
            _testDate = DateTime.Now;
        }

        /// <summary>
        /// Sets _musician to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _musician.AccountID = 0;
            _musician.Username = null;
            _musician.Password = null;
            _musician.Active = true;
            _musician.MusicianID = 0;
            _musician.Fname = null;
            _musician.Lname = null;
            _musician.DateOfbirth = DateTime.MinValue;
            _musician.Phone = null;
            _musician.Email = null;
            _musician.Instrument = null;
            _musician.Sex = null;
            _musician.Address1 = null;
            _musician.Address2 = null;
            _musician.City = null;
            _musician.ZipCode = null;
            _musician.StateCode = null;
            _musician.AvaterFilename = null;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new NewMusician object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewMusician_ShouldReturnDefaultAttributes()
        {
            Assert.AreEqual(0, _musician.AccountID);
            Assert.IsNull(_musician.Username);
            Assert.IsNull(_musician.Password);
            Assert.AreEqual(false, _musician.Active);
            Assert.AreEqual(0, _musician.MusicianID);
            Assert.IsNull(_musician.Fname);
            Assert.IsNull(_musician.Lname);
            Assert.AreEqual(DateTime.MinValue, _musician.DateOfbirth);
            Assert.IsNull(_musician.Phone);
            Assert.IsNull(_musician.Email);
            Assert.IsNull(_musician.Instrument);
            Assert.IsNull(_musician.Sex);
            Assert.IsNull(_musician.Address1);
            Assert.IsNull(_musician.Address2);
            Assert.IsNull(_musician.City);
            Assert.IsNull(_musician.ZipCode);
            Assert.IsNull(_musician.StateCode);
            Assert.IsNull(_musician.AvaterFilename);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a NewMusician object.
        /// </summary>
        [TestMethod]
        public void Test_SetNewMusicianAttributes_ShouldReturnSetAttributes()
        {
            _musician.AccountID = 1;
            _musician.Username = "jdoe";
            _musician.Password = "Test1234";
            _musician.Active = !_musician.Active;
            _musician.MusicianID++;
            _musician.Fname = "Jane";
            _musician.Lname = "Doe";
            _musician.DateOfbirth = _testDate;
            _musician.Phone = "123-456-7890";
            _musician.Email = "jdoe@jdoe.com";
            _musician.Instrument = "Ocarina";
            _musician.Sex = "Female";
            _musician.Address1 = "123 Street St.";
            _musician.Address2 = "#811-B";
            _musician.City = "Nowhereville";
            _musician.ZipCode = "90210";
            _musician.StateCode = "KS";
            _musician.AvaterFilename = "selfie.jpg";

            Assert.AreEqual(1, _musician.AccountID);
            Assert.AreEqual("jdoe", _musician.Username);
            Assert.AreEqual("Test1234", _musician.Password);
            Assert.AreEqual(true, _musician.Active);
            Assert.AreEqual(1, _musician.MusicianID);
            Assert.AreEqual("Jane", _musician.Fname);
            Assert.AreEqual("Doe", _musician.Lname);
            Assert.AreEqual(_testDate, _musician.DateOfbirth);
            Assert.AreEqual("123-456-7890", _musician.Phone);
            Assert.AreEqual("jdoe@jdoe.com", _musician.Email);
            Assert.AreEqual("Ocarina", _musician.Instrument);
            Assert.AreEqual("Female", _musician.Sex);
            Assert.AreEqual("123 Street St.", _musician.Address1);
            Assert.AreEqual("#811-B", _musician.Address2);
            Assert.AreEqual("Nowhereville", _musician.City);
            Assert.AreEqual("90210", _musician.ZipCode);
            Assert.AreEqual("KS", _musician.StateCode);
            Assert.AreEqual("selfie.jpg", _musician.AvaterFilename);
        }
    }
}
