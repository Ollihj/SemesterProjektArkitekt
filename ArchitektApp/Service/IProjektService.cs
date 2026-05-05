using Core.Models;

namespace ArchitektApp.Service;

public interface IProjektService
{
    Task<List<Projekt>> GetAll();
    Task<Projekt?> GetById(int id);
    Task<Projekt?> Create(Projekt projekt);
    Task<Projekt?> Update(Projekt projekt);
    Task Delete(int id);
}
