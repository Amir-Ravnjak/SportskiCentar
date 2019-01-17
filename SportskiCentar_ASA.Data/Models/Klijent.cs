using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Klijent
    {
        public int id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Spol { get; set; }
        public string JBMG { get; set; }
        public int GradID { get; set; }
        public Grad Grad { get; set; }
        public int NalogID { get; set; }
        public virtual Nalog Nalog { get; set; }
    }
}
