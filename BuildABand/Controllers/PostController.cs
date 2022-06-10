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
        private PostDBDAL postDBDAL;
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
        /// Gets all posts
        /// GET: api/posts
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
        /// GET: api/posts/UserID
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
        /// Submits a post for the current user 
        /// POST: api/posts
        /// </summary>
        /// <param name="newPost"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult Post(Post newPost)
        {
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

            return new JsonResult("Added Successfully");
        }
    }
}