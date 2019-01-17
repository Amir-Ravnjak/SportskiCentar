using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.Areas.Administrator.ViewModels;
using SportskiCentar_ASA.Web.Helper;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;

namespace SportskiCentar_ASA.Web.Areas.Administrator.Controllers
{

    [Autorizacija(false, TipKorisnika.Administrator)]
    [Area(nameof(Administrator))]
    public class KorisniciController : Controller
    {
        MojContext _db;
        public KorisniciController(MojContext db)
        {
            _db = db;
        }
        public IActionResult Pregled()
        {
            
            return View();
        }
        public IActionResult Dodaj()
        {
            KorisniciDodajVM model = new KorisniciDodajVM();
            return View(model);
        }
        public IActionResult Snimi(string username, string lozinka, string tipKorisnika, string email, string ime, string prezime,string tipUposlenika, string spol, string jmbg, string grad)
        {
            Nalog nalog = new Nalog { KorisnickoIme = username, Lozinka = lozinka, email = email, emailActivated = false };
            
            switch (tipKorisnika)
            {
                case "1":
                    nalog.IsAdministrator = true; nalog.IsUposlenik = false; nalog.IsKlijent = false;
                    _db.Nalozi.Add(nalog);
                    _db.Administratori.Add(new Data.Models.Administrator {
                        Ime = ime,
                        Prezime = prezime,
                        NalogID = nalog.id,
                        PlataID = 1
                    });
                    break;
                case "2":
                    nalog.IsAdministrator = false; nalog.IsUposlenik = true; nalog.IsKlijent = false;
                    _db.Nalozi.Add(nalog);
                    _db.Uposlenici.Add(new Data.Models.Uposlenik
                    {
                        Ime = ime,
                        Prezime = prezime,
                        NalogID = nalog.id,
                        PlataID = 1,
                        TipUposlenikaID = int.Parse(tipUposlenika),
                        GradID=int.Parse(grad)
                    });
                    break;
                case "3":
                    nalog.IsAdministrator = false; nalog.IsUposlenik = false; nalog.IsKlijent = true;
                    _db.Nalozi.Add(nalog);
                    _db.Klijenti.Add(new Data.Models.Klijent
                    {
                        Ime = ime,
                        Prezime = prezime,
                        NalogID = nalog.id,
                        GradID = int.Parse(grad),
                        JBMG = jmbg,
                        Spol = spol
                    });
                    break;                    
            }

            _db.SaveChanges();

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Sportski centar ASA", "sportskicentarasa@gmail.com"));

            message.To.Add(new MailboxAddress(nalog.KorisnickoIme, nalog.email));

            message.Subject = "Aktivacija profila";

            message.Body = new TextPart("plain")
            {
                Text = "Poštovani,\n" +
                "Napravljen je novi profil na našem portalu sa vašim emailom, molimo vas da aktivirate vaš nalog klikom na link ispod:\n" +
                "https://sportskicentarasa.azurewebsites.net/Autentifikacija/emailActivation?nalogId=" + nalog.id
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("sportskicentarasa@gmail.com", "SportskiCentarASA123");
                client.Send(message);
                client.Disconnect(true);
            }

            return RedirectToAction("Dodaj");
        }
    }
}