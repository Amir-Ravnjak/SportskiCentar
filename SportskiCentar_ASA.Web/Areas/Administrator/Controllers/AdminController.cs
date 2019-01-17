using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.Areas.Administrator.ViewModels;
using SportskiCentar_ASA.Web.Helper;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
namespace SportskiCentar_ASA.Web.Areas.Administrator.Controllers
{

    [Autorizacija(false, TipKorisnika.Administrator)]
    [Area(nameof(Administrator))]
    public class AdminController : Controller
    {
        MojContext _db;
        public AdminController(MojContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            AdminIndexVM model = new AdminIndexVM();

            


            //Info o nalozima
            model.trenutnoKorisnika = _db.Nalozi.Count();
            model.klijenata = _db.Klijenti.Count();
            model.uposlenika = _db.Nalozi.Where(q=>q.IsUposlenik).Count();
            model.administratora = _db.Administratori.Count();

            //Info o terminima
            model.odrađenoTermina = _db.Rezervacije.Where(x => x.Termin.DatumIVrijeme <= DateTime.Now).Count();
            model.propaloTermina = _db.Termini.Where(x => x.DatumIVrijeme <= DateTime.Now).Count() - model.odrađenoTermina;
            model.rezervisanoTermina = _db.Rezervacije.Count(x => x.Termin.DatumIVrijeme > DateTime.Now);
            model.slobodniTermini = _db.Termini.Where(x => x.DatumIVrijeme > DateTime.Now).Count() - model.rezervisanoTermina;


            //Shop info
            model.narudzbe = _db.Racun.Count();
            List<RacunStavke> stavke = _db.RacunStavke.ToList();
            foreach (var x in stavke)
            {
                model.artikli += x.Kolicina;
            }

            return View(model);
        }
    }
}