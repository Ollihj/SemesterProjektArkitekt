using Core.Models;

namespace ServerAPI.Repositories;

public interface IProjektRepository
{
    List<Projekt> GetAll();
    Projekt? GetById(int id);
    void Add(Projekt projekt);
    void Update(Projekt projekt);
    void Delete(int id);
}
