using Core.Models;

namespace ServerAPI.Repositories;

public interface IBookingRepository
{
    // Henter alle bookings
    Task<List<Booking>> GetAll();

    // Henter én booking baseret på id
    Task<Booking?> GetById(int id);

    // Henter alle bookings der tilhører en bestemt bruger
    Task<List<Booking>> GetByBrugerId(int brugerId);

    // Opretter en ny booking
    Task<Booking> Save(Booking booking);

    // Opdaterer en eksisterende booking
    Task Update(int id, Booking booking);

    // Sletter en booking
    Task Delete(int id);
}
