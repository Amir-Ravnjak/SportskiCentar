using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Web.Helper;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Web.Areas.Klijent.ViewModels;
using Microsoft.EntityFrameworkCore;
using SportskiCentar_ASA.Data.Models;
using System.IO;

namespace SportskiCentar_ASA.Web.Areas.Klijent.Controllers
{
    [Autorizacija(false, TipKorisnika.Klijent)]
    [Area(nameof(Klijent))]
    public class ShopController : Controller
    {
        MojContext db;
        public int PageSize = 4;

        public ShopController(MojContext db)
        {
            this.db = db;
        }

        public IActionResult PrikazArtikala(string sortOrder, int page = 1, string pretragaString = null)
        {
            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "Naziv_desc" : "";
            ViewBag.CijenaSortParm = sortOrder == "Cijena" ? "Cijena_desc" : "Cijena";


            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);



            ShopPrikazArtikalaVM model = new ShopPrikazArtikalaVM();

            if (string.IsNullOrEmpty(pretragaString))
            {
                model.rows = db.Artikli.Include(x => x.PodKategorija)
                    .Where(s => s.Kolicina > 0)
                    .Select(x => new ShopPrikazArtikalaVM.Row()
                    {
                        ArtikalID = x.id,
                        Naziv = x.Naziv,
                        Cijena = x.Cijena,
                        Kolicina = x.Kolicina,
                        Fajl = db.Fajlovi.FirstOrDefault(w => w.Id == db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == x.id).FajlID),
                        OdabranaKolicina = 1,
                        Podkategorija = x.PodKategorija.Naziv
                    }).OrderBy(x => x.ArtikalID).Skip((page - 1) * PageSize).Take(PageSize).ToList();

                model.PagingInfo = new Web.ViewModels.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = db.Artikli.Count()
                };
            }
            else
            {
                model.rows = db.Artikli
                    .Include(x => x.PodKategorija)
                      .Where(s => s.Kolicina > 0)
                    .Select(x => new ShopPrikazArtikalaVM.Row()
                    {
                        ArtikalID = x.id,
                        Naziv = x.Naziv,
                        Cijena = x.Cijena,
                        Kolicina = x.Kolicina,
                        Fajl = db.Fajlovi.FirstOrDefault(w => w.Id == db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == x.id).FajlID),
                        OdabranaKolicina = 1,
                        Podkategorija = x.PodKategorija.Naziv
                    }).Where(x => x.Podkategorija.ToLower().Contains(pretragaString.ToLower()) || x.Naziv.ToLower().Contains(pretragaString.ToLower())).ToList();

                model.PagingInfo = new Web.ViewModels.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = db.Artikli.Where(x => x.PodKategorija.Naziv.ToLower().Contains(pretragaString.ToLower()) || x.Naziv.ToLower().Contains(pretragaString.ToLower())).Count()
                };
            }

            switch (sortOrder)
            {
                case "Naziv_desc":
                    model.rows = model.rows.OrderByDescending(x => x.Naziv);
                    break;
                case "Cijena":
                    model.rows = model.rows.OrderBy(x => x.Cijena);
                    break;
                case "Cijena_desc":
                    model.rows = model.rows.OrderByDescending(x => x.Cijena);
                    break;
                default:
                    model.rows = model.rows.OrderBy(x => x.Naziv);
                    break;
            }
            return View(model);
        }
        public IActionResult StaviUKorpu(int ArtikalID, int Kolicina)
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);



            Narudzba postojecaNarudzba = db.Narudzbe
                .Where(s => s.KlijentID == klijent.id)
                .FirstOrDefault();

            if (postojecaNarudzba == null)
            {

                Narudzba novaNarudzba = new Narudzba
                {
                    KlijentID = klijent.id,
                    ZeliDostavu = false,
                };

                db.Narudzbe.Add(novaNarudzba);

                NarudzbaStavke novaNarudzbaStavke = new NarudzbaStavke
                {
                    ArtikalID = ArtikalID,
                    Kolicina = Kolicina,
                    NarudzbaID = novaNarudzba.id
                };
                db.NarudzbaStavke.Add(novaNarudzbaStavke);
            }
            else
            {
                NarudzbaStavke DaLiVecPostoji = this.db.NarudzbaStavke
                 .FirstOrDefault(x => x.ArtikalID == ArtikalID && x.NarudzbaID == postojecaNarudzba.id);




                if (DaLiVecPostoji != null)
                {
                    DaLiVecPostoji.Kolicina += Kolicina;

                    db.SaveChanges();
                    return RedirectToAction("PrikazArtikala");
                }
                else
                {
                    NarudzbaStavke novaNarudzbaStavke = new NarudzbaStavke
                    {
                        ArtikalID = ArtikalID,
                        Kolicina = Kolicina,
                        NarudzbaID = postojecaNarudzba.id
                    };
                    db.NarudzbaStavke.Add(novaNarudzbaStavke);
                }

            }

            db.SaveChanges();


            return RedirectToAction("PrikazArtikala");
        }

        [HttpGet]
        public FileStreamResult ViewImage(int FajlID)
        {

            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);


            Fajl image = db.Fajlovi.FirstOrDefault(m => m.Id == FajlID);
            MemoryStream ms = new MemoryStream(image.Podaci);
            return new FileStreamResult(ms, image.Tip);
        }

        public IActionResult UvidKorpe()
        {

            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);


            Narudzba postojecaNarudzba = db.Narudzbe
                .Where(s => s.KlijentID == klijent.id)
                .FirstOrDefault();


            if (postojecaNarudzba != null)
            {
                List<NarudzbaStavke> sviArtikli = db.NarudzbaStavke
              .Include(s => s.Artikal)
              .Where(s => s.NarudzbaID == postojecaNarudzba.id)
              .ToList();

                decimal UkupanIznos = 0;


                foreach (var x in sviArtikli)
                {
                    UkupanIznos += x.Kolicina * x.Artikal.Cijena;
                }
                ShopUvidKorpeVM model = new ShopUvidKorpeVM();

                model.NarudzbaID = postojecaNarudzba.id;
                model.UkupanIznos = UkupanIznos;
                model.ZeliDostavu = postojecaNarudzba.ZeliDostavu;
                model.rows = db.NarudzbaStavke
                .Include(x => x.Artikal)
                .Include(x => x.Narudzba)
                .Include(x => x.Artikal.PodKategorija)
                .Where(s => s.NarudzbaID == postojecaNarudzba.id)
                    .Select(x => new ShopUvidKorpeVM.Row()
                    {
                        
                        NarudzbaStavkeID = x.id,
                        ArtikalID = x.ArtikalID,
                        Naziv = x.Artikal.Naziv,
                        Cijena = x.Artikal.Cijena,
                        Kolicina = x.Artikal.Kolicina,
                        Fajl = db.Fajlovi.FirstOrDefault(w => w.Id == db.ArtikliFajlovi.FirstOrDefault(k => k.ArtikalID == x.id).FajlID),
                        OdabranaKolicina = x.Kolicina,
                        Podkategorija = x.Artikal.PodKategorija.Naziv,
                    }).OrderBy(x => x.ArtikalID).ToList();



                return View(model);
            }
            else
            {
                ShopUvidKorpeVM model = new ShopUvidKorpeVM();
                model.NarudzbaID = 0;
                return View(model);
            }
          
        }

        public IActionResult PovecajSmanjiStanje(int NarudzbaStavkeID, int OdabranaKolicina)
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);

            NarudzbaStavke postojecaNarudzba = db.NarudzbaStavke.Where(s => s.id == NarudzbaStavkeID).SingleOrDefault();

            postojecaNarudzba.Kolicina = OdabranaKolicina;
            db.SaveChanges();

            return RedirectToAction("UvidKorpe");
        }
        public IActionResult Obrisi(int NarudzbaStavkeID)
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);

            NarudzbaStavke narudzbaStavke = db.NarudzbaStavke.Where(s => s.id == NarudzbaStavkeID).SingleOrDefault();

            db.NarudzbaStavke.Remove(narudzbaStavke);
            db.SaveChanges();

            return RedirectToAction("UvidKorpe");
        }
    }
}