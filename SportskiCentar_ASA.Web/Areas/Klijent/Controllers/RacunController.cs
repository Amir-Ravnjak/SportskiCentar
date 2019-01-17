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

namespace SportskiCentar_ASA.Web.Areas.Klijent.Controllers
{
    [Autorizacija(false, TipKorisnika.Klijent)]
    [Area(nameof(Klijent))]
    public class RacunController : Controller
    {
        MojContext db;

        public RacunController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);
            return PartialView();
        }
        public IActionResult PraviRacun(int NarudzbaID,bool ZeliDostavu,decimal UkupanIznos)
        {
            Nalog trenutniNalog = Autentifikacija.GetLogiraniNalog(HttpContext);
            Data.Models.Klijent klijent = db.Klijenti.FirstOrDefault(x => x.NalogID == trenutniNalog.id);

            ViewData["brojArtikala"] = db.NarudzbaStavke.Where(x => x.Narudzba.KlijentID == klijent.id).Sum(q => q.Kolicina);

            Narudzba odabranaNarudzba = db.Narudzbe.Include(s=>s.Dostava)
                .Where(s => s.id == NarudzbaID).SingleOrDefault();

            odabranaNarudzba.ZeliDostavu = ZeliDostavu;

            Racun noviRacun = new Racun();
            noviRacun.NarudzbaID = NarudzbaID;
            noviRacun.DatumIzdavanja = DateTime.Now;
            noviRacun.KlijentId = klijent.id;
            noviRacun.UkupanIznos = UkupanIznos;

            if (ZeliDostavu)
            {
                Dostava novaDostava = new Dostava {
                    Adresa = db.Gradovi.Where(s => s.id == klijent.GradID)
                    .Select(s => s.Naziv).SingleOrDefault(),
                    Cijena=10
                };
                db.Dostava.Add(novaDostava);
                noviRacun.DostavaId = novaDostava.id;
                odabranaNarudzba.DostavaID = novaDostava.id;

                if (UkupanIznos < 100)
                {
                    noviRacun.UkupanIznos += 10;
                }
            }

            db.Racun.Add(noviRacun);

            List<NarudzbaStavke> sviArtikli = db.NarudzbaStavke
                .Include(s=>s.Artikal)
                .Where(s => s.NarudzbaID == NarudzbaID)
                .ToList();

            foreach (var x in sviArtikli)
            {
                RacunStavke RS = new RacunStavke {
                    Kolicina=x.Kolicina,
                    ArtikalId=x.ArtikalID,
                    RacunId=noviRacun.id
                };
                Artikal artikal = db.Artikli.Where(s => s.id == x.ArtikalID).SingleOrDefault();
                artikal.Kolicina -= x.Kolicina;
                db.RacunStavke.Add(RS);
            }
            odabranaNarudzba.KlijentID = null;
            db.SaveChanges();

            RacunPraviRacunVM model = new RacunPraviRacunVM {
                Dostava=ZeliDostavu
            };
            return PartialView(model);
        }
    }
}