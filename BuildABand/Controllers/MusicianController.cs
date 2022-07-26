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
    /// for data related to Musician table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for Musician media.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MusicianDAL userSource;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public MusicianController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.userSource = new MusicianDAL(_configuration);
        }

        /// <summary>
        /// Gets all musician
        /// GET: api/musician
        /// </summary>
        /// <returns>JsonResult table of all musicians</returns>
        [HttpGet]
        public JsonResult Get()
        {
            string selectStatement = @"
            SELECT * 
            FROM dbo.Musician
            ";

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
        /// Gets all active musician
        /// GET: api/musician/active
        /// </summary>
        /// <returns>JsonResult table of all active musicians</returns>
        [HttpGet("Active")]
        public JsonResult GetActiveMusicians()
        {
            string selectStatement = @"
            SELECT * 
            FROM dbo.Musician
            WHERE AccountID IN 
                (SELECT m.AccountID 
                FROM dbo.Musician m 
                JOIN Accounts a 
                ON m.AccountID = a.AccountID 
                WHERE a.is_Active = 1)
            ";

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
        /// Gets specified musician by id
        /// GET: api/musician/MusicianID
        /// </summary>
        /// <returns>JsonResult table of musician</returns>
        [HttpGet("{MusicianID}")]
        public JsonResult GetMusician(int musicianID)
        {
            if (musicianID < 1)
            {
                throw new ArgumentException("MusicianID must be greater than 0");
            }

            string selectStatement =
            @"SELECT * 
            FROM dbo.Musician
            WHERE MusicianID = @MusicianID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@MusicianID", musicianID);
                    dataReader = myCommand.ExecuteReader();
                    resultsTable.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(resultsTable);
        }

        // Post: api/musician
        // Post new musician
        [HttpPost]
        public JsonResult PostNewMusician(NewMusician user)
        {
            if (user == null)
                throw new ArgumentException("Invalid arguement");
            try
            {
                this.userSource.RegisterNewUser(user);
            }
           catch (Exception ex)
            {
               return new JsonResult(ex.Message);
            }

            return new JsonResult("New user created");
        }

        /// <summary>
        /// Updates Musician info by ID
        /// PATCH: api/musician/MusicianID
        /// </summary>
        /// <param name="musician"></param>
        /// <returns>JsonResult of update status</returns>
        [HttpPatch("{musician.MusicianID}")]
        public JsonResult UpdateMusicianInfo(Musician musician)
        {
            return this.userSource.UpdateMusicianInfo(musician);
        }
    }
}
