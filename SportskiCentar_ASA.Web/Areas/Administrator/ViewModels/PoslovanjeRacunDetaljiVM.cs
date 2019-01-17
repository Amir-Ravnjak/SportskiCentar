using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class PoslovanjeRacunDetaljiVM
    {
        public DateTime datumIzdavnaja { get; set; }
        public string klijent { get; set; }
        public float ukupanIznos { get; set; }
        public List<Row> racunStavke { get; set; }

        public class Row
        {            
            public int kolicina { get; set; }
            public string artikalNaziv { get; set; }
            public float artikalCijena { get; set; }
        }
    }
}
