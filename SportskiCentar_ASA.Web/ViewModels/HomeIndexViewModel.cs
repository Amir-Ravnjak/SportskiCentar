using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public Detalji Informacije { get; set; }

        public class Detalji
        {
            public int UkupnoProizvoda { get; set; }
            public int UkupnoUposlenika { get; set; }
            public int UkupnoKlijenata { get; set; }
            public decimal UkupnaZarada { get; set; }

        }
    }
}
