using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BuildABand.Models;
using Microsoft.Extensions.Configuration;

namespace BuildABand.DAL
{
    /// <summary>
    /// Post table data access layer
    /// </summary>
    public class PostDBDAL
    {
        private IConfiguration _configuration;
        public PostDBDAL(IConfiguration configuration){
            _configuration = configuration;
        }

        public List<Post> GetPostByMusicianID(int musicianID)
        {
            List<Post> posts = new List<Post>();
            string selectStatement = 
                @"SELECT *
                FROM dbo.Post 
                WHERE musicianID = @id";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                {
                    selectCommand.Parameters.AddWithValue("@id", musicianID);
                     using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }

                        while (reader.Read())
                        {
                            Post post = new Post
                            {
                                PostID = (int)reader["postID"],
                                CreatedTime = (DateTime)reader["createdTime"],
                                MusicianID = (int)reader["musicianID"],
                                Content = reader["content"].ToString(),
                            };

                        posts.Add(post);
                        }
                    }
                    connection.Close();
                }
            }
            return posts;
        }
    }
}