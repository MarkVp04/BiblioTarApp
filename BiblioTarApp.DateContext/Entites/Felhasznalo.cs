using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTarApp.DataContext.Entites
{
    public class Felhasznalo
    {
        public int Id {  get; set; }
        public string Nev { get; set; }
        public string Jelszo { get; set; }
        public string Email { get; set; }
        public List<Lakcim> Lakcim { get; set; }
        public enum Beosztas { 
            Regisztralt = 1,
            Konyvtaros = 2,
            Adminisztrator = 4
        }
        //test2

    }
}
