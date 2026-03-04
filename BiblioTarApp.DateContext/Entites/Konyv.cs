using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTarApp.DataContext.Entites
{
    public enum KonyvStatusz { 
        Elerheto = 1, NemElerheto = 2, Foglalt = 3
    }
    public enum KonyvAllapot { 
        Hibatlan = 1, Viseltes = 2, Serult = 3
    }

    public class Konyv
    {

        public int Id { get; set; }
        public string Cim { get; set; } = null!;
        public string Szerzo { get; set; } = null!;
        public string? Isbn { get; set; } 
        public string? Kategoria { get; set; }
        public int Kiadasev { get; set; } 
        public string? Allapot { get; set; }
        public string? Statusz { get; set; }
        public DateTime? PublikalasIdeje { get; set; }
        public bool Ertelekes { get; set; }
    }
}
