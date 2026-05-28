using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

// Håndterer kommunikation med API'et for projekt-relaterede operationer
public class ProjektService
{
    private readonly HttpClient _http;

    public ProjektService(HttpClient http)
    {
        _http = http;
    }

    // Henter alle projekter fra API'et
    public async Task<List<Projekt>> GetProjekter()
    {
        return await _http.GetFromJsonAsync<List<Projekt>>("api/projekt") ?? new();
    }

    // Opretter et nyt projekt — API returnerer den fulde liste direkte
    public async Task<List<Projekt>> PostProjekt(Projekt projekt)
    {
        var response = await _http.PostAsJsonAsync("api/projekt", projekt);
        return await response.Content.ReadFromJsonAsync<List<Projekt>>() ?? new();
    }

    // Opdaterer et eksisterende projekt
    public async Task Opdater(Projekt projekt)
    {
        await _http.PutAsJsonAsync($"api/projekt/{projekt.Id}", projekt);
    }

    // Sletter et projekt baseret på id
    public async Task Slet(int id)
    {
        await _http.DeleteAsync($"api/projekt/{id}");
    }
}
