using System.Net.Http.Json;
using Core.Models;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

public class ProjektServiceHttp : IProjektService
{
    private readonly HttpClient _client;

    public ProjektServiceHttp(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Projekt>> GetAll()
    {
        var data = await _client.GetFromJsonAsync<List<Projekt>>($"{Config.ServerUrl}/api/projekt");
        return data ?? new List<Projekt>();
    }

    public async Task<Projekt?> GetById(int id)
    {
        return await _client.GetFromJsonAsync<Projekt>($"{Config.ServerUrl}/api/projekt/{id}");
    }

    public async Task<Projekt?> Create(Projekt projekt)
    {
        var response = await _client.PostAsJsonAsync($"{Config.ServerUrl}/api/projekt", projekt);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Projekt>() : null;
    }

    public async Task<Projekt?> Update(Projekt projekt)
    {
        var response = await _client.PutAsJsonAsync($"{Config.ServerUrl}/api/projekt/{projekt.Id}", projekt);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Projekt>() : null;
    }

    public async Task Delete(int id)
    {
        await _client.DeleteAsync($"{Config.ServerUrl}/api/projekt/{id}");
    }
}
