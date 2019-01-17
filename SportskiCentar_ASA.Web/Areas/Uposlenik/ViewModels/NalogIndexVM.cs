using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Uposlenik.ViewModels
{
    public class NalogIndexVM
    {
        public int UposlenikID { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Grad { get; set; }
        public string TipUposlenika { get; set; }
        public decimal Plata { get; set; }
    }
}
