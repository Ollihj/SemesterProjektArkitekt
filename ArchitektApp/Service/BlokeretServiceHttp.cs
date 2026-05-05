using System.Net.Http.Json;
using Core.Models;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

public class BlokeretServiceHttp : IBlokeretService
{
    private readonly HttpClient _client;

    public BlokeretServiceHttp(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Blokeret>> GetAll()
    {
        var data = await _client.GetFromJsonAsync<List<Blokeret>>($"{Config.ServerUrl}/api/blokeret");
        return data ?? new List<Blokeret>();
    }

    public async Task<Blokeret?> GetById(int id)
    {
        return await _client.GetFromJsonAsync<Blokeret>($"{Config.ServerUrl}/api/blokeret/{id}");
    }

    public async Task<Blokeret?> Create(Blokeret blokeret)
    {
        var response = await _client.PostAsJsonAsync($"{Config.ServerUrl}/api/blokeret", blokeret);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Blokeret>() : null;
    }

    public async Task Delete(int id)
    {
        await _client.DeleteAsync($"{Config.ServerUrl}/api/blokeret/{id}");
    }
}
