namespace ServerAPI.Repositories.FileRepo;

// Interface der definerer hvad et file repository skal kunne
public interface IFileRepository
{
    // Gemmer en fil og returnerer det unikke filnavn der bruges til at hente den igen
    string Save(IFormFile file);

    // Henter en fils binære indhold baseret på filnavn
    byte[]? Get(string fileName);

    // Henter content-type (f.eks. "image/jpeg") for en bestemt fil
    string? GetContentType(string fileName);

    // Returnerer en liste af alle gemte filnavne
    List<string> GetAllKeys();

    // Sletter en fil baseret på filnavn
    void Delete(string fileName);
}
