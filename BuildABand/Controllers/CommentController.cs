using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Comment table in DB.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CommentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets all comments
        /// GET: api/comment
        /// </summary>
        /// <returns>JsonResult table of all comments</returns>
        [HttpGet]
        public JsonResult GetComments()
        {
            string selectStatement =
            @"SELECT *
            FROM dbo.Comment";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Gets specified comment
        /// GET: api/comment/comment.CommentID
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult table of comment</returns>
        [HttpGet("{commentID}")]
        public JsonResult GetComment(int commentID)
        {
            if (commentID < 1)
            {
                throw new ArgumentException("CommentID must be greater than 0");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.Comment 
            WHERE CommentID = @CommentID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CommentID", commentID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Gets all comment likes for specified post
        /// GET: api/comment/CommentID/like
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns>JsonResult table of comment likes</returns>
        [HttpGet("{CommentID}/like")]
        public JsonResult GetCommentLikes(int commentID)
        {
            if (commentID < 1)
            {
                throw new ArgumentException("CommentID must be greater than 0");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.CommentLike 
            WHERE CommentID = @CommentID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CommentID", commentID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Submits a comment for the current user 
        /// POST: api/comment
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult CreateComment(Comment newComment)
        {
            if (newComment is null)
            {
                throw new ArgumentException("Comment cannot be null");
            }
            if (String.IsNullOrWhiteSpace(newComment.Content))
            {
                throw new ArgumentException("Comment cannot be empty");
            }
            if (newComment.MusicianID < 1)
            {
                throw new ArgumentException("Invalid MusicianID");
            }
            if (newComment.PostID < 1)
            {
                throw new ArgumentException("Invalid PostID");
            }

            string insertStatement = 
            @"INSERT INTO dbo.Comment
            VALUES (@CreatedTime, @ParentID, @MusicianID, @PostID, @Content)";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CreatedTime", newComment.CreatedTime);
                    if (newComment.ParentID == 0)
                    {
                        myCommand.Parameters.AddWithValue("@ParentID", DBNull.Value);
                    }
                    if (newComment.ParentID > 0)
                    {
                        myCommand.Parameters.AddWithValue("@ParentID", newComment.ParentID);
                    }                  
                    myCommand.Parameters.AddWithValue("@MusicianID", newComment.MusicianID);
                    myCommand.Parameters.AddWithValue("@PostID", newComment.PostID);
                    myCommand.Parameters.AddWithValue("@Content", newComment.Content);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Comment Added Successfully");
        }

        /// <summary>
        /// Updates a comment
        /// POST: api/comment/comment.CommentID
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult if updated successfully</returns>
        [HttpPatch("{comment.CommentID}")]
        public JsonResult UpdateComment(Comment comment)
        {
            if (comment is null)
            {
                throw new ArgumentException("Comment cannot be null");
            }

            string updateStatement = 
            @"UPDATE dbo.Comment 
            SET Content = @Content
            WHERE CommentID = @CommentID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(updateStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CommentID", comment.CommentID);
                    myCommand.Parameters.AddWithValue("@Content", comment.Content);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Comment Updated Successfully");
        }

        /// <summary>
        /// Deletes a comment  
        /// DELETE: api/comment/comment.CommentID
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult if deleted successfully</returns>
        [HttpDelete("{comment.CommentID}")]
        public JsonResult DeleteComment(Comment comment)
        {
            if (comment is null)
            {
                throw new ArgumentException("Comment cannot be null");
            }

            string deleteStatement = 
            @"DELETE FROM dbo.Comment 
            WHERE CommentID = @CommentID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(deleteStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CommentID", comment.CommentID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Comment Deleted Successfully");
        }
    }
}
