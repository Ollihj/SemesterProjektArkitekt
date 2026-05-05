using System.Net.Http.Json;
using Core.Models;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

public class BrugerServiceHttp : IBrugerService
{
    private readonly HttpClient _client;

    public BrugerServiceHttp(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Bruger>> GetAll()
    {
        var data = await _client.GetFromJsonAsync<List<Bruger>>($"{Config.ServerUrl}/api/bruger");
        return data ?? new List<Bruger>();
    }

    public async Task<Bruger?> GetById(int id)
    {
        return await _client.GetFromJsonAsync<Bruger>($"{Config.ServerUrl}/api/bruger/{id}");
    }

    public async Task<Bruger?> Create(Bruger bruger)
    {
        var response = await _client.PostAsJsonAsync($"{Config.ServerUrl}/api/bruger", bruger);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Bruger>() : null;
    }

    public async Task<Bruger?> Update(Bruger bruger)
    {
        var response = await _client.PutAsJsonAsync($"{Config.ServerUrl}/api/bruger/{bruger.Id}", bruger);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Bruger>() : null;
    }

    public async Task Delete(int id)
    {
        await _client.DeleteAsync($"{Config.ServerUrl}/api/bruger/{id}");
    }
}
