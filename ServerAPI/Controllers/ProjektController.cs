using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjektController : ControllerBase
{
    private readonly IProjektRepository _repository;

    // Henter repository via dependency injection
    public ProjektController(IProjektRepository repository)
    {
        _repository = repository;
    }

    // Henter alle projekter
    [HttpGet]
    public async Task<List<Projekt>> GetAll()
    {
        return await _repository.GetAll();
    }

    // Henter ét projekt baseret på id
    [HttpGet("{id}")]
    public async Task<Projekt?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    // Henter alle projekter der tilhører en bestemt bruger
    [HttpGet("bruger/{brugerId}")]
    public async Task<List<Projekt>> GetByBrugerId(int brugerId)
    {
        return await _repository.GetByBrugerId(brugerId);
    }

    // Opretter et nyt projekt
    [HttpPost]
    public async Task<Projekt> Create(Projekt projekt)
    {
        return await _repository.Save(projekt);
    }

    // Opdaterer et eksisterende projekt
    [HttpPut("{id}")]
    public async Task Update(int id, Projekt projekt)
    {
        await _repository.Update(id, projekt);
    }

    // Sletter et projekt
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _repository.Delete(id);
    }
}
