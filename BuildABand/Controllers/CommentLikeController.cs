using BuildABand.DAL;
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
    /// for data related to CommentLike table in DB.
    /// </summary>
    [Route("api/comment/like")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private CommentLikeDAL commentLikeDAL;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CommentLikeController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.commentLikeDAL = new CommentLikeDAL(configuration);
        }

        /// <summary>
        /// Gets all comment likes
        /// GET: api/comment/like
        /// </summary>
        /// <returns>JsonResult table of all comments</returns>
        [HttpGet]
        public JsonResult GetComments()
        {
            string selectStatement =
            @"SELECT *
            FROM dbo.CommentLike";

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
        /// Gets the specified like
        /// GET: api/comment/like/CommentLikeID
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns>JsonResult table of comment</returns>
        [HttpGet("{CommentLikeID}")]
        public JsonResult GetCommentLike(int commentLikeID)
        {
            if (commentLikeID < 1)
            {
                throw new ArgumentException("LikeID must be greater than 0");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.CommentLike 
            WHERE CommentLikeID = @CommentLikeID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CommentLikeID", commentLikeID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Submits a comment like by the current user 
        /// POST: api/comment/like
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult LikeComment(CommentLike newCommentLike)
        {
            if (this.commentLikeDAL.UserAlreadyLikedComment(newCommentLike))
            {
                return new JsonResult("User has already liked this comment");
            }

            try
            {
                string insertStatement = 
                @"INSERT INTO dbo.CommentLike " +
                "VALUES (@CommentID, @MusicianID, @CreatedTime)";

                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@CommentID", newCommentLike.CommentID);
                        myCommand.Parameters.AddWithValue("@MusicianID", newCommentLike.MusicianID);
                        myCommand.Parameters.AddWithValue("@CreatedTime", newCommentLike.CreatedTime);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult("Comment Liked Successfully");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Deletes specified like
        /// DELETE: api/comment/like/CommentLikeID
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult if deleted successfully</returns>
        [HttpDelete("{CommentLikeID}")]
        public JsonResult DeleteComment(int commentLikeID)
        {
            if (!this.commentLikeDAL.CommentLikeExists(commentLikeID))
            {
                throw new ArgumentException("Error: comment like does not exist");
            }

            string deleteStatement = 
                @"DELETE FROM dbo.CommentLike " +
                "WHERE CommentLikeID = @CommentLikeID " +
                "DECLARE @lastID int " +
                "SELECT @lastID = MAX(CommentLikeID) FROM dbo.CommentLike " +
                "DBCC CHECKIDENT(CommentLike, RESEED, @lastID)";
            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(deleteStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CommentLikeID", commentLikeID);
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
