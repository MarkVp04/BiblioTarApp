using BiblioTarApp.DataContext.Entites;

namespace BiblioTarApp.DTOs
{
    public class FelhasznaloCreateDto
    {
        public string Nev { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Jelszo { get; set; } = string.Empty;
        public string? Telefonszam { get; set; }
        public Felhasznalo.Beosztas Szerepkor { get; set; }
    }

    public class FelhasznaloLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Jelszo { get; set; } = string.Empty;
    }

    public class FelhasznaloGetDto
    {
        public int Id { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefonszam { get; set; }
        public bool Aktiv { get; set; }
        public Felhasznalo.Beosztas Szerepkor { get; set; }
        public List<LakcimGetDto> Lakcimek { get; set; } = new();
    }

    public class FelhasznaloUpdateDto
    {
        public int Id { get; set; }
        public string? Nev { get; set; }
        public string? Email { get; set; }
        public string? Telefonszam { get; set; }
    }

    public class FelhasznaloSzerepkorUpdateDto
    {
        public int Id { get; set; }
        public Felhasznalo.Beosztas Szerepkor { get; set; }
    }
}