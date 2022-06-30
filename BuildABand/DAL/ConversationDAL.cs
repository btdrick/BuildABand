using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    public class ConversationDAL
    {
        private IConfiguration _configuration;

        public ConversationDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Conversation> GetConversationByMusicianID(int MusicianID)
        {
            List<Conversation> conversations = new List<Conversation>();
            string selectStatement = "SELECT * FROM Conversation " +
                "WHERE SenderID = @MusicianID OR ReceiverID = @MusicianID";

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
                            Conversation conversation = new Conversation()
                            {
                                ConversationID = (int)reader["ConversationID"],
                                CreatedTime = (DateTime)reader["CreatedTime"],
                                SenderID = (int)reader["SenderID"],
                                ReceiverID = (int)reader["ReceiverID"]

                            };
                            conversations.Add(conversation);

                        }

                    }
                }
            }
            return conversations;

        }

        public List<Conversation> GetConversationBySenderIDReceiverID(int SenderID, int ReceiverID)
        {
            List<Conversation> conversations = new List<Conversation>();
            string selectStatement = "SELECT * FROM Conversation " +
                "WHERE SenderID = @SenderID AND  ReceiverID = @ReceiverID ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@SenderID", SenderID);
                    selectCommand.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Conversation conversation = new Conversation()
                            {
                                ConversationID = (int)reader["ConversationID"],
                                CreatedTime = (DateTime)reader["CreatedTime"],
                                SenderID = (int)reader["SenderID"],
                                ReceiverID = (int)reader["ReceiverID"]

                            };
                            conversations.Add(conversation);

                        }

                    }
                }
            }
            return conversations;

        }

        public void AddConversation(int SenderID, int ReceiverID)
        {
            string insertStatement = "INSERT INTO Conversation " +
               "VALUES (@SenderID, @ReceiverID, @CreatedTime)";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                {

                    insertCommand.Parameters.AddWithValue("@SenderID", SenderID);
                    insertCommand.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                    insertCommand.Parameters.AddWithValue("@CreatedTime", DateTime.Now);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
