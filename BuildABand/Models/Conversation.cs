using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    public class Conversation
    {
        public int ConversationID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public DateTime CreatedTime { get; set; }

        
    }
}
