using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class RacunStavke
    {
        public int Id { get; set; }
        public int Kolicina { get; set; }
        public virtual Artikal Artikal { get; set; }
        [ForeignKey(nameof(ArtikalId))]
        public int ArtikalId { get; set; }
        public virtual Racun Racun { get; set; }
        [ForeignKey(nameof(RacunId))]
        public int RacunId { get; set; }
    }
}
