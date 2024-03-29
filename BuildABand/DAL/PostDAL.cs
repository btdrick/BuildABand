using System;
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
            string selectStatement = @"
            SELECT *
            FROM dbo.Post
            LEFT JOIN dbo.Music ON dbo.Post.AudioID = dbo.Music.ID
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
        /// Gets all posts.
        /// </summary>
        /// <returns>Table of posts</returns>
        public JsonResult GetAllPostsFromActiveAccounts()
        {
            string selectStatement = @"
            SELECT *
            FROM dbo.Post
            LEFT JOIN dbo.Music 
            ON dbo.Post.AudioID = dbo.Music.ID
            WHERE dbo.Post.MusicianID IN
            (SELECT m.MusicianID 
            FROM Musician m
            JOIN Accounts a
            ON m.AccountID = a.AccountID
            WHERE a.is_Active = 1)
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
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return new JsonResult(resultsTable);
        }

        /// <summary>
        /// Gets post by musicianID
        /// </summary>
        /// <param name="musicianID"></param>
        /// <returns>JsonResult of posts made by musician</returns>
        public JsonResult GetPostByMusicianID(int musicianID)
        {
            if (!this.MusicianExists(musicianID))
            {
                throw new ArgumentException("Error: musician does not exist");
            }

            string selectStatement = @"
            SELECT *
            FROM dbo.Post 
            LEFT JOIN dbo.Music ON dbo.Post.AudioID = dbo.Music.ID
            WHERE dbo.Post.musicianID = @MusicianID
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
                        myCommand.Parameters.AddWithValue("@MusicianID", musicianID);
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
            WHERE PostID = @PostID ";

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
            if (newPost.AudioID < 0)
            {
                throw new ArgumentException("Invalid AudioID");
            }

            string insertStatement = @"
            INSERT INTO dbo.Post
            VALUES (@CreatedTime, @MusicianID, @Content, @AudioID)
            ";

            if(newPost.AudioID == 0) 
            {
                insertStatement = @"
                    INSERT INTO dbo.Post
                    VALUES (@CreatedTime, @MusicianID, @Content, null)
                    ";
            }

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
                        if (newPost.AudioID == 0)
                        {
                            myCommand.Parameters.AddWithValue("@AudioID", DBNull.Value);
                        }
                        else
                        {
                            myCommand.Parameters.AddWithValue("@AudioID", newPost.AudioID);
                        }
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

        /// <summary>
        /// Returns true if musician exists.
        /// </summary>
        /// <param name="musicianID"></param>
        /// <returns>True if musician  exists</returns>
        public bool MusicianExists(int musicianID)
        {
            if (musicianID <= 0)
            {
                throw new ArgumentException("Error: musician ID must be greater than 0");
            }

            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Musician
            WHERE MusicianID = @MusicianID
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@MusicianID", musicianID);
                        bool commentLikeExists = Convert.ToBoolean(selectCommand.ExecuteScalar());

                        return commentLikeExists;
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