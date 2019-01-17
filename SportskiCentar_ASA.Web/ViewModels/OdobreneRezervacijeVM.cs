using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.ViewModels
{
    public class OdobreneRezervacijeVM
    {
        public List<OdRezervacije> OdobreneRezervacije { get; set; }

        public class OdRezervacije
        {
            public int Sr { get; set; }
            public string Title { get; set; }
            public string Prostorija { get; set; }
            public string Start_Date { get; set; }
            public string End_Date { get; set; }
        }
    }
}
