using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTarApp.DataContext.Entites
{
    public enum KolcsonzesStatusz {
        Aktiv = 1, Teljesitett = 2, Torolve = 3
    }
    public class Kolcsonzes
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        [ForeignKey("Felhasznalo")]
        public int? FelhasznaloId { get; set; }
        [ForeignKey("Konyv")]
        public int BookId { get; set; }
        public DateTime KolcsozesIdeje { get; set; }
        public KolcsonzesStatusz Statusz { get; set; }
        public Felhasznalo Felhasznalo { get; set; }
        public Konyv Konyv { get; set; }
        
    }
}
