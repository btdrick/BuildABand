using System;

namespace BuildABand.Models
{
    /// <summary>
    /// This class models a Like
    /// left on a Comment.
    /// </summary>
    public class CommentLike
    {
        public int CommentLikeID { get; set; }
        public int CommentID { get; set; }
        public int MusicianID { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
