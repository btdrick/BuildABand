using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.DAL
{
    public class MessageDAL
    {
        private IConfiguration _configuration;

        public MessageDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Message> GetMessageByMusicianID(int MusicianID)
        {
            List<Message> messages = new List<Message>();
            string selectStatement = "SELECT * FROM Message " +
                "WHERE SenderID = @MusicianID";

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
                            Message message = new Message()
                            {
                                MessageID = (int)reader["MessageID"],
                                CreatedTime = (DateTime)reader["createdTime"],
                                SenderID = (int)reader["SenderID"],
                                Text = reader["ReceiverID"].ToString()

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
            string insertStatement = "INSERT INTO Conversation " +
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
