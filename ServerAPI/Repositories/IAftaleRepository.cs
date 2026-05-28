using Core.Models;

namespace ServerAPI.Repositories;

public interface IAftaleRepository
{
    // Henter alle aftaler
    Task<List<Aftale>> GetAll();

    // Henter én aftale baseret på id
    Task<Aftale?> GetById(int id);

    // Henter alle aftaler der tilhører en bestemt bruger
    Task<List<Aftale>> GetByBrugerId(int brugerId);

    // Opretter en ny aftale
    Task<Aftale> Save(Aftale aftale);

    // Opdaterer en eksisterende aftale
    Task Update(int id, Aftale aftale);

    // Sletter en aftale
    Task Delete(int id);
}
