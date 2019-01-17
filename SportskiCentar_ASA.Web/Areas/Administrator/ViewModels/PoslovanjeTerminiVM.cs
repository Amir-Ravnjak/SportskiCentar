using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class PoslovanjeTerminiVM
    {

        public List<term> termini { get; set; }
        public List<rezerv> rezervacije { get; set; }

        public class term
        {
            public int id { get; set; }
            public DateTime DatumIVrijeme { get; set; }
            public float cijena { get; set; }
            public string prostorija { get; set; }
            public bool rezervacija { get; set; }
        }
        public class rezerv
        {
            public int id { get; set; }
            public int terminId { get; set; }
            public bool odobrena { get; set; }
            public string klijent { get; set; }
        }
    }
}
