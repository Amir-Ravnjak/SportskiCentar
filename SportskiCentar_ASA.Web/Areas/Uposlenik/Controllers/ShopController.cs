using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.Areas.Uposlenik.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportskiCentar_ASA.Web.Helper;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SportskiCentar_ASA.Web.Areas.Uposlenik.Controllers
{
    [Autorizacija(false, TipKorisnika.Uposlenik)]
    [Area(nameof(Uposlenik))]
    public class ShopController : Controller
    {
        private MojContext db;
        public int PageSize = 4;
        public ShopController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index(string sortOrder, int page = 1, string pretragaString = null)
        {
            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "Naziv_desc" : "";
            ViewBag.CijenaSortParm = sortOrder == "Cijena" ? "Cijena_desc" : "Cijena";

            ArtikliIndexVM vm = new ArtikliIndexVM();
            if (string.IsNullOrEmpty(pretragaString))
            {
                vm.Rows = db.Artikli.Include(x => x.PodKategorija).Select(x => new ArtikliIndexVM.Row()
                {
                    ArtikalID = x.id,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Kolicina = x.Kolicina,
                    Fajl = db.Fajlovi.FirstOrDefault(w => w.Id == db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == x.id).FajlID),
                    Podkategorija = x.PodKategorija.Naziv
                }).OrderBy(x => x.ArtikalID).Skip((page - 1) * PageSize).Take(PageSize).ToList();

                vm.PagingInfo = new Web.ViewModels.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = db.Artikli.Count()
                };
            }
            else
            {
                vm.Rows = db.Artikli.Include(x => x.PodKategorija).Select(x => new ArtikliIndexVM.Row()
                {
                    ArtikalID = x.id,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Kolicina = x.Kolicina,
                    Fajl = db.Fajlovi.FirstOrDefault(w => w.Id == db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == x.id).FajlID),
                    Podkategorija = x.PodKategorija.Naziv
                }).Where(x => x.Podkategorija.ToLower().Contains(pretragaString.ToLower()) || x.Naziv.ToLower().Contains(pretragaString.ToLower())).ToList();

                vm.PagingInfo = new Web.ViewModels.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = db.Artikli.Where(x => x.PodKategorija.Naziv.ToLower().Contains(pretragaString.ToLower()) || x.Naziv.ToLower().Contains(pretragaString.ToLower())).Count()
                };
            }

            switch (sortOrder)
            {
                case "Naziv_desc":
                    vm.Rows = vm.Rows.OrderByDescending(x=>x.Naziv);
                    break;
                case "Cijena":
                    vm.Rows = vm.Rows.OrderBy(x => x.Cijena);
                    break;
                case "Cijena_desc":
                    vm.Rows = vm.Rows.OrderByDescending(x => x.Cijena);
                    break;
                default:
                    vm.Rows = vm.Rows.OrderBy(x => x.Naziv);
                    break;
            }
            return View(vm);
        }


        public IActionResult Dodaj()
        {
            ArtikliDodajVM vm = new ArtikliDodajVM()
            {
                Artikal = new Artikal(),
                Podkategorije = db.PodKategorije.Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.id.ToString()
                }).ToList()
            };

            return View(vm);
        }
        public IActionResult Uredi(int artikalID)
        {
            ArtikliDodajVM vm = new ArtikliDodajVM()
            {
                Artikal = db.Artikli.Find(artikalID),
                Podkategorije = db.PodKategorije.Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.id.ToString()
                }).ToList()
            };
            return View(nameof(Dodaj), vm);
        }
        [HttpPost]
        public IActionResult Snimi(ArtikliDodajVM vm, IFormFile dokument)
        {
            if (vm.Artikal.Cijena == 0)
                ModelState.AddModelError("", "Polje za cijenu je obavezno!");
            if (vm.Artikal.Kolicina == 0)
                ModelState.AddModelError("", "Polje za kolicinu je obavezno!");
            if (!ModelState.IsValid)
            {
                vm.Podkategorije = db.PodKategorije.Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.id.ToString()
                }).ToList();
                
                return View(nameof(Dodaj), vm);
            }
            Artikal artikal = null;
            if (vm.Artikal.id != 0)
            {
                artikal = vm.Artikal;
                db.Artikli.Update(artikal);
                db.SaveChanges();

                Fajl provjera = db.Fajlovi.FirstOrDefault(w => w.Id == db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == artikal.id).FajlID);
                var arr = new byte[1000000];

                if (dokument != null)
                {
                    using (MemoryStream ms2 = new MemoryStream())
                    {
                        dokument.CopyTo(ms2);
                        arr = ms2.ToArray();
                    }

                    if (provjera == null || arr != provjera.Podaci)
                    {
                        if (dokument.Length <= 1000000)
                        {
                            Fajl noviFajl = new Fajl();
                            noviFajl.DatumDodavanja = DateTime.Now;
                            noviFajl.Naziv = dokument.FileName;
                            noviFajl.Tip = dokument.ContentType;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                dokument.CopyTo(ms);
                                noviFajl.Podaci = ms.ToArray();
                            }
                            db.Fajlovi.Add(noviFajl);

                            db.SaveChanges();

                            ArtikliFajlovi af = db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == artikal.id);
                            af.FajlID = noviFajl.Id;
                            db.ArtikliFajlovi.Update(af);
                            db.SaveChanges();

                            db.Fajlovi.Remove(provjera);//brisanje stare slike
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                artikal = new Artikal();
                artikal = vm.Artikal;
                db.Artikli.Add(artikal);
                db.SaveChanges();
                if (dokument != null)
                {
                    if (dokument.Length <= 1000000)
                    {
                        Fajl noviFajl = new Fajl();
                        noviFajl.DatumDodavanja = DateTime.Now;
                        noviFajl.Naziv = dokument.FileName;
                        noviFajl.Tip = dokument.ContentType;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            dokument.CopyTo(ms);
                            noviFajl.Podaci = ms.ToArray();
                        }
                        db.Fajlovi.Add(noviFajl);
                        db.SaveChanges();

                        ArtikliFajlovi af = new ArtikliFajlovi();
                        af.FajlID = noviFajl.Id;
                        af.ArtikalID = artikal.id;
                        db.ArtikliFajlovi.Add(af);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Obrisi(int artikalID)
        {

            Fajl f = db.Fajlovi.FirstOrDefault(w => w.Id == db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == artikalID).FajlID);
            if (f != null)
            {
                ArtikliFajlovi af = db.ArtikliFajlovi.FirstOrDefault(y => y.FajlID == f.Id && y.ArtikalID == artikalID);
                db.ArtikliFajlovi.Remove(af);
                db.Fajlovi.Remove(f);
            }
            db.Artikli.Remove(db.Artikli.Find(artikalID));
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DodajPodkategoriju()
        {
            AjaxPodkategorijeDodajVM vm = new AjaxPodkategorijeDodajVM()
            {
                Podkategorija = new PodKategorija(),
                Kategorije = db.Kategorije.Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.id.ToString()
                }).ToList()
            };
            return PartialView(vm);
        }
        public IActionResult SnimiPodkategoriju(AjaxPodkategorijeDodajVM vm)
        {
            if(string.IsNullOrEmpty(vm.Podkategorija.Naziv))
            {
                ModelState.AddModelError("", "Polje naziv je obavezno!");
            }
            if (!ModelState.IsValid)
            {
                return View(nameof(DodajPodkategoriju), vm);
            }
            PodKategorija p = new PodKategorija()
            {
                KategorijaID = vm.Podkategorija.KategorijaID,
                Naziv = vm.Podkategorija.Naziv
            };
            db.PodKategorije.Add(p);
            db.SaveChanges();
            return RedirectToAction(nameof(Dodaj));
        }


        [HttpGet]
        public FileStreamResult ViewImage(int FajlID)
        {
            Fajl image = db.Fajlovi.FirstOrDefault(m => m.Id == FajlID);
            MemoryStream ms = new MemoryStream(image.Podaci);
            return new FileStreamResult(ms, image.Tip);
        }
    }
}