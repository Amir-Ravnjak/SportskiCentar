using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Administrator
    {
   
        public int id { get; set; }
        public int NalogID { get; set; }
        public virtual Nalog Nalog { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }        
        public int PlataID { get; set; }
        public Plata Plata { get; set; }
    }
}
