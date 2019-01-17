using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class PodKategorija
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Polje je obavezno!")]
        public string Naziv { get; set; }
        public int KategorijaID { get; set; }
        public Kategorija Kategorija { get; set; }
    }
}
