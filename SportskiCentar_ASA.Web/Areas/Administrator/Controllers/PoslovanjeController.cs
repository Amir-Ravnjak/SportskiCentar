using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.Areas.Administrator.ViewModels;
using SportskiCentar_ASA.Web.Helper;

namespace SportskiCentar_ASA.Web.Areas.Administrator.Controllers
{
    [Autorizacija(false, TipKorisnika.Administrator)]
    [Area(nameof(Administrator))]
    public class PoslovanjeController : Controller
    {

        MojContext _db;
        public PoslovanjeController(MojContext db)
        {
            _db = db;
        }
        public IActionResult Shop()
        {
            PoslovanjeShopVM model = new PoslovanjeShopVM();



            model.racuni = _db.Racun.Select(x => new PoslovanjeShopVM.Racuni
            {
                DatumIzdavanja = x.DatumIzdavanja,
                Klijent = x.Klijent.Ime + " " + x.Klijent.Prezime + " (" + x.Klijent.Nalog.KorisnickoIme + ")",
                UkupanIznos = (float)x.UkupanIznos,
                Dostava = x.DostavaId,
                id = x.id
            }).ToList();

            model.artikli = _db.Artikli.Select(x => new PoslovanjeShopVM.Artikli {
                naziv = x.Naziv,
                cijena = (float)x.Cijena,
                kategorija = x.PodKategorija.Kategorija.Naziv,
                kolicina = x.Kolicina,
                podKategorija = x.PodKategorija.Naziv
            }).ToList();

            model.Klijenti = _db.Klijenti.Select(x => new SelectListItem {
                Text = x.Ime + " " + x.Prezime + " (" + x.Nalog.KorisnickoIme + ")",
                Value = x.id.ToString()
            }).ToList();

            return View(model);
        }
        public IActionResult PretrazivanjeRacuna(int? klijentId)
        {
            PoslovanjeShopVM model = new PoslovanjeShopVM();

            if (klijentId != null)
            {
                model.racuni = _db.Racun.Where(q => q.KlijentId == klijentId).Select(x => new PoslovanjeShopVM.Racuni
                {
                    DatumIzdavanja = x.DatumIzdavanja,
                    Klijent = x.Klijent.Ime + " " + x.Klijent.Prezime + " (" + x.Klijent.Nalog.KorisnickoIme + ")",
                    UkupanIznos = (float)x.UkupanIznos,
                    Dostava = x.DostavaId,
                    id = x.id
                }).ToList();
            }
            else
            {
                model.racuni = _db.Racun.Select(x => new PoslovanjeShopVM.Racuni
                {
                    DatumIzdavanja = x.DatumIzdavanja,
                    Klijent = x.Klijent.Ime + " " + x.Klijent.Prezime + " (" + x.Klijent.Nalog.KorisnickoIme + ")",
                    UkupanIznos = (float)x.UkupanIznos,
                    Dostava = x.DostavaId,
                    id = x.id
                }).ToList();
            }

            return View(model);
        }

        public IActionResult RacunDetalji(int id)
        {
            PoslovanjeRacunDetaljiVM model = _db.Racun.Where(q=>q.id==id).Select(x => new PoslovanjeRacunDetaljiVM {
                datumIzdavnaja = x.DatumIzdavanja,
                klijent = x.Klijent.Ime + " " + x.Klijent.Prezime + " (" + x.Klijent.Nalog.KorisnickoIme + ")",
                ukupanIznos = (float)x.UkupanIznos
            }).FirstOrDefault();

            model.racunStavke = _db.RacunStavke.Where(x => x.RacunId == id).Select(q=>new PoslovanjeRacunDetaljiVM.Row {
                artikalNaziv=q.Artikal.Naziv,
                artikalCijena=(float)q.Artikal.Cijena,
                kolicina=q.Kolicina
            }).ToList();
            

            return View(model);
        }

        public IActionResult Termini()
        {
            PoslovanjeTerminiVM model = new PoslovanjeTerminiVM();

            model.termini = _db.Termini.Select(x => new PoslovanjeTerminiVM.term {
                id = x.id,
                cijena = (float)x.Cijena,
                DatumIVrijeme = x.DatumIVrijeme,
                prostorija = x.Prostorija.Naziv
            }).ToList();

            model.rezervacije = _db.Rezervacije.Select(x => new PoslovanjeTerminiVM.rezerv {
                id = x.id,
                klijent = x.Klijent.Ime + " " + x.Klijent.Prezime + " (" + x.Klijent.Nalog.KorisnickoIme + ")",
                odobrena = x.Odobrena,
                terminId = x.TerminID
            }).ToList();

            foreach (var x in model.termini)
            {
                foreach (var q in model.rezervacije)
                {
                    if (x.id == q.terminId)
                        x.rezervacija = true;                }
                
            }

            return View(model);
        }

        public IActionResult TerminiAjax()
        {
            PoslovanjeTerminiAjaxVM model = new PoslovanjeTerminiAjaxVM();

            List<Termin> termins = _db.Termini.Where(x => x.DatumIVrijeme < DateTime.Now).Include(q => q.Prostorija).ToList();
            List<Rezervacija> rezervacijas = _db.Rezervacije.Where(x => x.Termin.DatumIVrijeme < DateTime.Now).Include(q=>q.Termin).ToList();
            model.PropaliTermini = new List<PoslovanjeTerminiAjaxVM.Term>();
            foreach (var x in termins)
            {
                bool pronadjen = false;
                foreach (var q in rezervacijas)
                {
                    if (q.TerminID == x.id)
                        pronadjen = true;
                }

                if (pronadjen == false)
                {
                    model.PropaliTermini.Add(_db.Termini.Where(z => z.id == x.id).Select(u => new PoslovanjeTerminiAjaxVM.Term {
                        DatumIVrijeme = u.DatumIVrijeme,
                        cijena = (float)u.Cijena,
                        Prostorija = u.Prostorija.Naziv
                    }).FirstOrDefault());
                }

            }

            termins = _db.Termini.Where(x => x.DatumIVrijeme > DateTime.Now).Include(q => q.Prostorija).ToList();
            rezervacijas = _db.Rezervacije.Where(x => x.Termin.DatumIVrijeme > DateTime.Now).Include(q => q.Termin).ToList();

            foreach (var x in termins)
            {
                bool pronadjen = false;
                foreach (var q in rezervacijas)
                {
                    if (q.TerminID == x.id)
                        pronadjen = true;
                }

                if (pronadjen == false)
                {
                    model.Termini = _db.Termini.Where(u=>u.id==x.id).Select(q => new PoslovanjeTerminiAjaxVM.Term
                    {
                        DatumIVrijeme = q.DatumIVrijeme,
                        cijena = (float)q.Cijena,
                        Prostorija = q.Prostorija.Naziv
                    }).ToList();
                }
            }
            return View(model);
        }

        public IActionResult RezervacijeAjax()
        {
            PoslovanjeRezervacijeAjaxVM model = new PoslovanjeRezervacijeAjaxVM();

            model.OdradjeneRezervacije = _db.Rezervacije.Where(q => q.Termin.DatumIVrijeme < DateTime.Now).Select(x => new PoslovanjeRezervacijeAjaxVM.Rez {
                DatumIVrijeme = x.Termin.DatumIVrijeme,
                cijena = (float)x.Termin.Cijena,
                klijent = x.Klijent.Ime + " " + x.Klijent.Prezime + " (" + x.Klijent.Nalog.KorisnickoIme + ")",
                odobrena = x.Odobrena,
                prostorija = x.Termin.Prostorija.Naziv
            }).ToList();

            model.Rezervacije= _db.Rezervacije.Where(q => q.Termin.DatumIVrijeme > DateTime.Now).Select(x => new PoslovanjeRezervacijeAjaxVM.Rez
            {
                DatumIVrijeme = x.Termin.DatumIVrijeme,
                cijena = (float)x.Termin.Cijena,
                klijent = x.Klijent.Ime + " " + x.Klijent.Prezime + " (" + x.Klijent.Nalog.KorisnickoIme + ")",
                odobrena = x.Odobrena,
                prostorija = x.Termin.Prostorija.Naziv
            }).ToList();

            return View(model);
        }

    }
}