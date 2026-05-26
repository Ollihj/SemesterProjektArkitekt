using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

// Håndterer kommunikation med API'et for kalender-relaterede operationer
// Dækker både aftaler og blokerede datoer
public class KalenderService
{
    private readonly HttpClient _http;

    public KalenderService(HttpClient http)
    {
        _http = http;
    }

    // Henter alle aftaler der tilhører en bestemt bruger
    public async Task<List<Aftale>> GetAftaler(int brugerId)
    {
        return await _http.GetFromJsonAsync<List<Aftale>>($"api/aftale/bruger/{brugerId}") ?? new();
    }

    // Opretter en ny aftale i kalenderen
    public async Task OpretAftale(Aftale aftale)
    {
        await _http.PostAsJsonAsync("api/aftale", aftale);
    }

    // Sletter en aftale fra kalenderen baseret på id
    public async Task SletAftale(int id)
    {
        await _http.DeleteAsync($"api/aftale/{id}");
    }

    // Henter alle blokerede datoer — bruges til at grå datoer ud i booking-formularen
    public async Task<List<Blokeret>> GetBlokereteDatoer()
    {
        return await _http.GetFromJsonAsync<List<Blokeret>>("api/blokeret") ?? new();
    }

    // Blokerer en hel dag så kunder ikke kan booke den
    // DateTimeKind.Utc forhindrer MongoDB i at konvertere datoen fra lokal tid og gemme den forkert
    public async Task<Blokeret> BlokerDag(int brugerId, DateTime dag)
    {
        var utcDato = DateTime.SpecifyKind(dag.Date, DateTimeKind.Utc);
        var blokeret = new Blokeret
        {
            BrugerId = brugerId,
            StartTid = utcDato,
            SlutTid = utcDato.AddDays(1).AddSeconds(-1),
            Type = "dag"
        };
        await _http.PostAsJsonAsync("api/blokeret", blokeret);
        return blokeret;
    }

    // Fjerner en blokering så datoen igen er tilgængelig for booking
    public async Task FjernBlokering(int id)
    {
        await _http.DeleteAsync($"api/blokeret/{id}");
    }
}
