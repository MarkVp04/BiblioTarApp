namespace BiblioTarApp.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int FelhasznaloId { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Szerepkor { get; set; } = string.Empty;
    }
}