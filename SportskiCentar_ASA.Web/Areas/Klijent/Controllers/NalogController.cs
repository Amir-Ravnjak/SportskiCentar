using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Web.Helper;
using SportskiCentar_ASA.Data.EF;
using Microsoft.AspNetCore.Http;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.Areas.Klijent.ViewModels;

namespace SportskiCentar_ASA.Web.Areas.Klijent.Controllers
{
    [Autorizacija(false, TipKorisnika.Klijent)]
    [Area(nameof(Klijent))]
    public class NalogController : Controller
    {
        private MojContext db;
        private IHttpContextAccessor httpContext;
        public NalogController(MojContext db, IHttpContextAccessor httpContext)
        {
            this.db = db;
            this.httpContext = httpContext;
        }

        public IActionResult PrikazNaloga()
        {
            
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);
            NalogPrikazNalogaVM model = new NalogPrikazNalogaVM()
            {
                KlijentID = klijent.id,
                Ime = klijent.Ime,
                KorisnickoIme = trenutniNalog.KorisnickoIme,
                Prezime = klijent.Prezime,
                Grad = db.Gradovi.Where(s => s.id == klijent.GradID).Select(s => s.Naziv).SingleOrDefault()
            };
            return View(model);

        }
    }
}