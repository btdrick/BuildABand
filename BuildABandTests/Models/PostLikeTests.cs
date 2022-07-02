using System;
using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the PostLike class.
    /// </summary>
    [TestClass]
    public class PostLikeTests
    {
        private PostLike _postLike;

        /// <summary>
        /// Initializes _postLike for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _postLike = new PostLike();
        }

        /// <summary>
        /// Sets _postLike to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _postLike.PostLikeID = 0;
            _postLike.PostID = 0;
            _postLike.MusicianID = 0;
            _postLike.CreatedTime = DateTime.MinValue;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new PostLike object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewPostLike_ShouldReturnDefaultAttributes()
        {
            Assert.AreEqual(0, _postLike.PostLikeID);
            Assert.AreEqual(0, _postLike.PostID);
            Assert.AreEqual(0, _postLike.MusicianID);
            Assert.AreEqual(DateTime.MinValue, _postLike.CreatedTime);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a PostLike object.
        /// </summary>
        [TestMethod]
        public void Test_SetPostLikeAttributes_ShouldReturnSetAttributes()
        {
            _postLike.PostLikeID = 1;
            _postLike.PostID++;
            _postLike.MusicianID++;
            _postLike.CreatedTime = DateTime.Parse("2022-06-11");

            Assert.AreEqual(1, _postLike.PostLikeID);
            Assert.AreEqual(1, _postLike.PostID);
            Assert.AreEqual(1, _postLike.MusicianID);
            Assert.AreEqual(DateTime.Parse("2022-06-11"), _postLike.CreatedTime);
        }
    }
}
