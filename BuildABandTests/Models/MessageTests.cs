using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the Message class.
    /// </summary>
    [TestClass]
    public class  MessageTests
    {
        private Message _message;
        private DateTime _testDate;

        /// <summary>
        /// Initializes _message for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _message = new Message();
            _testDate = DateTime.Now;
        }

        /// <summary>
        /// Sets _message to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _message.MessageID = 0;
            _message.ConversationID = 0;
            _message.CreatedTime = DateTime.MinValue;
            _message.SenderID = 0;
            _message.Text = null;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new Message object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewMessage_ShouldReturnDefaultAttributes()
        {
            Assert.AreEqual(0, _message.MessageID);
            Assert.AreEqual(0, _message.ConversationID);
            Assert.AreEqual(DateTime.MinValue, _message.CreatedTime);
            Assert.AreEqual(0, _message.SenderID);
            Assert.IsNull(_message.Text);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a Message object.
        /// </summary>
        [TestMethod]
        public void Test_SetNewMessageAttributes_ShouldReturnSetAttributes()
        {
            _message.MessageID++;
            _message.ConversationID++;
            _message.CreatedTime = _testDate;
            _message.SenderID++;
            _message.Text = "Hello!";

            Assert.AreEqual(1, _message.MessageID);
            Assert.AreEqual(1, _message.ConversationID);
            Assert.AreEqual(_testDate, _message.CreatedTime);
            Assert.AreEqual(1, _message.SenderID);
            Assert.AreEqual("Hello!", _message.Text);
        }
    }
}
