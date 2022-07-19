using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    /// <summary>
    /// This class serves as the Data Access Layer
    /// for DB States table.
    /// </summary>
    public class StatesDAL
    {
        private IConfiguration _configuration;

        /// <summary>
        /// Constructor to initialize configuration variable
        /// </summary>
        /// <param name="configuration"></param>
        public StatesDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Get list of all States.
        /// </summary>
        /// <returns></returns>
        public List<State> GetStates()
        {
            List<State> states = new List<State>();
            string selectStatement = "Select * from States";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            State state = new State();
                            state.StateCode = reader["StateCode"].ToString();
                            state.StateName = reader["StateName"].ToString();
                            states.Add(state);
                        }
                    }
                }

            }
            return states;
        }
    }
}
