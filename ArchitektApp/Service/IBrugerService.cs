using Core.Models;

namespace ArchitektApp.Service;

public interface IBrugerService
{
    Task<List<Bruger>> GetAll();
    Task<Bruger?> GetById(int id);
    Task<Bruger?> Create(Bruger bruger);
    Task<Bruger?> Update(Bruger bruger);
    Task Delete(int id);
}
