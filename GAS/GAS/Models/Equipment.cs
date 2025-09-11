using System;

namespace GAS.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public string House { get; set; } = "";
        public string Index { get; set; } = "";
        public string Corp { get; set; } = "";
        public string Entrance { get; set; } = "";
        public string Antenna { get; set; } = "";
        public string DRS { get; set; } = "";
        public string Note { get; set; } = "";
        public bool Serviced { get; set; } = true;
        public DateTime? InstallationDate { get; set; }
        public DateTime? RevisionDate { get; set; }
    }
}

