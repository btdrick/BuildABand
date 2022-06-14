using System;
using BuildABand.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildABandTests.Models
{
    /// <summary>
    /// This class serves as a series
    /// of unit tests for the Comment class.
    /// </summary>
    [TestClass]
    public class CommentTests
    {
        private Comment _comment;

        /// <summary>
        /// Initializes _comment for testing.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _comment = new Comment();
        }

        /// <summary>
        /// Sets _post to default values.
        /// </summary>
        [TestCleanup]
        public void TestClean()
        {
            _comment.CommentID = 0;
            _comment.PostID = 0;
            _comment.MusicianID = 0;
            _comment.CreatedTime = DateTime.MinValue;
            _comment.Content = null;
        }

        /// <summary>
        /// This test checks for the default
        /// attributes of a new Comment object.
        /// </summary>
        [TestMethod]
        public void Test_CreateNewComment_ShouldReturnDefaultAttributes()
        {
            Assert.IsNull(_comment.Content);
            Assert.AreEqual(0, _comment.CommentID);
            Assert.AreEqual(0, _comment.PostID);
            Assert.AreEqual(0, _comment.MusicianID);
            Assert.AreEqual(DateTime.MinValue, _comment.CreatedTime);
        }

        /// <summary>
        /// This test checks for set
        /// attributes of a Comment object.
        /// </summary>
        [TestMethod]
        public void Test_SetCommentAttributes_ShouldReturnSetAttributes()
        {
            _comment.Content = "Test";
            _comment.CommentID++;
            _comment.PostID++;
            _comment.MusicianID++;
            _comment.CreatedTime = DateTime.Parse("2022-06-11");

            Assert.AreEqual("Test", _comment.Content);
            Assert.AreEqual(1, _comment.CommentID);
            Assert.AreEqual(1, _comment.PostID);
            Assert.AreEqual(1, _comment.MusicianID);
            Assert.AreEqual(DateTime.Parse("2022-06-11"), _comment.CreatedTime);
        }
    }
}
