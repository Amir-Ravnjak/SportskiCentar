using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class PoslovanjeTerminiAjaxVM
    {
        public List<Term> PropaliTermini { get; set; }
        public List<Term> Termini { get; set; }

        public class Term
        {
            public DateTime DatumIVrijeme { get; set; }
            public string Prostorija { get; set; }
            public float cijena { get; set; }
        }
    }
}
