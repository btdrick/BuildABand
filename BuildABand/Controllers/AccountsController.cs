using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Accounts table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for Account media.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IConfiguration _configuration;
        private AccountDAL accountDAL;

        /// <summary>
        /// 1-para, costructor
        /// </summary>
        /// <param name="configuration"></param>
        public AccountsController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.accountDAL = new AccountDAL(configuration);
        }

        /// <summary>
        /// Validates login credentials 
        /// GET: api/accounts/login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>AccountID and Admin status if successful</returns>
        [HttpGet("login")]
        public ActionResult<int> ValidateLogin([FromQuery][Required] string username, [FromQuery][Required] string password)
        {
            string selectStatement =
            @"
            SELECT Username, Password, MusicianID, is_Admin
            FROM dbo.Accounts a 
            JOIN dbo.Musician m 
                ON a.AccountID = m.AccountID
            WHERE Username = @username
            AND is_Active = 1
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

            if (dbPassword != PasswordHash.GetSha256Hash(password))
            {
                return BadRequest("Username or password is incorrect");
            }

            int[] data = { row.Field<int>("MusicianID"), row.Field<byte>("is_Admin") };
            return Ok(data);
        }

        /// <summary>
        /// Update row for account
        /// PATCH: api/accounts/accountID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns>JsonResult if updated successfully</returns>
        [HttpPatch("{AccountID}")]
        public JsonResult UpdateAccount(NewMusician musician)
        {
            return this.accountDAL.UpdateAccount(musician);
        }

        /// <summary>
        /// Deactivate an account
        /// POST: api/accounts/accountID/deactivate
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult if updated successfully</returns>
        [HttpPatch("{AccountID}/deactivate")]
        public JsonResult DeactivateAccount(int accountID)
        {
            return this.accountDAL.DeactivateAccount(accountID);
        }
    }
}
