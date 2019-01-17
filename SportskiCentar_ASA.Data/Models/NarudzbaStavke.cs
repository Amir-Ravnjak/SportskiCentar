using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class NarudzbaStavke
    {
        public int id { get; set; }
        public int ArtikalID { get; set; }
        public Artikal Artikal { get; set; }
        public int Kolicina { get; set; }
        public int NarudzbaID { get; set; }
        public Narudzba Narudzba { get; set; }
    }
}
