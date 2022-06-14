using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    public class UserDAL
    {
        private IConfiguration _configuration;

        public UserDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Add new user to database
        /// </summary>
        /// <param name="user"> user</param>
        public void RegisterNewUser(User user)
        {
          
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
               
                {
                    using (SqlCommand insertCommand = new SqlCommand("dbo.createUser", connection))
                    {
                        insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        insertCommand.Parameters.AddWithValue("@Username", user.Username);
                        insertCommand.Parameters.AddWithValue("@Password", user.Password);
                        insertCommand.Parameters.AddWithValue("@Fname", user.Fname);
                        insertCommand.Parameters.AddWithValue("@Lname", user.Lname);
                        insertCommand.Parameters.AddWithValue("@DateOfBirth", user.DateOfbirth);
                        insertCommand.Parameters.AddWithValue("@Phone", user.Phone);
                        insertCommand.Parameters.AddWithValue("@Email", user.Email);
                        insertCommand.Parameters.AddWithValue("@Instrument", user.Instrument); 
                        insertCommand.Parameters.AddWithValue("@Sex", user.Sex);
                        insertCommand.Parameters.AddWithValue("@Address1", user.Address1);
                        if (string.IsNullOrEmpty(user.Address2))
                            insertCommand.Parameters.AddWithValue("Address2", DBNull.Value);
                        else
                            insertCommand.Parameters.AddWithValue("@Address2", user.Address2);
                        
                        insertCommand.Parameters.AddWithValue("@City", user.City);
                        insertCommand.Parameters.AddWithValue("@Zipcode", user.ZipCode);
                        insertCommand.Parameters.AddWithValue("@Statecode", user.StateCode);
                        if (string.IsNullOrEmpty(user.AvaterFilename))
                            insertCommand.Parameters.AddWithValue("@AvaterFilename", DBNull.Value);
                        else
                            insertCommand.Parameters.AddWithValue("@AvaterFilename", user.AvaterFilename);

                        insertCommand.ExecuteNonQuery();

                    }
                }
            }

        }
    }
}
