using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.Areas.Uposlenik.ViewModels;
using SportskiCentar_ASA.Web.Helper;

namespace SportskiCentar_ASA.Web.Areas.Uposlenik.Controllers
{
    [Autorizacija(false, TipKorisnika.Uposlenik)]
    [Area(nameof(Uposlenik))]
    public class NalogController : Controller
    {
        private MojContext db;
        private IHttpContextAccessor httpContext;
        public NalogController(MojContext db, IHttpContextAccessor httpContext)
        {
            this.db = db;
            this.httpContext = httpContext;
        }
        public IActionResult Index()
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Uposlenik u = db.Uposlenici.FirstOrDefault(x => x.NalogID == trenutniNalog.id);
            NalogIndexVM vm = new NalogIndexVM()
            {
                UposlenikID = u.id,
                Ime = u.Ime,
                KorisnickoIme = trenutniNalog.KorisnickoIme,
                Prezime = u.Prezime,
                Grad = db.Gradovi.FirstOrDefault(x => x.id == u.GradID).Naziv,//nije moglo na laksi nacin
                Plata = db.Plate.FirstOrDefault(x => x.id == u.PlataID).Iznos,
                TipUposlenika = db.TipoviUposlenika.FirstOrDefault(x => x.id == u.TipUposlenikaID).Naziv
            };
            return View(vm);
        }

    }
}