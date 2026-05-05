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
            var response = await _client.GetAsync($"{Config.ServerUrl}/api/bruger/login?email={email}&password={password}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Bruger>();
            return null;
        }
        catch
        {
            return null;
        }
    }
}
