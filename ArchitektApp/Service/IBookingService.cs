using Core.Models;

namespace ArchitektApp.Service;

public interface IBookingService
{
    Task<List<Booking>> GetAll();
    Task<Booking?> GetById(int id);
    Task<Booking?> Create(Booking booking);
    Task Delete(int id);
}
