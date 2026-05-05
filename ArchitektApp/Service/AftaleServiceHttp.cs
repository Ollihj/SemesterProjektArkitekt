using System.Net.Http.Json;
using Core.Models;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

public class AftaleServiceHttp : IAftaleService
{
    private readonly HttpClient _client;

    public AftaleServiceHttp(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Aftale>> GetAll()
    {
        var data = await _client.GetFromJsonAsync<List<Aftale>>($"{Config.ServerUrl}/api/aftale");
        return data ?? new List<Aftale>();
    }

    public async Task<Aftale?> GetById(int id)
    {
        return await _client.GetFromJsonAsync<Aftale>($"{Config.ServerUrl}/api/aftale/{id}");
    }

    public async Task<Aftale?> Create(Aftale aftale)
    {
        var response = await _client.PostAsJsonAsync($"{Config.ServerUrl}/api/aftale", aftale);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Aftale>() : null;
    }

    public async Task<Aftale?> Update(Aftale aftale)
    {
        var response = await _client.PutAsJsonAsync($"{Config.ServerUrl}/api/aftale/{aftale.Id}", aftale);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Aftale>() : null;
    }

    public async Task Delete(int id)
    {
        await _client.DeleteAsync($"{Config.ServerUrl}/api/aftale/{id}");
    }
}
