using Core.Models;

namespace ServerAPI.Repositories;

public interface IAftaleRepository
{
    List<Aftale> GetAll();
    Aftale? GetById(int id);
    void Add(Aftale aftale);
    void Update(Aftale aftale);
    void Delete(int id);
}
