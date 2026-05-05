using Core.Models;

namespace ArchitektApp.Service;

public interface IBlokeretService
{
    Task<List<Blokeret>> GetAll();
    Task<Blokeret?> GetById(int id);
    Task<Blokeret?> Create(Blokeret blokeret);
    Task Delete(int id);
}
