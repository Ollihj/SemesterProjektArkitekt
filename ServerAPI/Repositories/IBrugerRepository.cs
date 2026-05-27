using Core.Models;

namespace ServerAPI.Repositories;

public interface IBrugerRepository
{
    // Henter alle brugere
    Task<List<Bruger>> GetAll();

    // Henter én bruger baseret på id
    Task<Bruger?> GetById(int id);

    // Opretter en ny bruger
    Task<Bruger> Save(Bruger bruger);

    // Opdaterer en eksisterende bruger
    Task Update(int id, Bruger bruger);

    // Sletter en bruger
    Task Delete(int id);

    // Tjekker om email og password matcher
    Task<Bruger?> Login(string email, string password);
}
