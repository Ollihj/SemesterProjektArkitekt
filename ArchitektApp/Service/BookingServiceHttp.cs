using System.Net.Http.Json;
using Core.Models;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

public class BookingServiceHttp : IBookingService
{
    private readonly HttpClient _client;

    public BookingServiceHttp(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Booking>> GetAll()
    {
        var data = await _client.GetFromJsonAsync<List<Booking>>($"{Config.ServerUrl}/api/booking");
        return data ?? new List<Booking>();
    }

    public async Task<Booking?> GetById(int id)
    {
        return await _client.GetFromJsonAsync<Booking>($"{Config.ServerUrl}/api/booking/{id}");
    }

    public async Task<Booking?> Create(Booking booking)
    {
        var response = await _client.PostAsJsonAsync($"{Config.ServerUrl}/api/booking", booking);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Booking>() : null;
    }

    public async Task Delete(int id)
    {
        await _client.DeleteAsync($"{Config.ServerUrl}/api/booking/{id}");
    }
}
