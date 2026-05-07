


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

    public async Task<Bruger?> ValidLogin(string name, string password)
    {
        try
        {
            var response = await _client.GetAsync($"{Config.ServerUrl}/api/bruger/login?navn={name}&password={password}");
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

    public async Task<Bruger?> Register(string name, string password)
    {
        try
        {
            var newUser = new Bruger { Navn = name, Password = password, /*brugerID = Guid.NewGuid().ToString()*/ };
            var response = await _client.PostAsJsonAsync($"{Config.ServerUrl}/api/bruger/register", newUser);
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
