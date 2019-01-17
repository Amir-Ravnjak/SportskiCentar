using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class AjaxDodajUposlenikaVM
    {
        public List<SelectListItem> tipUposlenika { get; set; }
        public List<SelectListItem> gradovi { get; set; }
    }
}
