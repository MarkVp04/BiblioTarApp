using BiblioTarApp.DataContext.Entites;

namespace BiblioTarApp.DTOs
{
    public class BuntetesCreateDto
    {
        public int FelhasznaloId { get; set; }
        public int FoglalasId { get; set; }
        public int Ar { get; set; }
    }

    public class BuntetesGetDto
    {
        public int Id { get; set; }
        public int FelhasznaloId { get; set; }
        public int FoglalasId { get; set; }
        public string KonyvCim { get; set; } = string.Empty;
        public int Ar { get; set; }
        public bool FizetesiStatusz { get; set; }
        public DateTime BuntetesIdeje { get; set; }
    }

    public class BuntetesFizetesiStatuszUpdateDto
    {
        public int Id { get; set; }
        public bool FizetesiStatusz { get; set; }
    }
}