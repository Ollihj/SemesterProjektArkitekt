using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

public class HenvendelseService
{
    private readonly HttpClient _http;

    public HenvendelseService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Booking>> GetHenvendelser()
    {
        return await _http.GetFromJsonAsync<List<Booking>>("api/booking") ?? new();
    }

    public async Task Afvis(int id)
    {
        await _http.DeleteAsync($"api/booking/{id}");
    }

    // Opretter en aftale ud fra bookingen og sletter derefter bookingen
    public async Task Accepter(Booking booking, int brugerId)
    {
        var aftale = new Aftale
        {
            BrugerId = brugerId,
            Titel = $"Møde med {booking.Kundenavn}",
            StartTid = DateTime.SpecifyKind(booking.OensketDato, DateTimeKind.Utc),
            SlutTid = DateTime.SpecifyKind(booking.OensketDato.AddHours(1), DateTimeKind.Utc),
            Beskrivelse = booking.Beskrivelse,
            Type = "booking",
            Oprettelse = DateTime.UtcNow
        };

        await _http.PostAsJsonAsync("api/aftale", aftale);
        await _http.DeleteAsync($"api/booking/{booking.Id}");
    }
}
