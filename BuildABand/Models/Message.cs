using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    public class Message
    {
        public int MessageID  {get; set;}
        public DateTime CreatedTime { get; set; }
        public int SenderID { get; set; }
        public string Text { get; set; }

    }
}
