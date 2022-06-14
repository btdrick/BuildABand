using System;

namespace BuildABand.Models
{
    /// <summary>
    /// This class models a 
    /// comment for a post.
    /// </summary>
    public class Comment
    {
        public int CommentID { get; set; }
        public DateTime CreatedTime { get; set; }
        public int MusicianID { get; set; }
        public int PostID { get; set; }
        public string Content { get; set; }
    }
}
