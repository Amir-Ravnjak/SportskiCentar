using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Web.Helper;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Web.Areas.Klijent.ViewModels;
using SportskiCentar_ASA.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace SportskiCentar_ASA.Web.Areas.Klijent.Controllers
{
    [Autorizacija(false, TipKorisnika.Klijent)]
    [Area(nameof(Klijent))]
    public class RezervacijeController : Controller
    {
        private MojContext db;
        public int PageSize = 6;

        private IHttpContextAccessor httpContextAccessor;


       
        public RezervacijeController(MojContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.db = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult PrikazTermina()
        {
          
            List<Termin> listaTermina = db.Termini.Include(w=>w.Prostorija).ToList();
            List<Rezervacija> listaRezervacija = db.Rezervacije.Include(s=>s.Termin).ToList();

            List<Termin> slobodniTermini=new List<Termin>();
            List<Termin> zauzetiTermini = new List<Termin>();


            foreach (var x in listaTermina)
            {
                bool nadjen =false;
                
                foreach (var y in listaRezervacija)
                {
                    if (x.id == y.TerminID)
                    {
                        nadjen = true;
                        
                    }
                }
                if (nadjen != true)
                {
                    slobodniTermini.Add(x);
                }
                
            }

            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);


            RezervacijePrikazTerminaVM model = new RezervacijePrikazTerminaVM {
               KlijentID= klijent.id,
                rows = slobodniTermini.Select(x=>new RezervacijePrikazTerminaVM.Row {
                    TerminID=x.id,
                    Prostorija=x.Prostorija.Naziv,
                    DatumVrijeme=x.DatumIVrijeme,
                    Cijena=x.Cijena
                }).ToList(),


               
        };
            return View(model);
        }
        public IActionResult RezervisiTermin( int TerminID)
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);

            Rezervacija novaRezervacija = new Rezervacija {
                Odobrena=false,
                TerminID=TerminID,
                KlijentID= klijent.id,
            };

            db.Rezervacije.Add(novaRezervacija);
            db.SaveChanges();
            return RedirectToAction("PrikazTermina");
        }
        public IActionResult PrikazRezervacija(string sortOrder, int page = 1) {

            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);


            ViewBag.ProstorijaSortParm = String.IsNullOrEmpty(sortOrder)? "Prostorija_desc" : "";
            ViewBag.DatumSortParm=sortOrder=="Datum" ? "Datum_desc" : "Datum";

            RezervacijePrikazRezervacijaVM model = new RezervacijePrikazRezervacijaVM ();

            
                model.rows = db.Rezervacije
                                   .Include(x => x.Uplata)
                                   .Include(x => x.Klijent)
                                   .Include(x => x.Termin)
                                   .ThenInclude(x => x.Prostorija )
                                   .Where(x => x.KlijentID == klijent.id)
                                   .Select(x => new RezervacijePrikazRezervacijaVM.Row()
                                   {
                                       RezervacijaID = x.id,
                                       Klijent = x.Klijent.Ime + " " + x.Klijent.Prezime,
                                       DatumVrijeme = x.Termin.DatumIVrijeme.ToString("yyyy-MM-dd hh:mm"),
                                       Prostorija = x.Termin.Prostorija.Naziv,
                                       Uplata = x.Uplata.Iznos.ToString() ?? "0",
                                       Odobrena=x.Odobrena
                                   }).OrderBy(x => x.RezervacijaID).Skip((page - 1) * PageSize).Take(PageSize).ToList();

                model.PagingInfo = new Web.ViewModels.PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = db.Rezervacije.Count()
                };
           
            switch (sortOrder)
            {
                case "Prostorija_desc":
                    model.rows = model.rows.OrderByDescending(x => x.Prostorija);
                    break;
                case "Datum":
                    model.rows = model.rows.OrderBy(x => x.DatumVrijeme);
                    break;
                case "Datum_desc":
                    model.rows = model.rows.OrderByDescending(x => x.DatumVrijeme);
                    break;
                default:
                    model.rows = model.rows.OrderBy(x => x.Prostorija);
                    break;
            }
            return View(model);
        }
        public IActionResult Index()
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);


            return View();
        }
    }
}