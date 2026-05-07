using BiblioTarApp.DataContext.Entites;

namespace BiblioTarApp.DTOs
{
    public class KolcsonzesCreateDto
    {
        public int FoglalasId { get; set; }
    }

    public class KolcsonzesGetDto
    {
        public int Id { get; set; }
        public int? FelhasznaloId { get; set; }
        public string? Email { get; set; }
        public int KonyvId { get; set; }
        public string KonyvCim { get; set; } = string.Empty;
        public int FoglalasId { get; set; }
        public DateTime KolcsozesIdeje { get; set; }
        public DateTime? VisszahozasIdeje { get; set; }
        public DateTime Hatarido { get; set; }
        public int MeghosszabbitasiLehetosegek { get; set; } = 2;
        public KolcsonzesStatusz Statusz { get; set; }
    }

    public class KolcsonzesHosszabbitasDto
    {
        public int Id { get; set; }
        public DateTime UjHatarido { get; set; }
    }

    public class KolcsonzesStatuszUpdateDto
    {
        public int Id { get; set; }
        public KolcsonzesStatusz Statusz { get; set; }
    }
}