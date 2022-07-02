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
    public class CommentDAL
    {
        private IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CommentDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>Table of comments</returns>
        public JsonResult GetComments()
        {
            string selectStatement = @"
            SELECT *
            FROM dbo.Comment
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
        /// Get comment by ID.
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns>Array for specified comment</returns>
        public JsonResult GetCommentByID(int commentID)
        {
            if (!this.CommentExists(commentID))
            {
                throw new ArgumentException("Error: comment does not exist");
            }

            string selectStatement = @"
            SELECT *
            FROM dbo.Comment 
            WHERE CommentID = @CommentID
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
                        myCommand.Parameters.AddWithValue("@CommentID", commentID);
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
        /// Get comment likes 
        /// associated with commentID
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns>Table of comment likes</returns>
        public JsonResult GetCommentLikes(int commentID)
        {
            if (!this.CommentExists(commentID))
            {
                throw new ArgumentException("Error: comment does not exist");
            }

            string selectStatement =
            @"SELECT *
            FROM dbo.CommentLike 
            WHERE CommentID = @CommentID";

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
                        myCommand.Parameters.AddWithValue("@CommentID", commentID);
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
        /// Adds new Comment row to the table
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns>JsonResult with create status</returns>
        public JsonResult CreateComment(Comment newComment)
        {
            if (newComment is null)
            {
                throw new ArgumentException("Comment cannot be null");
            }
            if (String.IsNullOrWhiteSpace(newComment.Content))
            {
                throw new ArgumentException("Comment cannot be empty");
            }
            if (newComment.MusicianID < 1)
            {
                throw new ArgumentException("Invalid MusicianID");
            }
            if (newComment.PostID < 1)
            {
                throw new ArgumentException("Invalid PostID");
            }

            string insertStatement = @"
            INSERT INTO dbo.Comment
            VALUES (@CreatedTime, @ParentID, @MusicianID, @PostID, @Content)
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
                        myCommand.Parameters.AddWithValue("@CreatedTime", newComment.CreatedTime);
                        if (newComment.ParentID == 0)
                        {
                            myCommand.Parameters.AddWithValue("@ParentID", DBNull.Value);
                        }
                        if (newComment.ParentID > 0)
                        {
                            myCommand.Parameters.AddWithValue("@ParentID", newComment.ParentID);
                        }
                        myCommand.Parameters.AddWithValue("@MusicianID", newComment.MusicianID);
                        myCommand.Parameters.AddWithValue("@PostID", newComment.PostID);
                        myCommand.Parameters.AddWithValue("@Content", newComment.Content);
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

            return new JsonResult("Comment Added Successfully");
        }

        /// <summary>
        /// Updates comment row in table
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult with update status</returns>
        public JsonResult UpdateComment(Comment comment)
        {
            if (comment is null)
            {
                throw new ArgumentException("Error: comment cannot be null");
            }
            if (!this.CommentExists(comment.CommentID))
            {
                throw new ArgumentException("Error: comment does not exist");
            }

            string updateStatement = @"
            UPDATE dbo.Comment 
            SET Content = @Content
            WHERE CommentID = @CommentID
            ";

            DataTable resultsTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BuildABandAppCon");
            SqlDataReader dataReader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(updateStatement, connection))
                    {
                        myCommand.Parameters.AddWithValue("@CommentID", comment.CommentID);
                        myCommand.Parameters.AddWithValue("@Content", comment.Content);
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

            return new JsonResult("Comment Updated Successfully");
        }

        /// <summary>
        /// Deletes a comment and 
        /// its affiliated likes.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult with deletion status</returns>
        public JsonResult DeleteComment(Comment comment)
        {
            if (comment is null)
            {
                throw new ArgumentException("Comment cannot be null");
            }

            string deleteStatement = @"
            DELETE FROM dbo.CommentLike
            WHERE CommentID = @CommentID
            DECLARE @lastCommentLikeID int
            SELECT @lastCommentLikeID = MAX(CommentLikeID) FROM dbo.CommentLike
            IF @lastCommentLikeID IS NULL
            DBCC CHECKIDENT(CommentLike, RESEED, 0)
            ELSE 
            DBCC CHECKIDENT(CommentLike, RESEED, @lastCommentLikeID)
            
            DELETE FROM dbo.Comment 
            WHERE CommentID = @CommentID
            DECLARE @lastCommentID int
            SELECT @lastCommentID = MAX(CommentID) FROM dbo.Comment
            IF @lastCommentID IS NULL 
            DBCC CHECKIDENT(Comment, RESEED, 0)
            ELSE
            DBCC CHECKIDENT(Comment, RESEED, @lastCommentID)
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
                        myCommand.Parameters.AddWithValue("@CommentID", comment.CommentID);
                        dataReader = myCommand.ExecuteReader();
                        resultsTable.Load(dataReader);
                        dataReader.Close();
                        transaction.Commit();
                        connection.Close();
                    }
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
                
            }

            return new JsonResult("Comment Deleted Successfully");
        }

        /// <summary>
        /// Returns true if comment exists.
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns>True if comment  exists</returns>
        public bool CommentExists(int commentID)
        {
            if (commentID <= 0)
            {
                throw new ArgumentException("Error: comment ID must be greater than 0");
            }
            
            string selectStatement = @"
            SELECT COUNT(*)
            FROM dbo.Comment
            WHERE CommentID = @CommentID
            ";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("BuildABandAppCon")))
            {
                connection.Open();
                try
                {
                    using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@CommentID", commentID);
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
