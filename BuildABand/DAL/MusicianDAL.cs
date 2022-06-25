using BuildABand.Controllers;
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
    public class MusicianDAL
    {
        private IConfiguration _configuration;


        public MusicianDAL(IConfiguration configuration)
        {
            _configuration = configuration;
      
        }

        /// <summary>
        /// Add new user to database
        /// </summary>
        /// <param name="musician"> user</param>
        public void RegisterNewUser(NewMusician musician)
        {
          
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
               
                {
                    using (SqlCommand insertCommand = new SqlCommand("dbo.createUser", connection))
                    {
                        insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        insertCommand.Parameters.AddWithValue("@Username", musician.Username);
                        insertCommand.Parameters.AddWithValue("@Password", PasswordHash.GetSha256Hash(musician.Password));
                        insertCommand.Parameters.AddWithValue("@Fname", musician.Fname);
                        insertCommand.Parameters.AddWithValue("@Lname", musician.Lname);
                        insertCommand.Parameters.AddWithValue("@DateOfBirth", musician.DateOfbirth);
                        insertCommand.Parameters.AddWithValue("@Phone", musician.Phone);
                        insertCommand.Parameters.AddWithValue("@Email", musician.Email);
                        if (string.IsNullOrEmpty(musician.Instrument))
                             insertCommand.Parameters.AddWithValue("@Instrument", DBNull.Value);
                        else
                            insertCommand.Parameters.AddWithValue("@Instrument", musician.Instrument); 
                        insertCommand.Parameters.AddWithValue("@Sex", musician.Sex);
                        insertCommand.Parameters.AddWithValue("@Address1", musician.Address1);
                        if (string.IsNullOrEmpty(musician.Address2))
                            insertCommand.Parameters.AddWithValue("Address2", DBNull.Value);
                        else
                            insertCommand.Parameters.AddWithValue("@Address2", musician.Address2);
                        
                        insertCommand.Parameters.AddWithValue("@City", musician.City);
                        insertCommand.Parameters.AddWithValue("@Zipcode", musician.ZipCode);
                        insertCommand.Parameters.AddWithValue("@Statecode", musician.StateCode);
                        if (string.IsNullOrEmpty(musician.AvaterFilename))
                            insertCommand.Parameters.AddWithValue("@AvaterFilename", DBNull.Value);
                        else
                            insertCommand.Parameters.AddWithValue("@AvaterFilename", musician.AvaterFilename);

                        insertCommand.ExecuteNonQuery();

                    }
                }
            }

        }
    }
}
