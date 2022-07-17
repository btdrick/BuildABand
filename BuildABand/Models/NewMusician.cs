using System;

namespace BuildABand.Models
{
    public class NewMusician
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public int MusicianID { get; set; }
        public string Fname {get; set;}
        public string Lname { get; set; }
        public DateTime DateOfbirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Instrument { get; set; }
        public string Sex { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StateCode { get; set; }
        public string AvaterFilename { get; set; }

    }
}
