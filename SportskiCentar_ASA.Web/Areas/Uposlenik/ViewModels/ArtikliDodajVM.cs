using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportskiCentar_ASA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Uposlenik.ViewModels
{
    public class ArtikliDodajVM
    {
        public Artikal Artikal { get; set; }
        public List<SelectListItem> Podkategorije;
    }
}
