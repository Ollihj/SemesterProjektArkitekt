using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/projekt")]
public class ProjektController : ControllerBase
{
    private readonly IProjektRepository _repository;

    public ProjektController(IProjektRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<List<Projekt>> GetAll() => Ok(_repository.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Projekt> GetById(int id)
    {
        var projekt = _repository.GetById(id);
        if (projekt == null) return NotFound();
        return Ok(projekt);
    }

    [HttpPost]
    public ActionResult<Projekt> Add([FromBody] Projekt projekt)
    {
        _repository.Add(projekt);
        return Ok(projekt);
    }

    [HttpPut("{id}")]
    public ActionResult<Projekt> Update(int id, [FromBody] Projekt projekt)
    {
        _repository.Update(projekt);
        return Ok(projekt);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _repository.Delete(id);
        return Ok();
    }
}
