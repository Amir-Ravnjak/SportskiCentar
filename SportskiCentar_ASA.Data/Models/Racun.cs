using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class Racun
    {
        public int id { get; set; }
        public decimal UkupanIznos { get; set; }
        public int? NarudzbaID { get; set; }
        public Narudzba Narudzba { get; set; }        
        public virtual Klijent Klijent { get; set; }
        [ForeignKey(nameof(KlijentId))]
        public int KlijentId { get; set; }
        public DateTime DatumIzdavanja { get; set; }
        public virtual Dostava Dostava { get; set; }
        [ForeignKey(nameof(DostavaId))]
        public int? DostavaId { get; set; }
    }
}
