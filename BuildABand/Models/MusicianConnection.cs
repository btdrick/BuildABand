using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    /// <summary>
    /// This class models a state object.
    /// </summary>
    public class MusicianConnection
    {
        /// <summary>
        /// Musician connection attributes.
        /// </summary>
        public int ConnectionID { get; set; }
        public int InitiatorID { get; set; }
        public string InitiatorNames { get; set; }
        public int FollowerID { get; set; }
        public string FollowerNames { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Connected { get; set; }

    }
}
