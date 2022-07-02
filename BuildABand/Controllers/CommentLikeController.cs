using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to CommentLike table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for CommentLike media.
    /// </summary>
    [Route("api/comment/like")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private CommentLikeDAL commentLikeDAL;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public CommentLikeController(IConfiguration configuration)
        {
            this.commentLikeDAL = new CommentLikeDAL(configuration);
        }

        /// <summary>
        /// Gets all comment likes
        /// GET: api/comment/like
        /// </summary>
        /// <returns>JsonResult table of all comment likes</returns>
        [HttpGet]
        public JsonResult GetCommentLikes()
        {
            return this.commentLikeDAL.GetCommentLikes();
        }

        /// <summary>
        /// Gets the specified like
        /// GET: api/comment/like/CommentLikeID
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns>JsonResult table of comment</returns>
        [HttpGet("{CommentLikeID}")]
        public JsonResult GetCommentLikeByID(int commentLikeID)
        {
            return this.commentLikeDAL.GetCommentLikeByID(commentLikeID);
        }

        /// <summary>
        /// Submits a comment like by the current user 
        /// POST: api/comment/like
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult LikeComment(CommentLike newCommentLike)
        {
            return this.commentLikeDAL.LikeComment(newCommentLike);
        }

        /// <summary>
        /// Deletes specified comment like
        /// DELETE: api/comment/like/CommentLikeID
        /// </summary>
        /// <param name="commentLikeID"></param>
        /// <returns>JsonResult if deleted successfully</returns>
        [HttpDelete("{CommentLikeID}")]
        public JsonResult DeleteCommentLikeByID(int commentLikeID)
        {
            return this.commentLikeDAL.DeleteCommentLikeByID(commentLikeID);
        }
    }
}
