using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Nalog
    {
        public int id { get; set; }        
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
   
        public bool IsAdministrator { get; set; }
   
        public bool IsUposlenik { get; set; }
    
        public bool IsKlijent { get; set; }

        public string email { get; set; }
        public bool emailActivated { get; set; }

    }
}
