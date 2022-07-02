using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Comment table in DB.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private CommentDAL commentDAL;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CommentController(IConfiguration configuration)
        {
            this.commentDAL = new CommentDAL(configuration);
        }

        /// <summary>
        /// Gets all comments
        /// GET: api/comment
        /// </summary>
        /// <returns>JsonResult table of all comments</returns>
        [HttpGet]
        public JsonResult GetComments()
        {
            return this.commentDAL.GetComments();
        }

        /// <summary>
        /// Gets specified comment
        /// GET: api/comment/comment.CommentID
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult table of comment</returns>
        [HttpGet("{commentID}")]
        public JsonResult GetCommentByID(int commentID)
        {
            return this.commentDAL.GetCommentByID(commentID);
        }

        /// <summary>
        /// Gets all comment likes for specified post
        /// GET: api/comment/CommentID/like
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns>JsonResult table of comment likes</returns>
        [HttpGet("{CommentID}/like")]
        public JsonResult GetCommentLikes(int commentID)
        {
            return this.commentDAL.GetCommentLikes(commentID);
        }

        /// <summary>
        /// Submits a comment for the current user 
        /// POST: api/comment
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult CreateComment(Comment newComment)
        {
            return this.commentDAL.CreateComment(newComment);
        }

        /// <summary>
        /// Updates a comment
        /// POST: api/comment/comment.CommentID
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult if updated successfully</returns>
        [HttpPatch("{comment.CommentID}")]
        public JsonResult UpdateComment(Comment comment)
        {
            return this.commentDAL.UpdateComment(comment);
        }

        /// <summary>
        /// Deletes a comment  
        /// DELETE: api/comment/comment.CommentID
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>JsonResult if deleted successfully</returns>
        [HttpDelete("{comment.CommentID}")]
        public JsonResult DeleteComment(Comment comment)
        {
            return this.commentDAL.DeleteComment(comment);
        }
    }
}
