using System;
using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the MusicianConnection class.
    /// </summary>
    [TestClass]
    public class MusicianConnectionTests
    {
        private MusicianConnection _connection;
        private DateTime _testDate;

        /// <summary>
        /// Initializes _connection for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _connection = new MusicianConnection();
            _testDate = DateTime.Now;
        }

        /// <summary>
        /// Sets _connection to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _connection.ConnectionID = 0;
            _connection.InitiatorID = 0;
            _connection.InitiatorNames = null;
            _connection.FollowerNames = null;
            _connection.CreatedTime = DateTime.MinValue;
            _connection.Connected = false;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new MusicianConnection object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewMusicianConnection_ShouldReturnDefaultAttributes()
        {
            Assert.AreEqual(0, _connection.ConnectionID);
            Assert.AreEqual(0, _connection.InitiatorID);
            Assert.IsNull(_connection.InitiatorNames);
            Assert.IsNull(_connection.FollowerNames);
            Assert.AreEqual(DateTime.MinValue, _connection.CreatedTime);
            Assert.IsFalse(_connection.Connected);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a MusicianConnection object.
        /// </summary>
        [TestMethod]
        public void Test_SetNewMusicianConnectionAttributes_ShouldReturnSetAttributes()
        {
            _connection.ConnectionID++;
            _connection.InitiatorID = 2;
            _connection.InitiatorNames = "John Doe";
            _connection.FollowerNames = "Jane Doe";
            _connection.CreatedTime = _testDate;
            _connection.Connected = !_connection.Connected;

            Assert.AreEqual(1, _connection.ConnectionID);
            Assert.AreEqual(2, _connection.InitiatorID);
            Assert.AreEqual("John Doe", _connection.InitiatorNames);
            Assert.AreEqual("Jane Doe", _connection.FollowerNames);
            Assert.AreEqual(_testDate, _connection.CreatedTime);
            Assert.IsTrue(_connection.Connected);
        }
    }
}
