using System;
using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the Post class.
    /// </summary>
    [TestClass]
    public class PostTests
    {
        private Post _post;

        /// <summary>
        /// Initializes _post for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _post = new Post();
        }

        /// <summary>
        /// Sets _post to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _post.Content = null;
            _post.PostID = 0;
            _post.MusicianID = 0;
            _post.CreatedTime = DateTime.MinValue;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new Post object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewPost_ShouldReturnDefaultAttributes()
        {
            Assert.IsNull(_post.Content);
            Assert.AreEqual(0, _post.PostID);
            Assert.AreEqual(0, _post.MusicianID);
            Assert.AreEqual(DateTime.MinValue, _post.CreatedTime);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a Post object.
        /// </summary>
        [TestMethod]
        public void Test_SetPostAttributes_ShouldReturnSetAttributes()
        {
            _post.Content = "Test";
            _post.PostID++;
            _post.MusicianID++;
            _post.CreatedTime = DateTime.Parse("2022-06-11");

            Assert.AreEqual("Test", _post.Content);
            Assert.AreEqual(1, _post.PostID);
            Assert.AreEqual(1, _post.MusicianID);
            Assert.AreEqual(DateTime.Parse("2022-06-11"), _post.CreatedTime);
        }
    }
}
