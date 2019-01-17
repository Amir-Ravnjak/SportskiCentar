using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportskiCentar_ASA.Data.Models;
using Microsoft.AspNetCore.Http;

namespace SportskiCentar_ASA.Web.Helper
{
    public enum TipKorisnika
    {
        Administrator = 1,
        Uposlenik,
        Klijent
    }
    public class Autentifikacija
    {
        private const string _logiraniNalog = "logirani_nalog";
        private IHttpContextAccessor httpContextAccessor;

        public Autentifikacija(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public static void PokreniNovuSesiju(Nalog nalog, HttpContext context)
        {
            context.Session.SetJson(_logiraniNalog, nalog);
        }
        public static void OcistiSesiju(HttpContext httpContext) => httpContext.Session.SetJson(_logiraniNalog, null);
        public static Nalog GetLogiraniNalog(HttpContext context)
        {
            Nalog nalog = context.Session.GetJson<Nalog>(_logiraniNalog);  
            if (nalog != null)
            {
                return nalog;
            }
            PokreniNovuSesiju(nalog, context);
            return nalog;
        }
    }
}
