using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTarApp.DataContext.Entites
{
    public class Buntetes
    {
        public int Id { get; set; }
        [ForeignKey("Felhasznalo")]
        public int FelhasznaloId { get; set; }
        [ForeignKey("Foglalas")]
        public int FoglalasId { get; set; }
        public int Ar { get; set; }
        public bool FizetesiStatusz { get; set; } = false;
        public DateTime BuntetesIdeje { get; set; }

        public Foglalas Foglalas { get; set; }
        public Felhasznalo Felhasznalo { get; set; }
        //test4
    }
}
