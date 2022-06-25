using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
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
        /// Gets all post likes.
        /// </summary>
        /// <returns>Table of post likes</returns>
        public JsonResult GetPostLikes()
        {
            try
            {
                string selectStatement =
                @"SELECT *
                FROM dbo.PostLike";

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
        /// Gets post like by ID.
        /// </summary>
        /// <param name="postLikeID"></param>
        /// <returns>Array for specified post like</returns>
        public JsonResult GetPostLikeByID(int postLikeID)
        {
            if (!this.PostLikeExists(postLikeID))
            {
                throw new ArgumentException("Error: post like doesn't exist");
            }

            try
            {
                string selectStatement =
            @"SELECT *
            FROM dbo.PostLike 
            WHERE PostLikeID = @PostLikeID";

                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@PostLikeID", postLikeID);
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
        /// Adds new PostLike row to the table
        /// </summary>
        /// <param name="newPostLike"></param>
        /// <returns>JsonResult with create status</returns>
        public JsonResult LikePost(PostLike newPostLike)
        {
            if (this.UserAlreadyLikedPost(newPostLike))
            {
                return new JsonResult("User has already liked this post");
            }

            try
            {
                string insertStatement =
                @"INSERT INTO dbo.PostLike " +
                "VALUES (@PostID, @MusicianID, @CreatedTime)";

                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@PostID", newPostLike.PostID);
                        myCommand.Parameters.AddWithValue("@MusicianID", newPostLike.MusicianID);
                        myCommand.Parameters.AddWithValue("@CreatedTime", newPostLike.CreatedTime);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult("Post Liked Successfully");
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Removes PostLike row from table
        /// </summary>
        /// <param name="postLikeID"></param>
        /// <returns>JsonResult with deletion status</returns>
        public JsonResult DeletePostLikeByID(int postLikeID)
        {
            if (!this.PostLikeExists(postLikeID))
            {
                throw new ArgumentException("Error: post like does not exist");
            }

            try
            {
                string deleteStatement =
                @"DELETE FROM dbo.PostLike " +
                "WHERE PostLikeID = @PostLikeID " +
                "DECLARE @lastID int " +
                "SELECT @lastID = MAX(PostLikeID) FROM dbo.PostLike " +
                "DBCC CHECKIDENT(PostLike, RESEED, @lastID)";
                DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (SqlCommand myCommand = new SqlCommand(deleteStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@PostLikeID", postLikeID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }

                return new JsonResult("Post Like Deleted Successfully");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
