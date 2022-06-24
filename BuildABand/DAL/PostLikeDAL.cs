using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    /// <summary>
    /// PostLike table data access layer (DAL).
    /// </summary>
    public class PostLikeDAL
    {
        private IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public PostLikeDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns true if user liked specified post.
        /// </summary>
        /// <param name="postLike"></param>
        /// <returns>True if user liked specified post.</returns>
        public bool UserAlreadyLikedPost(PostLike postLike)
        {
            if (postLike is null)
            {
                throw new ArgumentException("Post Like cannot be null");
            }

            try
            {
                string selectStatement =
                    "SELECT COUNT(*) " +
                    "FROM dbo.PostLike " +
                    "WHERE PostID = @PostID " +
                    "AND MusicianID = @MusicianID";

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
                {
                    connection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@PostID", postLike.PostID);
                        selectCommand.Parameters.AddWithValue("@MusicianID", postLike.MusicianID);
                        bool isLiked = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return isLiked;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Returns true if post like exists.
        /// </summary>
        /// <param name="postLikeID"></param>
        /// <returns>True if post like exists</returns>
        public bool PostLikeExists(int postLikeID)
        {
            if (postLikeID <= 0)
            {
                throw new ArgumentException("Error: post like ID must be greater than 0");
            }

            try
            {
                string selectStatement =
                    "SELECT COUNT(*) " +
                    "FROM dbo.PostLike " +
                    "WHERE PostLikeID = @PostLikeID";

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
                {
                    connection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@PostLikeID", postLikeID);
                        bool postLikeExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return postLikeExists;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
