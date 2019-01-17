using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Web.Areas.Administrator.ViewModels;
using SportskiCentar_ASA.Web.Helper;

namespace SportskiCentar_ASA.Web.Areas.Administrator.Controllers
{
    [Autorizacija(false, TipKorisnika.Administrator)]
    [Area(nameof(Administrator))]
    public class AjaxKorisniciController : Controller
    {
        MojContext _db;
        public AjaxKorisniciController(MojContext db)
        {
            _db = db;
        }
        public IActionResult Administratori()
        {
            AjaxAdministratoriVM model = new AjaxAdministratoriVM();

            model.admini = _db.Administratori.Select(x => new AjaxAdministratoriVM.Admin
            {
                username = x.Nalog.KorisnickoIme,
                ime = x.Ime,
                prezime = x.Prezime
            }).ToList();
            return View(model);
        }
        public IActionResult Uposlenici()
        {
            AjaxUposleniciVM model = new AjaxUposleniciVM();

            model.uposlenici = _db.Uposlenici.Select(x => new AjaxUposleniciVM.Upos
            {
                id = x.id,
                username = x.Nalog.KorisnickoIme,
                ime = x.Ime,
                prezime = x.Prezime
            }).ToList();

            return View(model);
        }
        public IActionResult UrediUposlenika(int id)
        {
            AjaxKorisniciUrediUposlenikaVM model = _db.Uposlenici.Where(x => x.id == id).Select(q => new AjaxKorisniciUrediUposlenikaVM
            {
                id = q.id,
                ime = q.Ime,
                prezime = q.Prezime,
                lozinka = q.Nalog.Lozinka,
                username = q.Nalog.KorisnickoIme
            }).FirstOrDefault();

            model.gradovi = _db.Gradovi.Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.id.ToString()
            }).ToList();

            model.tipUposlenika = _db.TipoviUposlenika.Select(x => new SelectListItem {
                Text = x.Naziv,
                Value = x.id.ToString()
            }).ToList();

            return View(model);
        }
        public IActionResult SnimiUposlenika(int id, string username, string lozinka, string ime, string prezime, string tipUposlenika, string grad)
        {
            Data.Models.Uposlenik u = _db.Uposlenici.Where(x => x.id == id).Include(q => q.Grad).Include(w => w.Nalog).FirstOrDefault();

            u.Nalog.KorisnickoIme = username;
            u.Nalog.Lozinka = lozinka;
            u.Ime = ime;
            u.Prezime = prezime;
            u.PlataID = 1;
            u.TipUposlenikaID = int.Parse(tipUposlenika);
            u.GradID = int.Parse(grad);

            _db.SaveChanges();

            return Redirect("Uposlenici");
        }

        public IActionResult ObrisiUposlenika(int id)
        {
            Data.Models.Uposlenik u = _db.Uposlenici.Where(x => x.id == id).Include(q => q.Nalog).First();

            _db.Nalozi.Remove(u.Nalog);

            _db.Uposlenici.Remove(u);

            _db.SaveChanges();

            return Redirect("Uposlenici");
        }
        public IActionResult Klijenti()
        {
            AjaxKlijentiVM model = new AjaxKlijentiVM();

            model.klijenti = _db.Klijenti.Select(x => new AjaxKlijentiVM.Klij
            {
                id = x.id,
                username = x.Nalog.KorisnickoIme,
                ime = x.Ime,
                prezime = x.Prezime,
                jmbg = x.JBMG,
                spol = x.Spol,
                grad = x.Grad.Naziv
            }).ToList();

            return View(model);
        }

        public IActionResult UrediKlijenta(int id)
        {
            AjaxKorisniciUrediKlijentaVM model = _db.Klijenti.Where(x => x.id == id).Select(q => new AjaxKorisniciUrediKlijentaVM {
                id = q.id,
                ime = q.Ime,
                jmbg = q.JBMG,
                spol = q.Spol,
                prezime = q.Prezime,
                lozinka = q.Nalog.Lozinka,
                username = q.Nalog.KorisnickoIme
            }).FirstOrDefault();

            model.gradovi = _db.Gradovi.Select(x => new SelectListItem {
                Text = x.Naziv,
                Value = x.id.ToString()
            }).ToList();

            return View(model);
        }

        public IActionResult SnimiKlijenta(int id, string username, string lozinka, string ime, string prezime, string spol, string jmbg, string grad)
        {
            Data.Models.Klijent k = _db.Klijenti.Where(x => x.id == id).Include(q => q.Grad).Include(w => w.Nalog).FirstOrDefault();

            k.Nalog.KorisnickoIme = username;
            k.Nalog.Lozinka = lozinka;
            k.Ime = ime;
            k.Prezime = prezime;
            k.Spol = spol;
            k.JBMG = jmbg;
            k.GradID = int.Parse(grad);

            _db.SaveChanges();

            return Redirect("Klijenti");
        }

        public IActionResult ObrisiKlijenta(int id)
        {
            Data.Models.Klijent k = _db.Klijenti.Where(x => x.id == id).Include(q => q.Nalog).First();

            _db.Nalozi.Remove(k.Nalog);

            _db.Klijenti.Remove(k);

            _db.SaveChanges();

            return Redirect("Klijenti");
        }

        public IActionResult DodajAdministratora()
        {
            return View();
        }

        public IActionResult DodajUposlenika()
        {
            AjaxDodajUposlenikaVM model = new AjaxDodajUposlenikaVM();

            model.gradovi = _db.Gradovi.Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.id.ToString()
            }).ToList();

            model.tipUposlenika = _db.TipoviUposlenika.Select(x => new SelectListItem {
                Text = x.Naziv,
                Value = x.id.ToString()
            }).ToList();
            return View(model);
        }
        public IActionResult DodajKlijenta()
        {
            AjaxDodajKlijentaVM model = new AjaxDodajKlijentaVM();

            model.gradovi = _db.Gradovi.Select(x => new SelectListItem {
                Text = x.Naziv,
                Value = x.id.ToString()
            }).ToList();

            return View(model);
        }
    }
}