using System;

namespace BuildABand.Models
{
    /// <summary>
    /// This class models a Like
    /// left on a Post.
    /// </summary>
    public class PostLike
    {
        public int PostLikeID { get; set; }
        public int PostID { get; set; }
        public int MusicianID { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}