namespace BiblioTarApp.Models;

public class LoginResponse
{
    public int Id { get; set; }
    public string Nev { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Beosztas { get; set; }
}
