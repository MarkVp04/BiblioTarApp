using System.Net.Http.Headers;
using System.Net.Http.Json;
using BiblioTarApp.DTOs;
using BiblioTarApp.Services;

namespace BiblioTarApp.WinForms;

public static class ApiClient
{
    public static readonly HttpClient Client = new HttpClient();
    public static string? Token { get; private set; }

    static ApiClient()
    {
        Client.BaseAddress = new Uri("https://localhost:7007/");
    }

    public static async Task<LoginResponse?> LoginAsync(string email, string jelszo)
    {
        var loginData = new { Email = email, Jelszo = jelszo };

        try
        {
            var response = await Client.PostAsJsonAsync("api/Felhasznalo/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    Token = result.Token;
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    return result;
                }
            }
        }
        catch
        {
        }

        return null;
    }
    public static async Task<List<KonyvDto>?> GetKonyvekAsync()
    {
        try
        {

            var response = await Client.GetAsync("api/Konyv");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<KonyvDto>>();
            }
        }
        catch
        {
        }
        return null;
    }
    public static async Task<bool> RegisterAsync(string nev, string email, string jelszo)
    {
        var registerData = new { Nev = nev, Email = email, Jelszo = jelszo };

        try
        {
            var response = await Client.PostAsJsonAsync("api/Felhasznalo/create", registerData);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public int FelhasznaloId { get; set; }
    public string Nev { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Szerepkor { get; set; } = string.Empty;
}
public class KonyvDto
{
    public int Id { get; set; }
    public string Cim { get; set; } = string.Empty;
    public string Szerzo { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public string Kategoria { get; set; } = string.Empty;
    public int Kiadasev { get; set; }
    public bool Kolcsonozheto { get; set; }
    public string Allapot { get; set; } = string.Empty;
}