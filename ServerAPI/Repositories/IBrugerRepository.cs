using Core.Models;

namespace ServerAPI.Repositories;

public interface IBrugerRepository
{
    List<Bruger> GetAll();
    Bruger? GetById(int id);
    void Add(Bruger bruger);
    void Update(Bruger bruger);
    void Delete(int id);
}
