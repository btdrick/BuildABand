using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    public class MessageDAL
    {
        private IConfiguration _configuration;

        public MessageDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Message> GetMessageByConversationID(int ConversationID)
        {
            List<Message> messages = new List<Message>();
            string selectStatement = "SELECT * FROM Message " +
                "WHERE ConversationID = @ConversationID";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@ConversationID", ConversationID);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Message message = new Message()
                            {
                                MessageID = (int)reader["MessageID"],
                                ConversationID = (int)reader["ConversationID"],
                                CreatedTime = (DateTime)reader["CreatedTime"],
                                SenderID = (int)reader["SenderID"],
                                Text = Crypto.Decrypt(_configuration["MessageKey"],
                                    reader["Text"].ToString())

                            };
                            messages.Add(message);

                        }

                    }
                }
            }
            return messages;

        }


        public void AddMessage(Message message)
        {
            string insertStatement = "INSERT INTO Message " +
               "VALUEs (@ConversationID, @CreatedTime, @SenderID, @Text)";
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertStatement, connection))
                {

                    insertCommand.Parameters.AddWithValue("@SenderID", message.SenderID);
                    insertCommand.Parameters.AddWithValue("@ConversationID", message.ConversationID);
                    insertCommand.Parameters.AddWithValue("@Text", message.Text);
                    insertCommand.Parameters.AddWithValue("@CreatedTime", DateTime.Now);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
