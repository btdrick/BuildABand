using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

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
        public ActionResult<int> GetLogin([FromQuery][Required] string username, [FromQuery][Required] string password)
        {
            string selectStatement =
            @"
            SELECT Username, Password, MusicianID, is_Admin
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
                    myCommand.Parameters.AddWithValue("username", username);
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

            if (dbPassword == PasswordHash.GetSha256Hash(password))
            {
                int[] data = { row.Field<int>("MusicianID"), row.Field<byte>("is_Admin") };
                return Ok(data);
            }
            else
            {
                return BadRequest("Username or password is incorrect");
            }
        }
    }
}
