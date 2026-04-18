using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTarApp.DataContext.Entites
{
    public class Lakcim
    {
        public int Id { get; set; }
        public int Iranyitoszam { get; set; }
        public string Varos { get; set; } = string.Empty;
        public string Utca { get; set; } = string.Empty;
        public string hazszam { get; set; } = string.Empty;

        [ForeignKey("Felhasznalo")]
        public int FelhasznaloId { get; set; }

        public Felhasznalo Felhasznalo { get; set; } = null!;
    }
}