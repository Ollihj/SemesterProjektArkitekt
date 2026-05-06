using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AftaleController : ControllerBase
{
    private readonly IAftaleRepository _repository;

    // Henter repository via dependency injection
    public AftaleController(IAftaleRepository repository)
    {
        _repository = repository;
    }

    // Henter alle aftaler
    [HttpGet]
    public async Task<List<Aftale>> GetAll()
    {
        return await _repository.GetAll();
    }

    // Henter én aftale baseret på id
    [HttpGet("{id}")]
    public async Task<Aftale?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    // Henter alle aftaler der tilhører en bestemt bruger
    [HttpGet("bruger/{brugerId}")]
    public async Task<List<Aftale>> GetByBrugerId(int brugerId)
    {
        return await _repository.GetByBrugerId(brugerId);
    }

    // Opretter en ny aftale
    [HttpPost]
    public async Task<Aftale> Create(Aftale aftale)
    {
        return await _repository.Create(aftale);
    }

    // Opdaterer en eksisterende aftale
    [HttpPut("{id}")]
    public async Task Update(int id, Aftale aftale)
    {
        await _repository.Update(id, aftale);
    }

    // Sletter en aftale
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _repository.Delete(id);
    }
}
