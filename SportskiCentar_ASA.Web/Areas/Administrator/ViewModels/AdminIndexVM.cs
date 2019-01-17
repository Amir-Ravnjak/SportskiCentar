using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class AdminIndexVM
    {


        public int trenutnoKorisnika { get; set; }
        public int uposlenika { get; set; }
        public int klijenata { get; set; }
        public int administratora { get; set; }


        public int odrađenoTermina { get; set; }
        public int propaloTermina { get; set; }
        public int rezervisanoTermina { get; set; }
        public int slobodniTermini { get; set; }

        public int narudzbe { get; set; }
        public int artikli { get; set; }
    }
}
