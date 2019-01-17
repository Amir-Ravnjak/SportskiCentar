using Microsoft.EntityFrameworkCore;
using SportskiCentar_ASA.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.EF
{
    public class MojContext : DbContext
    {

            public MojContext(DbContextOptions<MojContext> options)
                : base(options)
            {
            }

        public MojContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                //modelBuilder.Entity<Uplata>()
                //.HasOne(x => x.Rezervacija)
                //.WithOne()
                //.HasForeignKey<Rezervacija>(x => x.UplataID);

                modelBuilder.Entity<Narudzba>()
                    .HasOne(pt => pt.Klijent)
                    .WithMany()
                    .HasForeignKey(pt => pt.KlijentID)
                    .OnDelete(DeleteBehavior.Restrict);
            }
            public DbSet<Fajl> Fajlovi { get; set; }
            public DbSet<ArtikliFajlovi> ArtikliFajlovi { get; set; }
            public DbSet<Administrator> Administratori { get; set; }
            public DbSet<Artikal> Artikli { get; set; }
            public DbSet<Dostava> Dostava { get; set; }
            public DbSet<Drzava> Drzave { get; set; }
            public DbSet<Grad> Gradovi { get; set; }
            public DbSet<Kategorija> Kategorije { get; set; }
            public DbSet<Klijent> Klijenti { get; set; }
            public DbSet<Racun> Racun { get; set; }
           public DbSet<RacunStavke> RacunStavke { get; set; }
            public DbSet<Nalog> Nalozi { get; set; }
            public DbSet<Narudzba> Narudzbe { get; set; }
            public DbSet<NarudzbaStavke> NarudzbaStavke { get; set; }
            public DbSet<Plata> Plate { get; set; }
            public DbSet<PodKategorija> PodKategorije { get; set; }
            public DbSet<Prostorija> Prostorije { get; set; }
            public DbSet<Rezervacija> Rezervacije { get; set; }
            public DbSet<Termin> Termini { get; set; }
            public DbSet<TipUposlenika> TipoviUposlenika { get; set; }
            public DbSet<Uplata> Uplate { get; set; }
            public DbSet<Uposlenik> Uposlenici { get; set; }
            public DbSet<UnlockToken> UnlockTokeni { get; set; }


    }
    
}
