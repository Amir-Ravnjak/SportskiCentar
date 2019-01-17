using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class PoslovanjeShopVM
    {
        public List<SelectListItem> Klijenti { get; set; }
        public List<Racuni> racuni { get; set; }
        public List<Artikli> artikli { get; set; }

        public class Racuni
        {
            public int id { get; set; }
            public DateTime DatumIzdavanja { get; set; }
            public int? Dostava { get; set; }
            public string Klijent { get; set; }
            public float UkupanIznos { get; set; }

        }

        public class Artikli
        {
            public string naziv { get; set; }
            public int kolicina { get; set; }
            public float cijena { get; set; }
            public string podKategorija { get; set; }
            public string kategorija { get; set; }
        }
    }
}
