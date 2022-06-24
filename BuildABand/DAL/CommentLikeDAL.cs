using BuildABand.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace BuildABand.DAL
{
    /// <summary>
    /// CommentLike table data access layer (DAL).
    /// </summary>
    public class CommentLikeDAL
    {
        private IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CommentLikeDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns true if user liked specified comment.
        /// </summary>
        /// <param name="commentLike"></param>
        /// <returns>True if user liked specified comment.</returns>
        public bool UserAlreadyLikedComment(CommentLike commentLike)
        {
            if (commentLike is null)
            {
                throw new ArgumentException("Comment Like cannot be null");
            }

            try
            {
                string selectStatement = 
                    "SELECT COUNT(*) " +
                    "FROM dbo.CommentLike " +
                    "WHERE CommentID = @CommentID " +
                    "AND MusicianID = @MusicianID";

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
                {
                    connection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@CommentID", commentLike.CommentID);
                        selectCommand.Parameters.AddWithValue("@MusicianID", commentLike.MusicianID);
                        bool isLiked = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return isLiked;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }         
        }

        /// <summary>
        /// Returns true if comment like exists.
        /// </summary>
        /// <param name="commentLikeID"></param>
        /// <returns>True if comment like exists</returns>
        public bool CommentLikeExists(int commentLikeID)
        {
            if (commentLikeID <= 0)
            {
                throw new ArgumentException("Error: comment like ID must be greater than 0");
            }

            try
            {
                string selectStatement =
                    "SELECT COUNT(*) " +
                    "FROM dbo.CommentLike " +
                    "WHERE CommentLikeID = @CommentLikeID";

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
                {
                    connection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@CommentLikeID", commentLikeID);
                        bool commentLikeExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return commentLikeExists;
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
