using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories.FileRepo;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileRepository _fileRepo;

    // Henter file repository via dependency injection
    public FileController(IFileRepository fileRepo)
    {
        _fileRepo = fileRepo;
    }

    // Modtager en fil fra klienten og gemmer den i MongoDB
    // Returnerer det unikke filnavn som bruges til at hente filen igen
    [HttpPost("upload")]
    public IActionResult Upload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Ingen fil modtaget.");

        // Gemmer filen og får det unikke filnavn tilbage
        var fileName = _fileRepo.Save(file);

        // Returnerer filnavnet så klienten kan gemme det på projektet
        return Ok(fileName);
    }

    // Henter en fil fra MongoDB og sender den til browseren så den kan vises som billede
    [HttpGet("download/{fileName}")]
    public IActionResult Download(string fileName)
    {
        // Henter filens binære indhold fra MongoDB
        var bytes = _fileRepo.Get(fileName);

        if (bytes == null)
            return NotFound("Filen blev ikke fundet.");

        // Henter content-type så browseren ved det er et billede
        var contentType = _fileRepo.GetContentType(fileName) ?? "application/octet-stream";

        // Sender filen tilbage til browseren
        return File(bytes, contentType);
    }

    // Returnerer en liste af alle gemte filnavne
    [HttpGet("keys")]
    public IActionResult GetAllKeys()
    {
        var keys = _fileRepo.GetAllKeys();
        return Ok(keys);
    }

    // Sletter en fil fra MongoDB baseret på filnavn
    [HttpDelete("{fileName}")]
    public IActionResult Delete(string fileName)
    {
        _fileRepo.Delete(fileName);
        return Ok();
    }
}
