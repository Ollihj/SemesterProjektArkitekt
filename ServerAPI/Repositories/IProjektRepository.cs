using Core.Models;

namespace ServerAPI.Repositories;

public interface IProjektRepository
{
    // Henter alle projekter
    Task<List<Projekt>> GetAll();

    // Henter ét projekt baseret på id
    Task<Projekt?> GetById(int id);

    // Henter alle projekter der tilhører en bestemt bruger
    Task<List<Projekt>> GetByBrugerId(int brugerId);

    // Opretter et nyt projekt
    Task<Projekt> Create(Projekt projekt);

    // Opdaterer et eksisterende projekt
    Task Update(int id, Projekt projekt);

    // Sletter et projekt
    Task Delete(int id);
}
