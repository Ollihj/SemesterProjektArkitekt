


using System.Net.Http.Json;
using Core.Models;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

public class LoginService
{
    private readonly HttpClient _client;

    public LoginService(HttpClient client)
    {
        _client = client;
    }

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
            return null;
        }
    }

    public async Task<Bruger?> Register(string navn, string email, string password)
    {
        try
        {
            // Opretter en ny bruger og sender den til API'et
            var newUser = new Bruger { Navn = navn, Email = email, Password = password };
            var response = await _client.PostAsJsonAsync($"{Config.ServerUrl}/api/bruger", newUser);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Bruger>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
