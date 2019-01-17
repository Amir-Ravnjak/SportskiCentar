using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Rezervacija
    {
        public int id { get; set; }
        public bool Odobrena { get; set; }
        public int TerminID { get; set; }
        public Termin Termin { get; set; }
        public int? UplataID { get; set; }
        public Uplata Uplata { get; set; }
        public int KlijentID { get; set; }
        public Klijent Klijent { get; set; }
    }
}
