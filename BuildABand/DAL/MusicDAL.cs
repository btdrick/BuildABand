using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    public class MusicDAL
    {
        private readonly IConfiguration _configuration;

        public MusicDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int addUserFileNameToAzureFileNameMapping(Guid guid, string fileName, int musicianID)
        {
            int audioID = 0;
            string insertStatement =
            @"INSERT INTO dbo.Music VALUES (@guid, @fileName, @musicianID)";

            string selectStatemet = @"SELECT ID FROM dbo.Music WHERE azure_file_name = @guid";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@guid", guid);
                        myCommand.Parameters.AddWithValue("@fileName", fileName);
                        myCommand.Parameters.AddWithValue("@musicianID", musicianID);

                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                    }

                    using (SqlCommand myCommand = new SqlCommand(selectStatemet, connection))
                    {
                        myCommand.Parameters.AddWithValue("@guid", guid);

                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        audioID = (int)myCommand.ExecuteScalar();
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return audioID;
        }

        public JsonResult getFileInfo(int musicianID)
        {
            string selectStatement =
            @"SELECT *
            FROM dbo.Music 
            WHERE musicianID = @id";

            var results = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@id", musicianID);

                        dataReader = myCommand.ExecuteReader();
                        results.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return new JsonResult(results);
        }
    }
}
