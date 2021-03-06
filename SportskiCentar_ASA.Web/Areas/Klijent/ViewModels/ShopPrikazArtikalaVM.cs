﻿using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Klijent.ViewModels
{
    public class ShopPrikazArtikalaVM
    {
        public IEnumerable<Row> rows { get; set; }
        public string pretragaString { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public class Row
        {
            public int ArtikalID { get; set; }
            public string Naziv { get; set; }
            public decimal Cijena { get; set; }
            public int Kolicina { get; set; }
            public Fajl Fajl { get; set; }
            public string Podkategorija { get; set; }
            public int OdabranaKolicina { get; internal set; }
        }
    }
}
