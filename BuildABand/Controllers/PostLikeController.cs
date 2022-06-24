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
    /// for data related to PostLike table in DB.
    /// </summary>
    [Route("api/post/like")]
    [ApiController]
    public class PostLikeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private PostLikeDAL postLikeDAL;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public PostLikeController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.postLikeDAL = new PostLikeDAL(configuration);
        }

        /// <summary>
        /// Gets all post likes
        /// GET: api/post/like
        /// </summary>
        /// <returns>JsonResult table of all post likes</returns>
        [HttpGet]
        public JsonResult GetPostLikes()
        {
            string selectStatement =
            @"SELECT *
            FROM dbo.PostLike";

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
        /// Gets the specified post like
        /// GET: api/post/like/PostLikeID
        /// </summary>
        /// <param name="postLikeID"></param>
        /// <returns>JsonResult table of post like</returns>
        [HttpGet("{PostLikeID}")]
        public JsonResult GetPostLike(int postLikeID)
        {
            if (postLikeID < 1)
            {
                throw new ArgumentException("LikeID must be greater than 0");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.PostLike 
            WHERE PostLikeID = @PostLikeID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@PostLikeID", postLikeID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Submits a post like by the current user 
        /// POST: api/post/like
        /// </summary>
        /// <param name="newPostLike"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult LikePost(PostLike newPostLike)
        {
            if (this.postLikeDAL.UserAlreadyLikedPost(newPostLike))
            {
                return new JsonResult("User has already liked this post");
            }

            try
            {
                string insertStatement =
                @"INSERT INTO dbo.PostLike " +
                "VALUES (@PostID, @MusicianID, @CreatedTime)";

                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@PostID", newPostLike.PostID);
                        myCommand.Parameters.AddWithValue("@MusicianID", newPostLike.MusicianID);
                        myCommand.Parameters.AddWithValue("@CreatedTime", newPostLike.CreatedTime);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult("Post Liked Successfully");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Deletes specified like
        /// DELETE: api/post/like/PostLikeID
        /// </summary>
        /// <param name="postLikeID"></param>
        /// <returns>JsonResult if deleted successfully</returns>
        [HttpDelete("{PostLikeID}")]
        public JsonResult DeletePost(int postLikeID)
        {
            if (!this.postLikeDAL.PostLikeExists(postLikeID))
            {
                throw new ArgumentException("Error: post like does not exist");
            }

            string deleteStatement =
                @"DELETE FROM dbo.PostLike " +
                "WHERE PostLikeID = @PostLikeID " +
                "DECLARE @lastID int " +
                "SELECT @lastID = MAX(PostLikeID) FROM dbo.PostLike " +
                "DBCC CHECKIDENT(PostLike, RESEED, @lastID)";
            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(deleteStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@PostLikeID", postLikeID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Post Like Deleted Successfully");
        }
    }
}
