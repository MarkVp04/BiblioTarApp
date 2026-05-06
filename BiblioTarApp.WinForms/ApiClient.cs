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
            var response = await Client.GetAsync("api/Konyv/getall");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<KonyvDto>>();
            }

            var errorLog = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Hiba: {response.StatusCode} - {errorLog}");

            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hálózati hiba: {ex.Message}");
            return null;
        }
    }
    public static async Task<bool> RegisterAsync(string nev, string email, string jelszo,string telefonszam, int szerepkor)
    {
        var registerData = new
        {
            Nev = nev,
            Email = email,
            Jelszo = jelszo,
            Telefonszam = telefonszam,
            Szerepkor = szerepkor
        };

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
    public static async Task<bool> CreateKonyvAsync(KonyvCreateDto ujKonyv)
    {
        try
        {
            var response = await Client.PostAsJsonAsync("api/Konyv/create", ujKonyv);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"Sikertelen létrehozás!\nStátuszkód: {response.StatusCode}\nÜzenet: {errorMsg}",
                            "API Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hálózati hiba: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    public static async Task<bool> UpdateKonyvAsync(int id, KonyvUpdateDto modositottKonyv)
    {
        try
        {
            modositottKonyv.Id = id;

            var response = await Client.PutAsJsonAsync("api/Konyv/update", modositottKonyv);

            if (response.IsSuccessStatusCode) return true;

            var errorMsg = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"Hiba: {response.StatusCode}\n{errorMsg}");
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hálózati hiba: {ex.Message}");
            return false;
        }
    }

    public static async Task<bool> DeleteKonyvAsync(int id)
    {
        try
        {
            var response = await Client.DeleteAsync($"api/Konyv/remove/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"Törlési hiba!\nStátuszkód: {(int)response.StatusCode} ({response.StatusCode})\nSzerver üzenete: {errorMsg}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hálózati hiba: {ex.Message}", "Kritikus hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    public static async Task<bool> CreateFoglalasAsync(int felhasznaloId, int konyvId, DateTime hatarido)
    {
        var foglalasAdat = new
        {
            FelhasznaloId = felhasznaloId,
            KonyvId = konyvId,
            Hatarido = hatarido
        };

        try
        {
            var response = await Client.PostAsJsonAsync("api/Foglalas/create", foglalasAdat);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<List<KolcsonzesGetDto>?> GetAllKolcsonzesAsync()
    {
        try
        {
            var response = await Client.GetAsync("api/Kolcsonzes/getall");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<KolcsonzesGetDto>>();
            return null;
        }
        catch (Exception ex) { MessageBox.Show($"Hiba: {ex.Message}"); return null; }
    }

    public static async Task<bool> CreateKolcsonzesAsync(int foglalasId)
    {
        try
        {
            var response = await Client.PostAsJsonAsync("api/Kolcsonzes/create", new { FoglalasId = foglalasId });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public static async Task<bool> UpdateKolcsonzesStatuszAsync(int kolcsonzesId, int statusz)
    {
        try
        {
            var response = await Client.PutAsJsonAsync("api/Kolcsonzes/status", new { Id = kolcsonzesId, Statusz = statusz });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public static async Task<bool> ExtendKolcsonzesAsync(int kolcsonzesId, DateTime ujHatarido)
    {
        try
        {
            var response = await Client.PutAsJsonAsync("api/Kolcsonzes/extend", new { Id = kolcsonzesId, UjHatarido = ujHatarido });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public static async Task<bool> CreateBuntetesAsync(int felhasznaloId, int foglalasId)
    {
        try
        {
            var response = await Client.PostAsJsonAsync("api/Buntetes/create", new { FelhasznaloId = felhasznaloId, FoglalasId = foglalasId });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
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
        public int MeghosszabbitasiLehetosegek { get; set; }
        public int Statusz { get; set; }
    }

    public static async Task<List<FoglalasGetDto>?> GetAllFoglalasAsync()
    {
        try
        {
            var response = await Client.GetAsync("api/Foglalas/getall");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<FoglalasGetDto>>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            MessageBox.Show($"API Hiba!\nStátusz: {response.StatusCode}\nÜzenet: {errorContent}",
                            "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kapcsolódási hiba: {ex.Message}", "Kritikus Hiba",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
    }

    public class FoglalasGetDto
    {
        public int Id { get; set; }
        public int FelhasznaloId { get; set; }
        public int KonyvId { get; set; }
        public string KonyvCim { get; set; } = string.Empty;
        public DateTime FoglalasIdeje { get; set; }
        public DateTime Hatarido { get; set; }
        public int Statusz { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int FelhasznaloId { get; set; }
        public string Nev { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Szerepkor { get; set; } = string.Empty;
    }
    //public record KonyvDto
    //{
    //    public int Id { get; set; }
    //    public string Cim { get; set; } = string.Empty;
    //    public string Szerzo { get; set; } = string.Empty;
    //    public string? Isbn { get; set; }
    //    public string Kategoria { get; set; } = string.Empty;
    //    public int Kiadasev { get; set; }
    //    public bool Kolcsonozheto { get; set; }
    //    public string Allapot { get; set; } = string.Empty;
    //}
}