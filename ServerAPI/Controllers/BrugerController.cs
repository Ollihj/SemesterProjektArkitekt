using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/bruger")]
public class BrugerController : ControllerBase
{
    private readonly IBrugerRepository _repository;

    public BrugerController(IBrugerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<List<Bruger>> GetAll() => Ok(_repository.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Bruger> GetById(int id)
    {
        var bruger = _repository.GetById(id);
        if (bruger == null) return NotFound();
        return Ok(bruger);
    }

    [HttpGet("login")]
    public ActionResult<Bruger> Login([FromQuery] string email, [FromQuery] string password)
    {
        var bruger = _repository.GetAll().FirstOrDefault(b => b.Email == email && b.Password == password);
        if (bruger == null) return Unauthorized();
        return Ok(bruger);
    }

    [HttpPost]
    public ActionResult<Bruger> Add([FromBody] Bruger bruger)
    {
        _repository.Add(bruger);
        return Ok(bruger);
    }

    [HttpPut("{id}")]
    public ActionResult<Bruger> Update(int id, [FromBody] Bruger bruger)
    {
        _repository.Update(bruger);
        return Ok(bruger);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _repository.Delete(id);
        return Ok();
    }
}
