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
    /// for data related to Post table in DB.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostDBDAL postDBDAL;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.postDBDAL = new PostDBDAL(_configuration);
        }

        /// <summary>
        /// Gets all post
        /// GET: api/post
        /// </summary>
        /// <returns>JsonResult table of all posts</returns>
        [HttpGet]
        public JsonResult GetPosts()
        {
            string selectStatement = 
            @"SELECT PostID, MusicianID, Content
            FROM dbo.Post";

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
        /// Gets all posts for specified users
        /// GET: api/post/UserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult table of user's posts</returns>
        [HttpGet("{id}")]
        public JsonResult GetPosts(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("UserID must be 1 or greater");
            }
            //Don't need to make query twice. Not sure how to incorporate DBDAL at this time
            //this.postDBDAL.GetPostByMusicianID(id);
            string selectStatement = 
            @"SELECT *
            FROM dbo.Post 
            WHERE musicianID = @id";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Gets all post likes for specified post
        /// GET: api/post/PostID/like
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>JsonResult table of post likes</returns>
        [HttpGet("{PostID}/like")]
        public JsonResult GetPostLikes(int postID)
        {
            if (postID < 1)
            {
                throw new ArgumentException("PostID must be greater than 0");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.PostLike 
            WHERE PostID = @PostID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@PostID", postID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Submits a post for the current user 
        /// POST: api/post
        /// </summary>
        /// <param name="newPost"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult Post(Post newPost)
        {
            if (String.IsNullOrWhiteSpace(newPost.Content))
            {
                throw new ArgumentException("Post cannot be empty");
            }
            if (newPost.MusicianID < 1)
            {
                throw new ArgumentException("Invalid MusicianID");
            }

            string insertStatement = @"INSERT INTO dbo.Post
                           VALUES (@CreatedTime, @MusicianID, @Content)";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@CreatedTime", newPost.CreatedTime);
                    myCommand.Parameters.AddWithValue("@MusicianID", newPost.MusicianID);
                    myCommand.Parameters.AddWithValue("@Content", newPost.Content);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Post Added Successfully");
        }

        /// <summary>
        /// Gets all comments for specified post
        /// GET: api/post/postID/comments
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>JsonResult table of post's comments</returns>
        [HttpGet("{postID}/comments")]
        public JsonResult GetPostComments(int postID)
        {
            if (postID < 1)
            {
                throw new ArgumentException("PostID must be greater than 0");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.Comment 
            WHERE PostID = @PostID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@PostID", postID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }
    }
}