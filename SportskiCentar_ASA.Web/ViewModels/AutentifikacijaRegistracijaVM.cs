using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.ViewModels
{
    public class AutentifikacijaRegistracijaVM
    {
        [Required(ErrorMessage = "Ime je obavezno.")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno.")]
        public string Prezime { get; set; }
        public string Spol { get; set; }
        [Required(ErrorMessage = "JMBG je obavezan.")]
        public string JBMG { get; set; }
        [Required(ErrorMessage = "Korisnicko ime je obavezno.")]
        public string KorisnickoIme { get; set; }
        [Required(ErrorMessage ="Lozinka je obavezna")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email je obavezan")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        public List<SelectListItem> Gradovi { get; set; }
        //[Display(Name = "Country"), 
        //    Required(ErrorMessage = "CountryIsRequired"),
        //    Range(1, int.MaxValue, ErrorMessage = "CountryIsRequired")]

        public int GradID { get; set; }
    }
}
