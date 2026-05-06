using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _repository;

    // Henter repository via dependency injection
    public BookingController(IBookingRepository repository)
    {
        _repository = repository;
    }

    // Henter alle bookings
    [HttpGet]
    public async Task<List<Booking>> GetAll()
    {
        return await _repository.GetAll();
    }

    // Henter én booking baseret på id
    [HttpGet("{id}")]
    public async Task<Booking?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    // Henter alle bookings der tilhører en bestemt bruger
    [HttpGet("bruger/{brugerId}")]
    public async Task<List<Booking>> GetByBrugerId(int brugerId)
    {
        return await _repository.GetByBrugerId(brugerId);
    }

    // Opretter en ny booking
    [HttpPost]
    public async Task<Booking> Create(Booking booking)
    {
        return await _repository.Create(booking);
    }

    // Opdaterer en eksisterende booking
    [HttpPut("{id}")]
    public async Task Update(int id, Booking booking)
    {
        await _repository.Update(id, booking);
    }

    // Sletter en booking
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _repository.Delete(id);
    }
}
