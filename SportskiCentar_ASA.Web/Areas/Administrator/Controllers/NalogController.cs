using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.Areas.Administrator.ViewModels;
using SportskiCentar_ASA.Web.Helper;

namespace SportskiCentar_ASA.Web.Areas.Administrator.Controllers
{
    [Autorizacija(false, TipKorisnika.Administrator)]
    [Area(nameof(Administrator))]
    public class NalogController : Controller
    {
        MojContext _db;
        public NalogController(MojContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            AdminNalogIndexVM model = _db.Administratori.Select(x => new AdminNalogIndexVM {
                id = x.id,
                ime = x.Ime,
                prezime = x.Prezime,
                username = x.Nalog.KorisnickoIme,
                lozinka = x.Nalog.Lozinka,
                email = x.Nalog.email
            }).FirstOrDefault();

            return View(model);
        }

        public IActionResult Snimi(int id, string ime, string prezime, string username, string lozinka, string email) {

            Data.Models.Administrator a = _db.Administratori.Where(x => x.id == id).Include(q => q.Nalog).First();

            a.Ime = ime;
            a.Prezime = prezime;
            a.Nalog.KorisnickoIme = username;
            a.Nalog.Lozinka = lozinka;
            a.Nalog.email = email;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}