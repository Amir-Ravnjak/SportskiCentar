using Microsoft.AspNetCore.Mvc.Rendering;
using SportskiCentar_ASA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Uposlenik.ViewModels
{
    public class AjaxPodkategorijeDodajVM
    {

        public List<SelectListItem> Kategorije { get; set; }
        public PodKategorija Podkategorija { get; set; }
    }
}
