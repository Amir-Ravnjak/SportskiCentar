using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Grad
    {
        public int id { get; set; }
        public string Naziv { get; set; }
        public int DrzavaID { get; set; }
        public Drzava Drzava { get; set; }
    }
}
