using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class AjaxKlijentiVM
    {
        public List<Klij> klijenti { get; set; }


        public class Klij
        {
            public int id { get; set; }
            public string username { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public string spol { get; set; }
            public string jmbg { get; set; }
            public string grad { get; set; }
        }
    }
}
