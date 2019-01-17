using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class AjaxUposleniciVM
    {
        public List<Upos> uposlenici { get; set; }
        public class Upos
        {
            public int id { get; set; }
            public string username { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
        }
    }
}
