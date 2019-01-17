using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportskiCentar_ASA.Web.Areas.Administrator.ViewModels
{
    public class AjaxAdministratoriVM
    {
        public List<Admin> admini { get; set; }
        public class Admin
        {
            public string username { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
        }

    }
}
