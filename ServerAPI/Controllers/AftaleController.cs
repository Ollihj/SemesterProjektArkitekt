using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/aftale")]
public class AftaleController : ControllerBase
{
    private readonly IAftaleRepository _repository;

    public AftaleController(IAftaleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<List<Aftale>> GetAll() => Ok(_repository.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Aftale> GetById(int id)
    {
        var aftale = _repository.GetById(id);
        if (aftale == null) return NotFound();
        return Ok(aftale);
    }

    [HttpPost]
    public ActionResult<Aftale> Add([FromBody] Aftale aftale)
    {
        _repository.Add(aftale);
        return Ok(aftale);
    }

    [HttpPut("{id}")]
    public ActionResult<Aftale> Update(int id, [FromBody] Aftale aftale)
    {
        _repository.Update(aftale);
        return Ok(aftale);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _repository.Delete(id);
        return Ok();
    }
}
