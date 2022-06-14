using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the Musician class.
    /// </summary>
    [TestClass]
    public class MusicianTests
    {
        private Musician _musician;

        /// <summary>
        /// Initializes _musician for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _musician = new Musician();
        }

        /// <summary>
        /// Sets _musician to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _musician.musicianID = 0;
            _musician.FirstName = null;
            _musician.LastName = null;
            _musician.Email = null;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new Musician object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewMusician_ShouldReturnDefaultAttributes()
        {
            Assert.AreEqual(0, _musician.musicianID);
            Assert.IsNull(_musician.FirstName);
            Assert.IsNull(_musician.LastName);
            Assert.IsNull(_musician.Email);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a Musician object.
        /// </summary>
        [TestMethod]
        public void Test_SetMusicianAttributes_ShouldReturnSetAttributes()
        {
            _musician.musicianID++;
            _musician.FirstName = "John";
            _musician.LastName = "Doe";
            _musician.Email = "jdoe@example.com";

            Assert.AreEqual(1, _musician.musicianID);
            Assert.AreEqual("John", _musician.FirstName);
            Assert.AreEqual("Doe", _musician.LastName);
            Assert.AreEqual("jdoe@example.com", _musician.Email);
        }
    }
}
