using System;
using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the CommentLike class.
    /// </summary>
    [TestClass]
    public class CommentLikeTests
    {
        private CommentLike _commentLike;

        /// <summary>
        /// Initializes _commentLike for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _commentLike = new CommentLike();
        }

        /// <summary>
        /// Sets _commentLike to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _commentLike.CommentLikeID = 0;
            _commentLike.CommentID = 0;
            _commentLike.MusicianID = 0;
            _commentLike.CreatedTime = DateTime.MinValue;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new CommentLike object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewCommentLike_ShouldReturnDefaultAttributes()
        {
            Assert.AreEqual(0, _commentLike.CommentLikeID);
            Assert.AreEqual(0, _commentLike.CommentID);
            Assert.AreEqual(0, _commentLike.MusicianID);
            Assert.AreEqual(DateTime.MinValue, _commentLike.CreatedTime);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a CommentLike object.
        /// </summary>
        [TestMethod]
        public void Test_SetCommentLikeAttributes_ShouldReturnSetAttributes()
        {
            _commentLike.CommentLikeID = 1;
            _commentLike.CommentID++;
            _commentLike.MusicianID++;
            _commentLike.CreatedTime = DateTime.Parse("2022-06-11");

            Assert.AreEqual(1, _commentLike.CommentLikeID);
            Assert.AreEqual(1, _commentLike.CommentID);
            Assert.AreEqual(1, _commentLike.MusicianID);
            Assert.AreEqual(DateTime.Parse("2022-06-11"), _commentLike.CreatedTime);
        }
    }
}
