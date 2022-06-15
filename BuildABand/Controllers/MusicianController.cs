using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly UserDAL userSource;

        public MusicianController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.userSource = new UserDAL(_configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string selectStatement = @"SELECT Username, FName, LName FROM
                            dbo.User";

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

        // Post: api/musician
        // Post new musician
        [HttpPost]
        public JsonResult PostNewMusician(User user)
        {
            try
            {
                this.userSource.RegisterNewUser(user);
            }
           catch (Exception)
            {
               return new JsonResult("Username already exist");
            }

            return new JsonResult("New user created");
        }
    }
}
