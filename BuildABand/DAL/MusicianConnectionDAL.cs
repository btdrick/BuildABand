using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    /// <summary>
    /// This class serves as the Data Access Layer
    /// for DB States table.
    /// </summary>
    public class MusicianConnectionDAL
    {
        private readonly IConfiguration _configuration;
       

        /// <summary>
        /// Constructor to initialize configuration variable
        /// </summary>
        /// <param name="configuration"></param>
        public MusicianConnectionDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Return Musician connections
        /// </summary>
        /// <param name="MusicianID"></param>
        /// <returns></returns>
        public List<MusicianConnection> GetMusicianConnectionsByID(int MusicianID)
        {
            List<MusicianConnection> musicianConnections = new List<MusicianConnection>();
            string selectStatement = @"
            SELECT ConnectionID, InitiatorID,
            CONCAT(A.Fname,' ',A.Lname) as InitiatorNames,
            FollowerID, CONCAT(B.Fname, ' ', B.Lname) as FollowerNames,
            CreatedTime, Connected
            FROM Connection C
            JOIN Musician A on C.InitiatorID = A.AccountID
            JOIN Musician B on C.FollowerID = B.AccountID
            WHERE (C.InitiatorID = @MusicianID OR C.FollowerID = @MusicianID) 
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("MusicianID", MusicianID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MusicianConnection musicianConnection = new MusicianConnection()
                            {
                                ConnectionID = (int)reader["ConnectionID"],
                                InitiatorID = (int)reader["InitiatorID"],
                                InitiatorNames = reader["InitiatorNames"].ToString(),
                                FollowerID = (int)reader["FollowerID"],
                                FollowerNames = reader["FollowerNames"].ToString(),
                                CreatedTime = (DateTime)reader["createdTime"],
                                Connected = Convert.ToBoolean(Convert.ToInt32(reader["Connected"]))
                            };
                            musicianConnections.Add(musicianConnection);
                        }
                    }
                }

            }
            return musicianConnections;
        }

        /// <summary>
        /// Removes user from connections table. This can also be used to disconnect a current connection.
        /// </summary>
        /// <param name="connectionRequestID"></param>
        public void RejectConnectionRequest(int connectionRequestID)
        {
            string deleteStatement = "DELETE FROM Connection " +
               "WHERE ConnectionID = @ConnectionID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(deleteStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ConnectionID", connectionRequestID);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Changes connection status
        /// </summary>
        /// <param name="connectionRequestID"></param>
        public void AcceptConnectionRequest(int connectionRequestID)
        {
            string updateStatement = "UPDATE Connection " +
                "SET connected = 1 " +
                "WHERE ConnectionID = @ConnectionID";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(updateStatement, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ConnectionID", connectionRequestID);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Adds pending connection to connections table
        /// </summary>
        /// <param name="fromMusicianID"></param>
        /// <param name="toMusicianID"></param>
        public void SendConnectionRequest(int fromMusicianID, int toMusicianID)
        {
            string insertStatement = "INSERT INTO Connection " +
                "VALUEs (@InitiatorID, @FollowerID, @CreatedTime, 0)";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                {
                   
                    insertCommand.Parameters.AddWithValue("@InitiatorID", fromMusicianID);
                    insertCommand.Parameters.AddWithValue("@FollowerID", toMusicianID);
                    insertCommand.Parameters.AddWithValue("@CreatedTime", DateTime.Now);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
