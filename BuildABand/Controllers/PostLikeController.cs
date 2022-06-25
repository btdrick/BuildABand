using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to PostLike table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for PostLike media.
    /// </summary>
    [Route("api/post/like")]
    [ApiController]
    public class PostLikeController : ControllerBase
    {
        private PostLikeDAL postLikeDAL;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public PostLikeController(IConfiguration configuration)
        {
            this.postLikeDAL = new PostLikeDAL(configuration);
        }

        /// <summary>
        /// Gets all post likes
        /// GET: api/post/like
        /// </summary>
        /// <returns>JsonResult table of all post likes</returns>
        [HttpGet]
        public JsonResult GetPostLikes()
        {
            return this.postLikeDAL.GetPostLikes();
        }

        /// <summary>
        /// Gets the specified post like
        /// GET: api/post/like/PostLikeID
        /// </summary>
        /// <param name="postLikeID"></param>
        /// <returns>JsonResult table of post like</returns>
        [HttpGet("{PostLikeID}")]
        public JsonResult GetPostLikeByID(int postLikeID)
        {
            return this.postLikeDAL.GetPostLikeByID(postLikeID);
        }

        /// <summary>
        /// Submits a post like by the current user 
        /// POST: api/post/like
        /// </summary>
        /// <param name="newPostLike"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult LikePost(PostLike newPostLike)
        {
            return this.postLikeDAL.LikePost(newPostLike);   
        }

        /// <summary>
        /// Deletes specified like
        /// DELETE: api/post/like/PostLikeID
        /// </summary>
        /// <param name="postLikeID"></param>
        /// <returns>JsonResult if deleted successfully</returns>
        [HttpDelete("{PostLikeID}")]
        public JsonResult DeletePostLikeByID(int postLikeID)
        {
            return this.postLikeDAL.DeletePostLikeByID(postLikeID);
        }
    }
}
