using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    /// <summary>
    /// This class models a state object.
    /// For status property 0 represent pending and 1 accept
    /// </summary>
    public class ProjectInvite
    {
        public int ProjectInviteID { get; set; }
        public int ProjectID { get; set; }
        public int InviterID { get; set; }
        public string InviterNames { get; set; }
        public int InviteeID { get; set; }
        public string InviteeNames { get; set; }
        public int Status { get; set; }

    }
}
