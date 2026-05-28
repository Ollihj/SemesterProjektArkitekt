using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

// Håndterer kommunikation med API'et for medarbejder-relaterede operationer
// Kun brugere med rollen "medarbejder" hentes og håndteres af denne service
public class MedarbejderService
{
    private readonly HttpClient _http;

    public MedarbejderService(HttpClient http)
    {
        _http = http;
    }

    // Henter alle brugere fra API'et og filtrerer så kun medarbejdere returneres
    public async Task<List<Bruger>> GetMedarbejdere()
    {
        var alle = await _http.GetFromJsonAsync<List<Bruger>>("api/bruger") ?? new();
        return alle.Where(b => b.Rolle == "medarbejder").ToList();
    }

    // Opretter en ny medarbejder — API returnerer den fulde liste, vi filtrerer medarbejdere ud
    public async Task<List<Bruger>> PostBruger(Bruger bruger)
    {
        var response = await _http.PostAsJsonAsync("api/bruger", bruger);
        var medarbejderliste = await response.Content.ReadFromJsonAsync<List<Bruger>>() ?? new();
        return medarbejderliste.Where(b => b.Rolle == "medarbejder").ToList();
    }

    // Opdaterer en eksisterende medarbejder og returnerer den opdaterede liste
    public async Task<List<Bruger>> Opdater(Bruger bruger)
    {
        await _http.PutAsJsonAsync($"api/bruger/{bruger.Id}", bruger);
        return await GetMedarbejdere();
    }

    // Sletter en medarbejder og returnerer den opdaterede liste
    public async Task<List<Bruger>> Slet(int id)
    {
        await _http.DeleteAsync($"api/bruger/{id}");
        return await GetMedarbejdere();
    }
}
