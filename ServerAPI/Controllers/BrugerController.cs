using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrugerController : ControllerBase
{
    private readonly IBrugerRepository _repository;

    // Henter repository via dependency injection
    public BrugerController(IBrugerRepository repository)
    {
        _repository = repository;
    }

    // Henter alle brugere
    [HttpGet]
    public async Task<List<Bruger>> GetAll()
    {
        return await _repository.GetAll();
    }

    // Henter én bruger baseret på id
    [HttpGet("{id}")]
    public async Task<Bruger?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    // Opretter en ny bruger
    [HttpPost]
    public async Task<Bruger> Create(Bruger bruger)
    {
        return await _repository.Create(bruger);
    }

    // Opdaterer en eksisterende bruger
    [HttpPut("{id}")]
    public async Task Update(int id, Bruger bruger)
    {
        await _repository.Update(id, bruger);
    }

    // Sletter en bruger
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _repository.Delete(id);
    }

    // Logger en bruger ind
    [HttpPost("login")]
    public async Task<Bruger?> Login(string email, string password)
    {
        return await _repository.Login(email, password);
    }
}
