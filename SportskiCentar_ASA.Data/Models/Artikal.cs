using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Artikal
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Polje za naziv je obavezno!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Polje za cijenu je obavezno!")]
        public decimal Cijena { get; set; }
        [Required(ErrorMessage = "Polje za kolicinu je obavezno!")]
        public int Kolicina { get; set; }
        public int PodKategorijaID { get; set; }
        public PodKategorija PodKategorija { get; set; }
    }
}
