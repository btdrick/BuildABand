using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    /// <summary>
    /// Account table data access layer (DAL).
    /// </summary>
    public class AccountDAL
    {
        private IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public AccountDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Updates a login
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public JsonResult UpdateAccount(NewMusician musician)
        {
            if (!this.AccountExists(musician.AccountID))
            {
                throw new ArgumentException("Error: account does not exist");
            }

            string updateStatement = "UPDATE dbo.Accounts SET ";
            if (!string.IsNullOrEmpty(musician.Username) && !string.IsNullOrEmpty(musician.Password))
            {
                updateStatement += "Username = @Username, Password = @Password ";
            }
            else if (!string.IsNullOrEmpty(musician.Username))
            {
                updateStatement += "Username = @Username ";
            }
            else if (!string.IsNullOrEmpty(musician.Password))
            {
                updateStatement += "Password = @Password ";
            }
            updateStatement += "WHERE AccountID = @AccountID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(updateStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@AccountID", musician.AccountID);
                        if (!string.IsNullOrWhiteSpace(musician.Username))
                            myCommand.Parameters.AddWithValue("@Username", musician.Username);
                        if (!string.IsNullOrWhiteSpace(musician.Password))
                            myCommand.Parameters.AddWithValue("@Password", PasswordHash.GetSha256Hash(musician.Password));
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult("Account Updated Successfully");
        }

        /// <summary>
        /// Returns whether account is active
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns>True if account active</returns>
        public bool IsActiveAccount(int accountID)
        {
            if(!this.AccountExists(accountID))
            {
                throw new ArgumentException("Error: account does not exist");
            }

            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Accounts
            WHERE AccountID = @AccountID
            AND is_Active = 1
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@AccountID", accountID);
                        bool accountExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return accountExists;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    

        /// <summary>
        /// Deactivates an account
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns>JsonResult for deactivation result</returns>
        public JsonResult DeactivateAccount(int accountID)
        {
            if (!this.AccountExists(accountID))
            {
                throw new ArgumentException("Error: account does not exist");
            }

            string updateStatement = @"
            UPDATE dbo.Accounts
            SET is_Active = 0
            WHERE AccountID = @AccountID
            ";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(updateStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@AccountID", accountID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult("Account Deactivated Successfully");
        }

        /// <summary>
        /// Returns true if account exists.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns>True if account exists</returns>
        public bool AccountExists(int accountID)
        {
            if (accountID <= 0)
            {
                throw new ArgumentException("Error: account ID must be greater than 0");
            }

            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Accounts
            WHERE AccountID = @AccountID
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@AccountID", accountID);
                        bool accountExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return accountExists;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    }
}
