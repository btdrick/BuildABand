using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    public class StatesDAL
    {
        private IConfiguration _configuration;

        public StatesDAL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public List<State> GetStates()
        {
            List<State> states = new List<State>();
            string selectStatement = "Select * from State";

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
