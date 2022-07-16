using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public string OwnerNames { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public List<Musician> Collaborators { get; set; }
        public int MusicID { get; set; }
        public string FileName { get; set; }
        public String  AzureFileName { get; set; }

    }
}
