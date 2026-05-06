using Core.Models;

namespace ServerAPI.Repositories;

public interface IBlokeretRepository
{
    // Henter alle blokerede tider
    Task<List<Blokeret>> GetAll();

    // Henter én blokeret tid baseret på id
    Task<Blokeret?> GetById(int id);

    // Henter alle blokerede tider der tilhører en bestemt bruger
    Task<List<Blokeret>> GetByBrugerId(int brugerId);

    // Opretter en ny blokeret tid
    Task<Blokeret> Create(Blokeret blokeret);

    // Opdaterer en eksisterende blokeret tid
    Task Update(int id, Blokeret blokeret);

    // Sletter en blokeret tid
    Task Delete(int id);
}
