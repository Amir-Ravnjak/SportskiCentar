using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class AjaxKorisniciUrediUposlenikaVM
    {
        public int id { get; set; }
        public string username { get; set; }
        public string lozinka { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public List<SelectListItem> tipUposlenika { get; set; }
        public List<SelectListItem> gradovi { get; set; }
    }
}
