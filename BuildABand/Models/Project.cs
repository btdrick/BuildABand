using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set }
        public int OwnerID { get; set }
        public int OwnerNames { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Musician> Collaborators { get; set; }
        public List<int> Audios { get; set; }

    }
}
