using Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ServerAPI.Services;

namespace ServerAPI.Repositories.FileRepo;

// Gemmer og henter filer som binære data direkte i MongoDB
public class MongoFileRepository : IFileRepository
{
    private readonly IMongoCollection<FileDocument> _filer;

    // Henter fil-samlingen fra MongoDB via MongoDbService
    public MongoFileRepository(MongoDbService db)
    {
        _filer = db.GetFiler();
    }

    // Gemmer en fil i MongoDB og returnerer det unikke filnavn
    public string Add(IFormFile file)
    {
        // Laver et unikt filnavn ved at kombinere tidsstempel og originalt filnavn
        var uniqueName = $"{DateTime.Now.Ticks}_{file.FileName}";

        // Læser filens indhold ind i en MemoryStream
        using var ms = new MemoryStream();
        file.CopyTo(ms);

        // Opretter et FileDocument med filens data
        var doc = new FileDocument
        {
            FileName = uniqueName,
            ContentType = GetContentType(file.FileName) ?? "application/octet-stream",
            // Konverterer filens bytes til BsonBinaryData så MongoDB kan gemme dem
            Content = new BsonBinaryData(ms.ToArray())
        };

        // Gemmer dokumentet i MongoDB
        _filer.InsertOne(doc);

        // Returnerer filnavnet så det kan gemmes på projektet
        return uniqueName;
    }

    // Henter filens binære indhold fra MongoDB baseret på filnavn
    public byte[]? Get(string fileName)
    {
        // Finder dokumentet med det pågældende filnavn
        var doc = _filer.Find(f => f.FileName == fileName).FirstOrDefault();

        if (doc?.Content == null)
            return null;

        // Returnerer filens bytes
        return doc.Content.Bytes;
    }

    // Henter content-type for en bestemt fil (bruges når filen sendes tilbage til browseren)
    public string? GetContentType(string fileName)
    {
        // Bestemmer content-type ud fra filens endelse
        var ext = Path.GetExtension(fileName).ToLower();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }

    // Returnerer en liste af alle gemte filnavne
    public List<string> GetAllKeys()
    {
        // Henter alle dokumenter og trækker kun filnavnene ud
        return _filer.Find(FilterDefinition<FileDocument>.Empty)
            .ToList()
            .Select(f => f.FileName)
            .ToList();
    }

    // Sletter en fil fra MongoDB baseret på filnavn
    public void Delete(string fileName)
    {
        _filer.DeleteOne(f => f.FileName == fileName);
    }
}
