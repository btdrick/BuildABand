using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private PostDBDAL postDBDAL;
        private readonly IConfiguration _configuration;
        public PostsController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.postDBDAL = new PostDBDAL(_configuration);
        }

        // GET: api/posts
        // Gets all posts
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

        // GET: api/posts/UserID
        // Gets all posts for specified user
        [HttpGet("{id}")]
        public List<Post> GetPosts(int id)
        {
            return this.postDBDAL.GetPostByMusicianID(id);

            /*
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
            */
        }
    }
}