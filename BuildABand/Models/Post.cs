using System;

namespace BuildABand.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public Byte[] CreatedTime { get; set; }
        public int MusicianID { get; set; }
        public string Content { get; set; }
    }
}