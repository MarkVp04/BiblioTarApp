namespace BiblioTarApp.DTOs
{
    public class KonyvCreateDto
    {
        public int Id { get; set; }
        public string Cim { get; set; } = null!;
        public string Szerzo { get; set; } = null!;
        public string? Isbn { get; set; }
        public string? Kategoria { get; set; }
        public int Kiadasev { get; set; }
        public bool Kolcsonozheto { get; set; }

        public string? Allapot { get; set; }
        public string? Statusz { get; set; }

        public DateTime? PublikalasIdeje { get; set; }

        public bool Ertelekes { get; set; }
    }

    public class KonyvUpdateDto
    {
        public int Id { get; set; }

        public string Cim { get; set; } = null!;
        public string Szerzo { get; set; } = null!;
        public string? Isbn { get; set; }
        public string? Kategoria { get; set; }
        public int Kiadasev { get; set; }
        public bool Kolcsonozheto { get; set; }


        public string? Allapot { get; set; }
        public string? Statusz { get; set; }

        public DateTime? PublikalasIdeje { get; set; }

        public bool Ertelekes { get; set; }
    }

    public class KonyvGetDto
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

    public class KonyvDeleteDto
    {
        public int BookId { get; set; }
    }
    public record KonyvDto
    {
        public int Id { get; set; }
        public string Cim { get; set; } = string.Empty;
        public string Szerzo { get; set; } = string.Empty;
        public string? Isbn { get; set; }
        public string Kategoria { get; set; } = string.Empty;
        public int Kiadasev { get; set; }
        public bool Kolcsonozheto { get; set; }
        public string Allapot { get; set; } = string.Empty;
        public string? Statusz { get; set; }

    }

}