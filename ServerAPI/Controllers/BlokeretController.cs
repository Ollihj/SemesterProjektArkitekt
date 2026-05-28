using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlokeretController : ControllerBase
{
    private readonly IBlokeretRepository _repository;

    // Henter repository via dependency injection
    public BlokeretController(IBlokeretRepository repository)
    {
        _repository = repository;
    }

    // Henter alle blokerede tider
    [HttpGet]
    public async Task<List<Blokeret>> GetAll()
    {
        return await _repository.GetAll();
    }

    // Henter én blokeret tid baseret på id
    [HttpGet("{id}")]
    public async Task<Blokeret?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    // Henter alle blokerede tider der tilhører en bestemt bruger
    [HttpGet("bruger/{brugerId}")]
    public async Task<List<Blokeret>> GetByBrugerId(int brugerId)
    {
        return await _repository.GetByBrugerId(brugerId);
    }

    // Opretter en ny blokeret tid
    [HttpPost]
    public async Task<Blokeret> Create(Blokeret blokeret)
    {
        return await _repository.Save(blokeret);
    }

    // Opdaterer en eksisterende blokeret tid
    [HttpPut("{id}")]
    public async Task Update(int id, Blokeret blokeret)
    {
        await _repository.Update(id, blokeret);
    }

    // Sletter en blokeret tid
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _repository.Delete(id);
    }
}
