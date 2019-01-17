using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportskiCentar_ASA.Web.Helper;
using SportskiCentar_ASA.Web.Models;
using SportskiCentar_ASA.Data.EF;
using SportskiCentar_ASA.Data.Models;
using SportskiCentar_ASA.Web.ViewModels;
using static SportskiCentar_ASA.Web.ViewModels.OdobreneRezervacijeVM;
using Microsoft.EntityFrameworkCore;

namespace SportskiCentar_ASA.Web.Controllers
{
    [Autorizacija(true)]
    public class HomeController : Controller
    {
        private MojContext db;
        private IHttpContextAccessor httpContextAccessor;
        public HomeController(MojContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.db = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
           
                ViewData[Konfiguracija.LogiraniNalog] = httpContextAccessor
                                .HttpContext.Session.GetJson<Nalog>(Konfiguracija.LogiraniNalog);

                decimal zarada = 0;
                foreach (var item in db.Rezervacije.Where(w => w.Odobrena == true && w.UplataID != null))
                {
                    zarada += db.Uplate.FirstOrDefault(x => x.id == item.UplataID).Iznos;
                }

                HomeIndexViewModel vm = new HomeIndexViewModel()
                {
                    Informacije = new HomeIndexViewModel.Detalji()
                    {
                        UkupnoProizvoda = db.Artikli.Count(),
                        UkupnoUposlenika = db.Uposlenici.Count(),
                        UkupnoKlijenata = db.Klijenti.Count(),
                        UkupnaZarada = zarada

                    }
                };

                return View(vm);

           

        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region FullCalendar       
        public JsonResult GetCalendarData()
        {
            OdobreneRezervacijeVM vm = new OdobreneRezervacijeVM()
            {
                OdobreneRezervacije = this.LoadData()
            };
            return Json(vm.OdobreneRezervacije);
        }


        private List<OdRezervacije> LoadData()
        {
            List<OdRezervacije> OdRezervacije = new List<OdRezervacije>();


            OdRezervacije = db.Rezervacije.Include(k => k.Klijent).Include(x => x.Termin).ThenInclude(y => y.Prostorija).Where(i=>i.Odobrena==true).Select(x => new OdRezervacije()
            {
                Sr = x.id,
                Title = x.Klijent.Ime + " " + x.Klijent.Prezime,
                Start_Date = x.Termin.DatumIVrijeme.ToString("yyyy-MM-dd hh:mm"),
                End_Date = x.Termin.DatumIVrijeme.AddHours(1).ToString("yyyy-MM-dd hh:mm"),
                Prostorija=x.Termin.Prostorija.Naziv
            }).ToList();

            return OdRezervacije;
        }
        #endregion

    }
}
