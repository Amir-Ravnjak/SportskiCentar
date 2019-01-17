using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Fajl
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Tip { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public byte[] Podaci { get; set; }
    }
}
