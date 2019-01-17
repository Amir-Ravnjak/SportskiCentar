using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class ArtikliFajlovi
    {
        [Key]
        public int ArtikliFajloviId { get; set; }
        public int ArtikalID { get; set; }
        public Artikal Artikal { get; set; }

        public int FajlID { get; set; }
        public Fajl Fajl { get; set; }
    }
}
