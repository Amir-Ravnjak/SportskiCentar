using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SportskiCentar_ASA.Data.Migrations
{
    public partial class inicijalna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dostava",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Adresa = table.Column<string>(nullable: true),
                    Cijena = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dostava", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Drzave",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drzave", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Fajlovi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumDodavanja = table.Column<DateTime>(nullable: false),
                    Naziv = table.Column<string>(nullable: true),
                    Podaci = table.Column<byte[]>(nullable: true),
                    Tip = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fajlovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Nalozi",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsAdministrator = table.Column<bool>(nullable: false),
                    IsKlijent = table.Column<bool>(nullable: false),
                    IsUposlenik = table.Column<bool>(nullable: false),
                    KorisnickoIme = table.Column<string>(nullable: true),
                    Lozinka = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    emailActivated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nalozi", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Plate",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Iznos = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Prostorije",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prostorije", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TipoviUposlenika",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviUposlenika", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Uplate",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Iznos = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uplate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DrzavaID = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.id);
                    table.ForeignKey(
                        name: "FK_Gradovi_Drzave_DrzavaID",
                        column: x => x.DrzavaID,
                        principalTable: "Drzave",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PodKategorije",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KategorijaID = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodKategorije", x => x.id);
                    table.ForeignKey(
                        name: "FK_PodKategorije_Kategorije_KategorijaID",
                        column: x => x.KategorijaID,
                        principalTable: "Kategorije",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnlockTokeni",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUsed = table.Column<DateTime>(nullable: true),
                    NalogID = table.Column<int>(nullable: false),
                    isUsed = table.Column<bool>(nullable: false),
                    token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlockTokeni", x => x.id);
                    table.ForeignKey(
                        name: "FK_UnlockTokeni_Nalozi_NalogID",
                        column: x => x.NalogID,
                        principalTable: "Nalozi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Administratori",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(nullable: true),
                    NalogID = table.Column<int>(nullable: false),
                    PlataID = table.Column<int>(nullable: false),
                    Prezime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administratori", x => x.id);
                    table.ForeignKey(
                        name: "FK_Administratori_Nalozi_NalogID",
                        column: x => x.NalogID,
                        principalTable: "Nalozi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Administratori_Plate_PlataID",
                        column: x => x.PlataID,
                        principalTable: "Plate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Termini",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cijena = table.Column<decimal>(nullable: false),
                    DatumIVrijeme = table.Column<DateTime>(nullable: false),
                    ProstorijaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termini", x => x.id);
                    table.ForeignKey(
                        name: "FK_Termini_Prostorije_ProstorijaID",
                        column: x => x.ProstorijaID,
                        principalTable: "Prostorije",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Klijenti",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GradID = table.Column<int>(nullable: false),
                    Ime = table.Column<string>(nullable: true),
                    JBMG = table.Column<string>(nullable: true),
                    NalogID = table.Column<int>(nullable: false),
                    Prezime = table.Column<string>(nullable: true),
                    Spol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klijenti", x => x.id);
                    table.ForeignKey(
                        name: "FK_Klijenti_Gradovi_GradID",
                        column: x => x.GradID,
                        principalTable: "Gradovi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Klijenti_Nalozi_NalogID",
                        column: x => x.NalogID,
                        principalTable: "Nalozi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Uposlenici",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GradID = table.Column<int>(nullable: false),
                    Ime = table.Column<string>(nullable: true),
                    NalogID = table.Column<int>(nullable: false),
                    PlataID = table.Column<int>(nullable: false),
                    Prezime = table.Column<string>(nullable: true),
                    TipUposlenikaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uposlenici", x => x.id);
                    table.ForeignKey(
                        name: "FK_Uposlenici_Gradovi_GradID",
                        column: x => x.GradID,
                        principalTable: "Gradovi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uposlenici_Nalozi_NalogID",
                        column: x => x.NalogID,
                        principalTable: "Nalozi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uposlenici_Plate_PlataID",
                        column: x => x.PlataID,
                        principalTable: "Plate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uposlenici_TipoviUposlenika_TipUposlenikaID",
                        column: x => x.TipUposlenikaID,
                        principalTable: "TipoviUposlenika",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artikli",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cijena = table.Column<decimal>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(nullable: false),
                    PodKategorijaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikli", x => x.id);
                    table.ForeignKey(
                        name: "FK_Artikli_PodKategorije_PodKategorijaID",
                        column: x => x.PodKategorijaID,
                        principalTable: "PodKategorije",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Narudzbe",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DostavaID = table.Column<int>(nullable: true),
                    KlijentID = table.Column<int>(nullable: true),
                    ZeliDostavu = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narudzbe", x => x.id);
                    table.ForeignKey(
                        name: "FK_Narudzbe_Dostava_DostavaID",
                        column: x => x.DostavaID,
                        principalTable: "Dostava",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Narudzbe_Klijenti_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "Klijenti",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KlijentID = table.Column<int>(nullable: false),
                    Odobrena = table.Column<bool>(nullable: false),
                    TerminID = table.Column<int>(nullable: false),
                    UplataID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Klijenti_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "Klijenti",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Termini_TerminID",
                        column: x => x.TerminID,
                        principalTable: "Termini",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Uplate_UplataID",
                        column: x => x.UplataID,
                        principalTable: "Uplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArtikliFajlovi",
                columns: table => new
                {
                    ArtikliFajloviId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArtikalID = table.Column<int>(nullable: false),
                    FajlID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikliFajlovi", x => x.ArtikliFajloviId);
                    table.ForeignKey(
                        name: "FK_ArtikliFajlovi_Artikli_ArtikalID",
                        column: x => x.ArtikalID,
                        principalTable: "Artikli",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtikliFajlovi_Fajlovi_FajlID",
                        column: x => x.FajlID,
                        principalTable: "Fajlovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NarudzbaStavke",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArtikalID = table.Column<int>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    NarudzbaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarudzbaStavke", x => x.id);
                    table.ForeignKey(
                        name: "FK_NarudzbaStavke_Artikli_ArtikalID",
                        column: x => x.ArtikalID,
                        principalTable: "Artikli",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NarudzbaStavke_Narudzbe_NarudzbaID",
                        column: x => x.NarudzbaID,
                        principalTable: "Narudzbe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Racun",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumIzdavanja = table.Column<DateTime>(nullable: false),
                    DostavaId = table.Column<int>(nullable: true),
                    KlijentId = table.Column<int>(nullable: false),
                    NarudzbaID = table.Column<int>(nullable: true),
                    UkupanIznos = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racun", x => x.id);
                    table.ForeignKey(
                        name: "FK_Racun_Dostava_DostavaId",
                        column: x => x.DostavaId,
                        principalTable: "Dostava",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Racun_Klijenti_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijenti",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Racun_Narudzbe_NarudzbaID",
                        column: x => x.NarudzbaID,
                        principalTable: "Narudzbe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RacunStavke",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArtikalId = table.Column<int>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false),
                    RacunId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunStavke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunStavke_Artikli_ArtikalId",
                        column: x => x.ArtikalId,
                        principalTable: "Artikli",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunStavke_Racun_RacunId",
                        column: x => x.RacunId,
                        principalTable: "Racun",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administratori_NalogID",
                table: "Administratori",
                column: "NalogID");

            migrationBuilder.CreateIndex(
                name: "IX_Administratori_PlataID",
                table: "Administratori",
                column: "PlataID");

            migrationBuilder.CreateIndex(
                name: "IX_Artikli_PodKategorijaID",
                table: "Artikli",
                column: "PodKategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikliFajlovi_ArtikalID",
                table: "ArtikliFajlovi",
                column: "ArtikalID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtikliFajlovi_FajlID",
                table: "ArtikliFajlovi",
                column: "FajlID");

            migrationBuilder.CreateIndex(
                name: "IX_Gradovi_DrzavaID",
                table: "Gradovi",
                column: "DrzavaID");

            migrationBuilder.CreateIndex(
                name: "IX_Klijenti_GradID",
                table: "Klijenti",
                column: "GradID");

            migrationBuilder.CreateIndex(
                name: "IX_Klijenti_NalogID",
                table: "Klijenti",
                column: "NalogID");

            migrationBuilder.CreateIndex(
                name: "IX_NarudzbaStavke_ArtikalID",
                table: "NarudzbaStavke",
                column: "ArtikalID");

            migrationBuilder.CreateIndex(
                name: "IX_NarudzbaStavke_NarudzbaID",
                table: "NarudzbaStavke",
                column: "NarudzbaID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbe_DostavaID",
                table: "Narudzbe",
                column: "DostavaID");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbe_KlijentID",
                table: "Narudzbe",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_PodKategorije_KategorijaID",
                table: "PodKategorije",
                column: "KategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Racun_DostavaId",
                table: "Racun",
                column: "DostavaId");

            migrationBuilder.CreateIndex(
                name: "IX_Racun_KlijentId",
                table: "Racun",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Racun_NarudzbaID",
                table: "Racun",
                column: "NarudzbaID");

            migrationBuilder.CreateIndex(
                name: "IX_RacunStavke_ArtikalId",
                table: "RacunStavke",
                column: "ArtikalId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunStavke_RacunId",
                table: "RacunStavke",
                column: "RacunId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_KlijentID",
                table: "Rezervacije",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_TerminID",
                table: "Rezervacije",
                column: "TerminID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_UplataID",
                table: "Rezervacije",
                column: "UplataID");

            migrationBuilder.CreateIndex(
                name: "IX_Termini_ProstorijaID",
                table: "Termini",
                column: "ProstorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_UnlockTokeni_NalogID",
                table: "UnlockTokeni",
                column: "NalogID");

            migrationBuilder.CreateIndex(
                name: "IX_Uposlenici_GradID",
                table: "Uposlenici",
                column: "GradID");

            migrationBuilder.CreateIndex(
                name: "IX_Uposlenici_NalogID",
                table: "Uposlenici",
                column: "NalogID");

            migrationBuilder.CreateIndex(
                name: "IX_Uposlenici_PlataID",
                table: "Uposlenici",
                column: "PlataID");

            migrationBuilder.CreateIndex(
                name: "IX_Uposlenici_TipUposlenikaID",
                table: "Uposlenici",
                column: "TipUposlenikaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administratori");

            migrationBuilder.DropTable(
                name: "ArtikliFajlovi");

            migrationBuilder.DropTable(
                name: "NarudzbaStavke");

            migrationBuilder.DropTable(
                name: "RacunStavke");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "UnlockTokeni");

            migrationBuilder.DropTable(
                name: "Uposlenici");

            migrationBuilder.DropTable(
                name: "Fajlovi");

            migrationBuilder.DropTable(
                name: "Artikli");

            migrationBuilder.DropTable(
                name: "Racun");

            migrationBuilder.DropTable(
                name: "Termini");

            migrationBuilder.DropTable(
                name: "Uplate");

            migrationBuilder.DropTable(
                name: "Plate");

            migrationBuilder.DropTable(
                name: "TipoviUposlenika");

            migrationBuilder.DropTable(
                name: "PodKategorije");

            migrationBuilder.DropTable(
                name: "Narudzbe");

            migrationBuilder.DropTable(
                name: "Prostorije");

            migrationBuilder.DropTable(
                name: "Kategorije");

            migrationBuilder.DropTable(
                name: "Dostava");

            migrationBuilder.DropTable(
                name: "Klijenti");

            migrationBuilder.DropTable(
                name: "Gradovi");

            migrationBuilder.DropTable(
                name: "Nalozi");

            migrationBuilder.DropTable(
                name: "Drzave");
        }
    }
}
