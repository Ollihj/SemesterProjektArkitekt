using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

public class MedarbejderService
{
    private readonly HttpClient _http;

    public MedarbejderService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Bruger>> GetMedarbejdere()
    {
        var alle = await _http.GetFromJsonAsync<List<Bruger>>("api/bruger") ?? new();
        return alle.Where(b => b.Rolle == "medarbejder").ToList();
    }

    public async Task Opret(Bruger bruger)
    {
        await _http.PostAsJsonAsync("api/bruger", bruger);
    }

    public async Task Opdater(Bruger bruger)
    {
        await _http.PutAsJsonAsync($"api/bruger/{bruger.Id}", bruger);
    }

    public async Task Slet(int id)
    {
        await _http.DeleteAsync($"api/bruger/{id}");
    }
}
