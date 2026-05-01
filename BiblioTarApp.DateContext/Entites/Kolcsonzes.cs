using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTarApp.DataContext.Entites
{
    public enum KolcsonzesStatusz
    {
        Aktiv = 1,
        Teljesitett = 2,
        Torolve = 3
    }

    public class Kolcsonzes
    {
        public int Id { get; set; }
        public string? Email { get; set; }

        [ForeignKey("Felhasznalo")]
        public int? FelhasznaloId { get; set; }

        [ForeignKey("Konyv")]
        public int BookId { get; set; }

        [ForeignKey("Foglalas")]
        public int FoglalasId { get; set; }

        public DateTime KolcsozesIdeje { get; set; }
        public DateTime? VisszahozasIdeje { get; set; }
        public KolcsonzesStatusz Statusz { get; set; }

        public Felhasznalo? Felhasznalo { get; set; }
        public Konyv Konyv { get; set; } = null!;
        public Foglalas Foglalas { get; set; } = null!;
    }
}