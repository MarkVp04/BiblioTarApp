namespace BiblioTarApp.DTOs
{
    public class LakcimCreateDto
    {
        public int Iranyitoszam { get; set; }
        public string Varos { get; set; } = string.Empty;
        public string Utca { get; set; } = string.Empty;
        public string Hazszam { get; set; } = string.Empty;
        public int FelhasznaloId { get; set; }
    }

    public class LakcimGetDto
    {
        public int Id { get; set; }
        public int Iranyitoszam { get; set; }
        public string Varos { get; set; } = string.Empty;
        public string Utca { get; set; } = string.Empty;
        public string Hazszam { get; set; } = string.Empty;
        public int FelhasznaloId { get; set; }
    }
}