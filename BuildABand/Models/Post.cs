using System;

namespace BuildABand.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public DateTime CreatedTime { get; set; }
        public int MusicianID { get; set; }
        public string Content { get; set; }
    }
}