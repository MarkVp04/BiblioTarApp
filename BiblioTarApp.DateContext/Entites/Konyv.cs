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
        public KonyvStatusz Statusz { get; set; } = KonyvStatusz.Elerheto;
        public string Cim { get; set; }
        public string Szerzo { get; set; }
        public KonyvAllapot Allapot {  get; set; }
        public int Ertelekes { get; set; }
        public string Kategoria {  get; set; }
        public DateTime PublikalasIdeje { get; set; }
        //test2

    }
}
