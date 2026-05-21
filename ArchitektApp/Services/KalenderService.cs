using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

public class KalenderService
{
    private readonly HttpClient _http;

    public KalenderService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Aftale>> GetAftaler(int brugerId)
    {
        return await _http.GetFromJsonAsync<List<Aftale>>($"api/aftale/bruger/{brugerId}") ?? new();
    }

    public async Task OpretAftale(Aftale aftale)
    {
        await _http.PostAsJsonAsync("api/aftale", aftale);
    }

    public async Task SletAftale(int id)
    {
        await _http.DeleteAsync($"api/aftale/{id}");
    }

    public async Task<List<Blokeret>> GetBlokereteDatoer()
    {
        return await _http.GetFromJsonAsync<List<Blokeret>>("api/blokeret") ?? new();
    }

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

    public async Task FjernBlokering(int id)
    {
        await _http.DeleteAsync($"api/blokeret/{id}");
    }
}
