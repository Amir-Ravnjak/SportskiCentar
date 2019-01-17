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
using SportskiCentar_ASA.Web.ViewModels;

namespace SportskiCentar_ASA.Web.Areas.Uposlenik.Controllers
{
    [Autorizacija(false, TipKorisnika.Uposlenik)]
    [Area(nameof(Uposlenik))]
    public class TerminiController : Controller
    {
        private MojContext db;
        public int PageSize = 6;
        public TerminiController(MojContext context)
        {
            this.db = context;
        }

        public IActionResult Index(string sortOrder, int page = 1, string pretragaString = null)
        {
            ViewBag.ProstorijaSortParm = String.IsNullOrEmpty(sortOrder) ? "Prostorija_desc" : "";
            ViewBag.DatumSortParm = sortOrder == "Datum" ? "Datum_desc" : "Datum";

            TerminiIndexVM vm = new TerminiIndexVM();

            if (string.IsNullOrEmpty(pretragaString))
            {
                vm.Rows = db.Rezervacije.Include(x => x.Uplata).Include(x => x.Klijent).Include(x => x.Termin).ThenInclude(x => x.Prostorija).Where(x => x.Odobrena == false).Select(x => new TerminiIndexVM.Row()
                {
                    RezervacijaID = x.id,
                    Klijent = x.Klijent.Ime + " " + x.Klijent.Prezime,
                    DatumVrijeme = x.Termin.DatumIVrijeme.ToString("yyyy-MM-dd hh:mm"),
                    Prostorija = x.Termin.Prostorija.Naziv,
                    Uplata = x.Uplata.Iznos.ToString() ?? "0",
                    Odobrena = "Ne"
                }).OrderBy(x => x.RezervacijaID).Skip((page - 1) * PageSize).Take(PageSize).ToList();

                vm.PagingInfo = new Web.ViewModels.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = db.Rezervacije.Where(x => x.Odobrena == false).Count()
                };
            }
            else
            {
                vm.Rows = db.Rezervacije.Include(i => i.Uplata).Include(u => u.Klijent).Include(j => j.Termin).ThenInclude(k => k.Prostorija).Where(x => x.Odobrena == false).Select(x => new TerminiIndexVM.Row()
                {
                    RezervacijaID = x.id,
                    Klijent = x.Klijent.Ime + " " + x.Klijent.Prezime,
                    DatumVrijeme = x.Termin.DatumIVrijeme.ToString("yyyy-MM-dd hh:mm"),
                    Prostorija = x.Termin.Prostorija.Naziv,
                    Uplata = x.Uplata.Iznos.ToString() ?? "0",
                    Odobrena = "Ne"
                }).Where(y => y.Klijent.ToLower().Contains(pretragaString.ToLower()) || y.Prostorija.ToLower().Contains(pretragaString.ToLower())).ToList();

                vm.PagingInfo = new Web.ViewModels.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = db.Rezervacije.Where(x => (x.Klijent.Ime + " " + x.Klijent.Prezime).ToLower().Contains(pretragaString.ToLower()) || x.Termin.Prostorija.Naziv.ToLower().Contains(pretragaString.ToLower())).Count()
                };
            }

            switch (sortOrder)
            {
                case "Prostorija_desc":
                    vm.Rows = vm.Rows.OrderByDescending(x => x.Prostorija);
                    break;
                case "Datum":
                    vm.Rows = vm.Rows.OrderBy(x => x.DatumVrijeme);
                    break;
                case "Datum_desc":
                    vm.Rows = vm.Rows.OrderByDescending(x => x.DatumVrijeme);
                    break;
                default:
                    vm.Rows = vm.Rows.OrderBy(x => x.Prostorija);
                    break;
            }
            return View(vm);
        }
    

        public IActionResult OdobriRezervaciju(int id)
        {
            Rezervacija r = db.Rezervacije.First(x => x.id == id);
            if (!r.Odobrena)
                r.Odobrena = true;
            else
                r.Odobrena = false;

            db.Rezervacije.Update(r);
            db.SaveChanges();

            string status = r.Odobrena ? "Odobrena" : "Odobri";

            return Json(new { success = true, status });
        }

    }
}