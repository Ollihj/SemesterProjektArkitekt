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

    // Opretter en ny medarbejder i databasen via API'et
    public async Task Opret(Bruger bruger)
    {
        await _http.PostAsJsonAsync("api/bruger", bruger);
    }

    // Opdaterer en eksisterende medarbejders oplysninger
    public async Task Opdater(Bruger bruger)
    {
        await _http.PutAsJsonAsync($"api/bruger/{bruger.Id}", bruger);
    }

    // Sletter en medarbejder fra databasen baseret på id
    public async Task Slet(int id)
    {
        await _http.DeleteAsync($"api/bruger/{id}");
    }
}
