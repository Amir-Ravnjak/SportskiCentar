using System;
using System.Collections.Generic;
using System.Text;

namespace SportskiCentar_ASA.Data.Models
{
    public class UnlockToken
    {
        public int id { get; set; }
        public string token { get; set; }
        public int NalogID { get; set; }
        public virtual Nalog Nalog { get; set; }
        public DateTime DateCreated { get; set; }
        public bool isUsed { get; set; }
        public DateTime? DateUsed { get; set; }
    }
}
