using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTarApp.DataContext.Entites
{
    public enum FoglalasStatusz
    {
        Foglalt = 1,
        Visszahozott = 2,
        Torolve = 3
    }

    public class Foglalas
    {
        public int Id { get; set; }

        [ForeignKey("Felhasznalo")]
        public int FelhasznaloId { get; set; }

        [ForeignKey("Konyv")]
        public int KonyvId { get; set; }

        public DateTime FoglalasIdeje { get; set; }
        public DateTime Hatarido { get; set; }
        public int MeghosszabbitasiLehetosegek { get; set; } = 2;
        public FoglalasStatusz Statusz { get; set; }

        public Felhasznalo Felhasznalo { get; set; } = null!;
        public Konyv Konyv { get; set; } = null!;
        public Buntetes? Szamla { get; set; }
        public Kolcsonzes? Kolcsonzes { get; set; }
    }
}