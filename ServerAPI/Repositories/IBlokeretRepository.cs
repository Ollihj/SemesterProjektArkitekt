using Core.Models;

namespace ServerAPI.Repositories;

public interface IBlokeretRepository
{
    List<Blokeret> GetAll();
    Blokeret? GetById(int id);
    void Add(Blokeret blokeret);
    void Delete(int id);
}
