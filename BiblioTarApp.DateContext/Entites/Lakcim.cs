using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTarApp.DataContext.Entites
{
    public class Lakcim
    {
        public int Id { get; set; }
        public int Iranyitoszam { get; set; }
        public string Varos { get; set; }
        public string Utca { get; set; }
        public string hazszam { get; set; }
        [ForeignKey("Felhasznalo")]
        public int? FelhasznaloId { get; set; }
        public Felhasznalo Felhasznalo { get; set; }
        
    }
}
