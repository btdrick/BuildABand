using BuildABand.DAL;
using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Post table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for Post media.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostDAL postDAL;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 1-param constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.postDAL = new PostDAL(_configuration);
        }

        /// <summary>
        /// Gets all post
        /// GET: api/post
        /// </summary>
        /// <returns>JsonResult table of all posts</returns>
        [HttpGet]
        public JsonResult GetAllPosts()
        {
            return this.postDAL.GetAllPosts();
        }

        /// <summary>
        /// Gets all posts from active accounts
        /// GET: api/post/active_accounts
        /// </summary>
        /// <returns>JsonResult table of all posts</returns>
        [HttpGet("active_accounts")]
        public JsonResult GetAllPostsFromActiveAccounts()
        {
            return this.postDAL.GetAllPostsFromActiveAccounts();
        }

        /// <summary>
        /// Gets all posts for specified users
        /// GET: api/post/UserID
        /// </summary>
        /// <param name="musicianID"></param>
        /// <returns>JsonResult table of user's posts</returns>
        [HttpGet("{musicianID}")]
        public JsonResult GetPostsByMusicianID(int musicianID)
        {
            return this.postDAL.GetPostByMusicianID(musicianID);
        }

        /// <summary>
        /// Gets all post likes for specified post
        /// GET: api/post/PostID/like
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>JsonResult table of post likes</returns>
        [HttpGet("{PostID}/like")]
        public JsonResult GetPostLikesByPostID(int postID)
        {
            return this.postDAL.GetPostLikesByPostID(postID);
        }

        /// <summary>
        /// Submits a post for the current user 
        /// POST: api/post
        /// </summary>
        /// <param name="newPost"></param>
        /// <returns>JsonResult if added successfully</returns>
        [HttpPost]
        public JsonResult CreatePost(Post newPost)
        {
            return this.postDAL.CreatePost(newPost);
        }

        /// <summary>
        /// Gets all comments for specified post
        /// GET: api/post/postID/comments
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>JsonResult table of post's comments</returns>
        [HttpGet("{postID}/comments")]
        public JsonResult GetPostCommentsByPostID(int postID)
        {
            return this.postDAL.GetPostCommentsByPostID(postID);
        }

        /// <summary>
        /// Deletes specified post
        /// DELETE: api/post/PostID
        /// </summary>
        /// <param name="postID"></param>
        /// <returns>JsonResult if deleted successfully</returns>
        [HttpDelete("{PostID}")]
        public JsonResult DeletePostByID(int postID)
        {
            return this.postDAL.DeletePostByID(postID);
        }
    }
}