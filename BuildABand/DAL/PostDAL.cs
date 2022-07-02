using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BuildABand.DAL
{
    /// <summary>
    /// Post table data access layer
    /// </summary>
    public class PostDAL
    {
        private IConfiguration _configuration;
        public PostDAL(IConfiguration configuration){
            _configuration = configuration;
        }

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>Table of posts</returns>
        public JsonResult GetAllPosts()
        {
            string selectStatement =
            @"SELECT *
            FROM dbo.Post
            JOIN dbo.Music ON dbo.Post.AudioID = dbo.Music.ID";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Get post likes 
        /// associated with postID
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>Table of post likes</returns>
        public JsonResult GetPostLikesByPostID(int postID)
        {
            if (!this.PostExists(postID))
            {
                throw new ArgumentException("Error: post does not exist");
            }

            string selectStatement = @"
            SELECT *
            FROM dbo.PostLike 
            WHERE PostID = @PostID
            ";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@PostID", postID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Adds new Post row to the table
        /// </summary>
        /// <param name="newPost"></param>
        /// <returns>JsonResult with create status</returns>
        public JsonResult CreatePost(Post newPost)
        {
            if (newPost is null)
            {
                throw new ArgumentException("Post cannot be null");
            }
            if (String.IsNullOrWhiteSpace(newPost.Content))
            {
                throw new ArgumentException("Post cannot be empty");
            }
            if (newPost.MusicianID < 1)
            {
                throw new ArgumentException("Invalid MusicianID");
            }
            if (newPost.AudioID < 1)
            {
                throw new ArgumentException("Invalid AudioID");
            }

            string insertStatement = @"
            INSERT INTO dbo.Post
            VALUES (@CreatedTime, @MusicianID, @Content, @AudioID)
            ";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(insertStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@CreatedTime", newPost.CreatedTime);
                        myCommand.Parameters.AddWithValue("@MusicianID", newPost.MusicianID);
                        myCommand.Parameters.AddWithValue("@Content", newPost.Content);
                        myCommand.Parameters.AddWithValue("@AudioID", newPost.AudioID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult("Post Added Successfully");
        }

        /// <summary>
        /// Gets comments associated
        /// with postID
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>Table of post's comments</returns>
        public JsonResult GetPostCommentsByPostID(int postID)
        {
            if (!this.PostExists(postID)) 
            {
                throw new ArgumentException("Error: post does not exist");
            }

            string selectStatement = @"
            SELECT *
            FROM dbo.Comment 
            WHERE PostID = @PostID
            ";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(selectStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@PostID", postID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult(resultsTable);
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

        /// <summary>
        /// Removes Post row from table,
        /// and its associated likes and comments.
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>JsonResult with deletion status</returns>
        public JsonResult DeletePostByID(int postID)
        {
            if (!this.PostExists(postID))
            {
                throw new ArgumentException("Error: post does not exist");
            }

            ///Statement deletes any row from Post, PostLike, Comment, CommentLike connected to @PostID
            string deleteStatement = @"
            DELETE FROM dbo.PostLike
            WHERE PostID = @PostID
            DECLARE @lastPostLikeID int
            SELECT @lastPostLikeID = MAX(PostLikeID) FROM dbo.PostLike
            IF @lastPostLikeID IS NULL
            DBCC CHECKIDENT(PostLike, RESEED, 0)
            ELSE
            DBCC CHECKIDENT(PostLike, RESEED, @lastPostLikeID)

            DELETE FROM dbo.CommentLike
            WHERE CommentID IN (SELECT CommentID FROM Comment WHERE PostID = @PostID)
            DECLARE @lastCommentLikeID int
            SELECT @lastCommentLikeID = MAX(CommentLikeID) FROM dbo.CommentLike
            IF @lastCommentLikeID IS NULL
            DBCC CHECKIDENT(CommentLike, RESEED, 0)
            ELSE 
            DBCC CHECKIDENT(CommentLike, RESEED, @lastCommentLikeID)

            DELETE FROM dbo.Comment
            WHERE PostID = @PostID
            DECLARE @lastCommentID int
            SELECT @lastCommentID = MAX(CommentID) FROM dbo.Comment
            IF @lastCommentID IS NULL 
            DBCC CHECKIDENT(Comment, RESEED, 0)
            ELSE
            DBCC CHECKIDENT(Comment, RESEED, @lastCommentID)

            DELETE FROM dbo.Post
            WHERE PostID = @PostID
            DECLARE @lastPostID int
            SELECT @lastPostID = MAX(PostID) FROM dbo.Post
            IF @lastPostID IS NULL
            DBCC CHECKIDENT(Post, RESEED, 0)
            ELSE
            DBCC CHECKIDENT(Post, RESEED, @lastPostID)
            ";

            DataTable resultsTable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
                SqlDataReader dataReader;
                using (SqlConnection connection = new SqlConnection(sqlDataSource))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        using (SqlCommand myCommand = new SqlCommand(deleteStatement, connection, transaction))
                        {
                            myCommand.Parameters.AddWithValue("@PostID", postID);
                            dataReader = myCommand.ExecuteReader();
                            resultsTable.Load(dataReader);
                            dataReader.Close();
                            transaction.Commit();
                            connection.Close();
                        }
                }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
            }

            return new JsonResult("Post Deleted Successfully");       
        }

        /// <summary>
        /// Returns true if post exists.
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>True if post exists</returns>
        public bool PostExists(int postID)
        {
            if (postID <= 0)
            {
                throw new ArgumentException("Error: post ID must be greater than 0");
            }

            
            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Post
            WHERE PostID = @PostID
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@PostID", postID);
                        bool postExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return postExists;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }
    }
}