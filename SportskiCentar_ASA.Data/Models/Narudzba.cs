using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Narudzba
    {
        public int id { get; set; }
        public int? KlijentID { get; set; }
        public Klijent Klijent { get; set; }
        public bool ZeliDostavu { get; set; }
        public int? DostavaID { get; set; }
        public Dostava Dostava { get; set; }
    }
}
