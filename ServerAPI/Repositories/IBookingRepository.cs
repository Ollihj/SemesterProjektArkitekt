using Core.Models;

namespace ServerAPI.Repositories;

public interface IBookingRepository
{
    List<Booking> GetAll();
    Booking? GetById(int id);
    void Add(Booking booking);
    void Delete(int id);
}
