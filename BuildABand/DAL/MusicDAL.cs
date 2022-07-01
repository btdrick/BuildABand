using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    public class MusicDAL
    {
        private readonly IConfiguration _configuration;

        public MusicDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void addUserFileNameToAzureFileNameMapping(Guid guid, string fileName, int musicianID)
        {
            string insertStatement =
            @"INSERT INTO Music";

            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@guid", guid);
                    myCommand.Parameters.AddWithValue("@fileName", fileName);
                    myCommand.Parameters.AddWithValue("@musicianID", musicianID);

                    dataReader = myCommand.ExecuteReader();
                    //results.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }
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
                using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                {
                    myCommand.Parameters.AddWithValue("@id", musicianID);

                    dataReader = myCommand.ExecuteReader();
                    results.Load(dataReader);
                    dataReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(results);
        }
    }
}
