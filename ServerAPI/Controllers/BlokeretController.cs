using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/blokeret")]
public class BlokeretController : ControllerBase
{
    private readonly IBlokeretRepository _repository;

    public BlokeretController(IBlokeretRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<List<Blokeret>> GetAll() => Ok(_repository.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Blokeret> GetById(int id)
    {
        var blokeret = _repository.GetById(id);
        if (blokeret == null) return NotFound();
        return Ok(blokeret);
    }

    [HttpPost]
    public ActionResult<Blokeret> Add([FromBody] Blokeret blokeret)
    {
        _repository.Add(blokeret);
        return Ok(blokeret);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _repository.Delete(id);
        return Ok();
    }
}
