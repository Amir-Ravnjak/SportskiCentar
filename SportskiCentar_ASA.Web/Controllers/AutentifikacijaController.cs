using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using Microsoft.AspNetCore.Http;
using SportskiCentar_ASA.Web.ViewModels;
using SportskiCentar_ASA.Web.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;
using System.Security.Cryptography;
using System.Text;

namespace SportskiCentar_ASA.Web.Controllers
{
    public class AutentifikacijaController : Controller
    {
        
        private MojContext db;
        private IHttpContextAccessor httpContext;
        public AutentifikacijaController(MojContext db, IHttpContextAccessor httpContext)
        {
            this.db = db;
            this.httpContext = httpContext;
        }
        public IActionResult Prijava()
        {
            return View(new LoginVM());
        }
        [HttpPost]
        public IActionResult Prijava(LoginVM login)
        {
       
            if(!ModelState.IsValid)
            {
                return View(login);
            }
            Nalog nalog = db.Nalozi
                .Where(x => x.KorisnickoIme == login.Username && x.Lozinka == login.Password)
                .FirstOrDefault();

            if (nalog == null)
            {
                ModelState.AddModelError("", "Korisnicko ime ili lozinka nisu tacni");
            }
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            if (!nalog.emailActivated) {

                ModelState.AddModelError("", "Nalog nije aktiviran. Provjerite email za aktivacijski link.");
                return View(login);

            }
                                   
            if (nalog.IsUposlenik)
            {
                Autentifikacija.PokreniNovuSesiju(nalog, httpContext.HttpContext);
                return RedirectToAction("Index", "Home");
            }
            else if (nalog.IsKlijent)
            {
                Autentifikacija.PokreniNovuSesiju(nalog, httpContext.HttpContext);
                return RedirectToAction("Index", "Klijent/Rezervacije");
            }
            else
            {
                
               return Redirect("/Autentifikacija/tokenLogin?nalogId="+nalog.id);
            }

        }
        public IActionResult Registracija()
        {
            AutentifikacijaRegistracijaVM model = new AutentifikacijaRegistracijaVM();

        
            model.Gradovi = db.Gradovi.Select(s => new SelectListItem
            {
                Value = s.id.ToString(),
                Text = s.Naziv
            }).ToList();

            return View(model); 
        }
        [HttpPost]
        public IActionResult Registracija(AutentifikacijaRegistracijaVM klijent)
        {
            Nalog noviNalog = new Nalog {
                KorisnickoIme = klijent.KorisnickoIme,
                Lozinka = klijent.Password,
                IsAdministrator = false,
                IsKlijent = true,
                IsUposlenik = false,
                email = klijent.email,
                emailActivated = false
            };
         
            db.Nalozi.Add(noviNalog);

            Klijent noviKlijent = new Klijent {
                Ime=klijent.Ime,
                Prezime=klijent.Prezime,
                Spol=klijent.Spol,
                JBMG=klijent.JBMG,
                GradID=klijent.GradID,
                NalogID=noviNalog.id
            };
            db.Klijenti.Add(noviKlijent);
            db.SaveChanges();

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Sportski centar ASA", "sportskicentarasa@gmail.com"));

            message.To.Add(new MailboxAddress(noviKlijent.Ime, noviNalog.email));

            message.Subject = "Aktivacija profila";

            message.Body = new TextPart("plain")
            {
                Text = "Poštovani,\n" +
                "Hvala vam što ste se registrovali na naš portal, molimo vas da aktivirate vaš nalog klikom na link ispod:\n" +
                "https://sportskicentarasa.azurewebsites.net/Autentifikacija/emailActivation?nalogId=" + noviNalog.id
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("sportskicentarasa@gmail.com", "SportskiCentarASA123");
                client.Send(message);
                client.Disconnect(true);
            }
            if (!ModelState.IsValid)
            {
                return View(klijent);
            }
            return RedirectToAction("Prijava", "Autentifikacija");
        }
        public IActionResult Odjava()
        {
            Autentifikacija.OcistiSesiju(httpContext.HttpContext);
            return RedirectToAction(nameof(Prijava));
        }
        public IActionResult emailActivation(int nalogId)
        {

            Nalog nalog = db.Nalozi.Where(x => x.id == nalogId).FirstOrDefault();

            nalog.emailActivated = true;
            db.SaveChanges();           

            return RedirectToAction("Prijava","Autentifikacija");
        }

        public class TokenGenerator
        {
            public static string GetUniqueKey()
            {
                char[] chars =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                byte[] data = new byte[10];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetBytes(data);
                }
                StringBuilder result = new StringBuilder(10);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length)]);
                }
                return result.ToString();
            }
        }
        public IActionResult tokenLogin(int nalogId,int poruka)
        {

            UnlockToken neiskoristen = db.UnlockTokeni.Where(x => x.NalogID == nalogId && x.isUsed == false).FirstOrDefault();
            Nalog nalog = db.Nalozi.Where(x => x.id == nalogId).FirstOrDefault();


            if (neiskoristen == null) {
                string noviToken = TokenGenerator.GetUniqueKey();

                UnlockToken u = new UnlockToken {
                    NalogID = nalog.id,
                    isUsed = false,
                    token = noviToken,
                    DateCreated = DateTime.Now
                };

                db.UnlockTokeni.Add(u);

                db.SaveChanges();                

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Sportski centar ASA", "sportskicentarasa@gmail.com"));

                message.To.Add(new MailboxAddress(nalog.KorisnickoIme, nalog.email));

                message.Subject = "Vaš token za login";

                message.Body = new TextPart("plain")
                {
                    Text = "Poštovani,\n" +
                    "Molimo vas da nakon uspješnog logina unesete sljedeći token da biste pristupili daljim opcijama:\n" +
                    u.token
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("sportskicentarasa@gmail.com", "SportskiCentarASA123");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }

            AutentifikacijaTokenLoginVM model = new AutentifikacijaTokenLoginVM
            {
                nalogId = nalog.id                
            };
            if (poruka==1)
            {
                model.msg = "Token nije validan, molimo vas unesite token koji ste dobili na vašu email adresu.";
            }
            return View(model);
        }
        public IActionResult provjeriToken(string token,int nalogId)
        {
            UnlockToken t = db.UnlockTokeni.Where(x => x.NalogID == nalogId && x.isUsed == false).FirstOrDefault();

            Nalog nalog = db.Nalozi.Where(x => x.id == nalogId).FirstOrDefault();

            
            if (t.token==token)
            {
                t.isUsed = true;
                t.DateUsed = DateTime.Now;
                db.SaveChanges();
                Autentifikacija.PokreniNovuSesiju(nalog, httpContext.HttpContext);
                return RedirectToAction("Index", "Administrator/Admin");
            }
            else
            {
                return Redirect("/Autentifikacija/tokenLogin?nalogId=" + nalogId+"&poruka=1");
            }
            
        }
    }
}