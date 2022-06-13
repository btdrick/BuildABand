using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/login
        // Confirm login
        [HttpGet("login")]
        public ActionResult<int> GetLogin([FromQuery] string username, [FromQuery] string password)
        {
            string selectStatement =
            @"
            SELECT Username, Password, MusicianID
            FROM dbo.Accounts a 
            JOIN dbo.Musician m 
                ON a.AccountID = m.AccountID
            WHERE Username = @username
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

            if (resultsTable.Rows.Count == 0)
            {
                return BadRequest("Username or password is incorrect");
            }

            var row = resultsTable.Rows[0];
            var dbPassword = row.Field<string>("Password");

            if (dbPassword == password) // TODO implement hashing
            {
                return new ActionResult<int>(row.Field<int>("MusicianID"));
            }
            else
            {
                return BadRequest("Username or password is incorrect");
            }
        }
    }
}
