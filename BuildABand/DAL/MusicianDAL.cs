using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

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

        /// <summary>
        /// Updates musician row in table
        /// </summary>
        /// <param name="musician"></param>
        /// <returns>JsonResult with update status</returns>
        public JsonResult UpdateMusicianInfo(Musician musician)
        {
                if (musician is null)
                {
                    throw new ArgumentException("Error: musician cannot be null");
                }
                if (!this.MusicianExists(musician.musicianID))
                {
                    throw new ArgumentException("Error: musician does not exist");
                }

                string updateStatement = @"
                UPDATE dbo.Musician SET 
                DateOfBirth = @DateOfBirth, 
                Phone = @Phone, 
                Email = @Email,
                Instrument = @Instrument, 
                Address1 = @Address1, 
                Address2 = @Address2,
                City = @City, 
                StateCode = @StateCode, 
                ZipCode = @ZipCode
                WHERE MusicianID = @MusicianID
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
                            myCommand.Parameters.AddWithValue("@MusicianID", musician.musicianID);
                            myCommand.Parameters.AddWithValue("@DateOfBirth", musician.DateOfBirth);
                            myCommand.Parameters.AddWithValue("@Phone", musician.Phone);
                            myCommand.Parameters.AddWithValue("@Email", musician.Email);
                            if (string.IsNullOrEmpty(musician.Instrument))
                            {
                                myCommand.Parameters.AddWithValue("@Instrument", DBNull.Value);
                            }
                            else
                            {
                                myCommand.Parameters.AddWithValue("@Instrument", musician.Instrument);
                            }
                            myCommand.Parameters.AddWithValue("@Address1", musician.Address1);
                            if (string.IsNullOrEmpty(musician.Address2))
                            {
                                myCommand.Parameters.AddWithValue("@Address2", DBNull.Value);
                            }
                            else
                            {
                                myCommand.Parameters.AddWithValue("@Address2", musician.Address2);
                            }
                            myCommand.Parameters.AddWithValue("@City", musician.City);
                            myCommand.Parameters.AddWithValue("@StateCode", musician.StateCode);
                            myCommand.Parameters.AddWithValue("@ZipCode", musician.ZipCode);
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

            return new JsonResult("Information Updated Successfully");
        }

        /// <summary>
        /// Returns true if musician exists.
        /// </summary>
        /// <param name="musicianID"></param>
        /// <returns>True if musician  exists</returns>
        public bool MusicianExists(int musicianID)
        {
            if (musicianID <= 0)
            {
                throw new ArgumentException("Error: musician ID must be greater than 0");
            }

            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Musician
            WHERE MusicianID = @MusicianID
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@MusicianID", musicianID);
                        bool commentLikeExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return commentLikeExists;
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
