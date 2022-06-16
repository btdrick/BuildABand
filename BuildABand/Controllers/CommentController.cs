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
        /// Gets all comments for specified post
        /// GET: api/comment/PostID
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>JsonResult table of post's comments</returns>
        [HttpGet("{PostID}")]
        public JsonResult GetPosts(int postID)
        {
            if (postID < 1)
            {
                throw new ArgumentException("PostID must be 1 or greater");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.Comment 
            WHERE PostID = @postID";

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
        /// Submits a comment for the current user 
        /// POST: api/comment
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult Comment(Comment newComment)
        {
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

            string insertStatement = @"INSERT INTO dbo.Comment
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
    }
}
