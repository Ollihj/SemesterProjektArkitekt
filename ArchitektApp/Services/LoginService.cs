using System.Net.Http.Json;
using Core.Models;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

// Håndterer login-kommunikation med API'et
public class LoginService
{
    private readonly HttpClient _client;

    public LoginService(HttpClient client)
    {
        _client = client;
    }

    // Forsøger at logge brugeren ind med email og password
    // Returnerer Bruger-objektet hvis login lykkedes, ellers null
    public async Task<Bruger?> ValidLogin(string email, string password)
    {
        try
        {
            // Sender POST request til login endpoint med email og password som query parametre
            var response = await _client.PostAsync($"{Config.ServerUrl}/api/bruger/login?email={email}&password={password}", null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Bruger>();
            }
            return null;
        }
        catch
        {
            // Returnerer null hvis serveren ikke kan nås eller svarer med fejl
            return null;
        }
    }
}