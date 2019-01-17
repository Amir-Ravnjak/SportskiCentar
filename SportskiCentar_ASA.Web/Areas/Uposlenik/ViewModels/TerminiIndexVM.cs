using SportskiCentar_ASA.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Uposlenik.ViewModels
{
    public class TerminiIndexVM
    {
        public IEnumerable<Row> Rows { get; set; }
        public string pretragaString { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public class Row
        {
            public int RezervacijaID { get; set; }
            public string Klijent { get; set; }
            public string DatumVrijeme { get; set; }
            public string Prostorija { get; set; }
            public string Uplata { get; set; }
            public string Odobrena { get; set; }
        }
    }
}
