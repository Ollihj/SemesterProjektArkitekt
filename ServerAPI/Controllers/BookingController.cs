using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/booking")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _repository;

    public BookingController(IBookingRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<List<Booking>> GetAll() => Ok(_repository.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Booking> GetById(int id)
    {
        var booking = _repository.GetById(id);
        if (booking == null) return NotFound();
        return Ok(booking);
    }

    [HttpPost]
    public ActionResult<Booking> Add([FromBody] Booking booking)
    {
        _repository.Add(booking);
        return Ok(booking);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _repository.Delete(id);
        return Ok();
    }
}
