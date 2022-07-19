using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    /// <summary>
    /// Project_Workon table data access layer (DAL).
    /// </summary>
    public class ProjectCollaboratorDAL
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor
        /// </summary>
        /// <param name="configuration"></param>
        public ProjectCollaboratorDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Gets all project collaborators.
        /// </summary>
        /// <returns>Table of project collaborations</returns>
        public JsonResult GetAllProjectCollaborations()
        {
            string selectStatement = @"
            SELECT *
            FROM dbo.Project_Workon
            ";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
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

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Get all collaborations
        /// associated with musicianID
        /// </summary>
        /// <param name="musicianID"></param>
        /// <returns>JsonResult of musician's collaborations</returns>
        public JsonResult GetAllProjectCollaborationsByMusicianID(int musicianID)
        {
            if (!this.MusicianExists(musicianID))
            {
                throw new ArgumentException("Musician does not exist");
            }

            string selectStatement = @"
            SELECT *
            FROM dbo.Project_Workon
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
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@MusicianID", musicianID);
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

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Returns true if musician exists.
        /// </summary>
        /// <param name="musicianID"></param>
        /// <returns>True if musician exists</returns>
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
                        bool projectExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return projectExists;
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
