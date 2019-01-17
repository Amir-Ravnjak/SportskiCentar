using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportskiCentar_ASA.Data.Models;

namespace SportskiCentar_ASA.Web.Areas.Klijent.ViewModels
{
    public class RezervacijePrikazTerminaVM
    {

        public List<Termin> sviTermini { get; set; }
        public List<Termin> zauzetiTermini { get; internal set; }
        public List<Termin> slobodniTermini { get; internal set; }

        public List<Row> rows { get; set; }
        public int KlijentID { get; internal set; }

        public class Row
        {
            public int TerminID { get; internal set; }
            public string Prostorija { get; internal set; }
            public int ProstorijaID { get; set; }
            public DateTime DatumVrijeme { get; internal set; }
            public decimal Cijena { get; internal set; }
        }
    }
}
