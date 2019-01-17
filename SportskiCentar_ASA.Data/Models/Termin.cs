using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Termin
    {
        public int id { get; set; }
        public int ProstorijaID { get; set; }
        public Prostorija Prostorija { get; set; }
        public DateTime DatumIVrijeme { get; set; }
        public decimal Cijena { get; set; }
    }
}
