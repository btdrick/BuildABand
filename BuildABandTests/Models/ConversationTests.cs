using System;
using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the Conversation class.
    /// </summary>
    [TestClass]
    public class ConversationTests
    {
        private Conversation _conversation;
        private DateTime _testDate;

        /// <summary>
        /// Initializes _conversation for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _conversation = new Conversation();
            _testDate = DateTime.Now;
        }

        /// <summary>
        /// Sets _conversation to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _conversation.ConversationID = 0;
            _conversation.SenderID = 0;
            _conversation.ReceiverID = 0;
            _conversation.CreatedTime = DateTime.MinValue;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new Message object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewMessage_ShouldReturnDefaultAttributes()
        {
            Assert.AreEqual(0, _conversation.ConversationID);
            Assert.AreEqual(0, _conversation.SenderID);
            Assert.AreEqual(0, _conversation.ReceiverID);
            Assert.AreEqual(DateTime.MinValue, _conversation.CreatedTime);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a Message object.
        /// </summary>
        [TestMethod]
        public void Test_SetNewMessageAttributes_ShouldReturnSetAttributes()
        {
            _conversation.ConversationID++;
            _conversation.SenderID++;
            _conversation.ReceiverID = 2;
            _conversation.CreatedTime = _testDate;

            Assert.AreEqual(1, _conversation.ConversationID);
            Assert.AreEqual(1, _conversation.SenderID);
            Assert.AreEqual(2, _conversation.ReceiverID);
            Assert.AreEqual(_testDate, _conversation.CreatedTime);
        }
    }
}
