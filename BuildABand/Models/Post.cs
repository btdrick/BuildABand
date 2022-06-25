using System;

namespace BuildABand.Models
{
    /// <summary>
    /// This class models a 
    /// user-created post for a feed.
    /// </summary>
    public class Post
    {
        public int PostID { get; set; }
        public DateTime CreatedTime { get; set; }
        public int MusicianID { get; set; }
        public string Content { get; set; }
    }
}