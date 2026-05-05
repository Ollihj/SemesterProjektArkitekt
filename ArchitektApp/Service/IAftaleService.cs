using Core.Models;

namespace ArchitektApp.Service;

public interface IAftaleService
{
    Task<List<Aftale>> GetAll();
    Task<Aftale?> GetById(int id);
    Task<Aftale?> Create(Aftale aftale);
    Task<Aftale?> Update(Aftale aftale);
    Task Delete(int id);
}
