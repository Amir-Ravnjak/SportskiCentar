using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Korisnicko ime je obavezno.")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
