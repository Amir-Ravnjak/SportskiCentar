using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Klijent.ViewModels
{
    public class NalogPrikazNalogaVM
    {
        public int KlijentID { get; internal set; }
        public string Ime { get; internal set; }
        public string KorisnickoIme { get; internal set; }
        public string Prezime { get; internal set; }
        public string Grad { get; internal set; }
    }
}
