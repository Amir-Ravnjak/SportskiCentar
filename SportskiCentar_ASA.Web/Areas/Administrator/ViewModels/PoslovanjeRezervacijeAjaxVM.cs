using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class PoslovanjeRezervacijeAjaxVM
    {

        public List<Rez> OdradjeneRezervacije { get; set; }
        public List<Rez> Rezervacije { get; set; }

        public class Rez
        {
            public DateTime DatumIVrijeme { get; set; }
            public float cijena { get; set; }
            public string klijent { get; set; }
            public bool odobrena { get; set; }
            public string prostorija { get; set; }
        }
    }
}
