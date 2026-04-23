using System.Collections.Generic;

namespace BiblioTarApp.DataContext.Entites
{
    public class Felhasznalo
    {
        public int Id { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string Jelszo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefonszam { get; set; }

        public bool Aktiv { get; set; } = true;

        public Beosztas Szerepkor { get; set; } = Beosztas.Regisztralt;

        public List<Lakcim> Lakcimek { get; set; } = new();
        public List<Foglalas> Foglalasok { get; set; } = new();
        public List<Kolcsonzes> Kolcsonzesek { get; set; } = new();
        public List<Buntetes> Buntetesek { get; set; } = new();

        public enum Beosztas
        {
            Regisztralt = 1,
            Konyvtaros = 2,
            Adminisztrator = 4
        }
    }
}