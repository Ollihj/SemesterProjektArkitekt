using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

public class ProjektService
{
    private readonly HttpClient _http;

    public ProjektService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Projekt>> GetProjekter()
    {
        return await _http.GetFromJsonAsync<List<Projekt>>("api/projekt") ?? new();
    }

    public async Task Opret(Projekt projekt)
    {
        await _http.PostAsJsonAsync("api/projekt", projekt);
    }

    public async Task Opdater(Projekt projekt)
    {
        await _http.PutAsJsonAsync($"api/projekt/{projekt.Id}", projekt);
    }

    public async Task Slet(int id)
    {
        await _http.DeleteAsync($"api/projekt/{id}");
    }
}
