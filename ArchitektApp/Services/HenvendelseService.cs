using Core.Models;
using System.Net.Http.Json;

namespace ArchitektApp.Service;

// Håndterer kommunikation med API'et for booking-henvendelser fra kunder
public class HenvendelseService
{
    private readonly HttpClient _http;

    public HenvendelseService(HttpClient http)
    {
        _http = http;
    }

    // Henter alle ubehandlede booking-henvendelser fra API'et
    public async Task<List<Booking>> GetHenvendelser()
    {
        return await _http.GetFromJsonAsync<List<Booking>>("api/booking") ?? new();
    }

    // Sletter en henvendelse uden at oprette en aftale
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
            Kundemail = booking.Kundemail,
            KundeTlf = booking.KundeTlf,
            Oprettelse = DateTime.UtcNow
        };

        await _http.PostAsJsonAsync("api/aftale", aftale);
        await _http.DeleteAsync($"api/booking/{booking.Id}");
    }
}
