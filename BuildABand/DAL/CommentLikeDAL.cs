using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
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
        /// Gets all comment likes.
        /// </summary>
        /// <returns>Table of comment likes</returns>
        public JsonResult GetCommentLikes()
        {
            try
            {
                string selectStatement =
                @"SELECT *
                FROM dbo.CommentLike";

                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult(resultsTable);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get comment like by ID.
        /// </summary>
        /// <param name="commentLikeID"></param>
        /// <returns>Array for specified comment like</returns>
        public JsonResult GetCommentLikeByID(int commentLikeID)
        {
            if (!this.CommentLikeExists(commentLikeID))
            {
                throw new ArgumentException("Error: comment like does not exist");
            }

            try
            {
                string selectStatement =
                @"SELECT *
                FROM dbo.CommentLike 
                WHERE CommentLikeID = @CommentLikeID";

                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@CommentLikeID", commentLikeID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult(resultsTable);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Adds new CommentLike row to the table
        /// </summary>
        /// <param name="newCommentLike"></param>
        /// <returns>JsonResult with create status</returns>
        /// <returns></returns>
        public JsonResult LikeComment(CommentLike newCommentLike)
        {
            if (this.UserAlreadyLikedComment(newCommentLike))
            {
                return new JsonResult("User has already liked this comment");
            }

            try
            {
                string insertStatement =
                @"INSERT INTO dbo.CommentLike " +
                "VALUES (@CommentID, @MusicianID, @CreatedTime)";

                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@CommentID", newCommentLike.CommentID);
                        myCommand.Parameters.AddWithValue("@MusicianID", newCommentLike.MusicianID);
                        myCommand.Parameters.AddWithValue("@CreatedTime", newCommentLike.CreatedTime);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult("Comment Liked Successfully");
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Removes CommentLike row from table.
        /// </summary>
        /// <param name="commentLikeID"></param>
        /// <returns>JsonResult with deletion status</returns>
        public JsonResult DeleteCommentLikeByID(int commentLikeID)
        {
            if (!this.CommentLikeExists(commentLikeID))
            {
                throw new ArgumentException("Error: comment like does not exist");
            }

            try
            {
                string deleteStatement =
                @"DELETE FROM dbo.CommentLike " +
                "WHERE CommentLikeID = @CommentLikeID " +
                "DECLARE @lastID int " +
                "SELECT @lastID = MAX(CommentLikeID) FROM dbo.CommentLike " +
                "DBCC CHECKIDENT(CommentLike, RESEED, @lastID)";
                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(deleteStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@CommentLikeID", commentLikeID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult("Comment Deleted Successfully");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
