using BiblioTarApp.DataContext.Entites;

namespace BiblioTarApp.DTOs
{
    public class FoglalasCreateDto
    {
        public int FelhasznaloId { get; set; }
        public int KonyvId { get; set; }
        public DateTime Hatarido { get; set; }
    }

    public class FoglalasGetDto
    {
        public int Id { get; set; }
        public int FelhasznaloId { get; set; }
        public int KonyvId { get; set; }
        public string KonyvCim { get; set; } = string.Empty;
        public DateTime FoglalasIdeje { get; set; }
        public DateTime Hatarido { get; set; }
        public int MeghosszabbitasiLehetosegek { get; set; }
        public FoglalasStatusz Statusz { get; set; }
    }

    public class FoglalasStatuszUpdateDto
    {
        public int Id { get; set; }
        public FoglalasStatusz Statusz { get; set; }
    }
}