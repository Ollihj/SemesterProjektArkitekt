using MongoDB.Bson;

namespace Core.Models;

// Repræsenterer en fil gemt som binære data i MongoDB
public class FileDocument
{
    // Unikt filnavn brugt som nøgle til at hente filen igen
    public string FileName { get; set; } = "";

    // MIME-type, f.eks. "image/jpeg" eller "image/png"
    public string ContentType { get; set; } = "";

    // Selve filens binære indhold gemt i MongoDB
    public BsonBinaryData? Content { get; set; }
}
