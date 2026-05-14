using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models;

// Repræsenterer en fil gemt som binære data i MongoDB
// BsonIgnoreExtraElements fortæller MongoDB.Driver at ignorere _id feltet som MongoDB tilføjer automatisk
[BsonIgnoreExtraElements]
public class FileDocument
{
    // Unikt filnavn brugt som nøgle til at hente filen igen
    public string FileName { get; set; } = "";

    // MIME-type, f.eks. "image/jpeg" eller "image/png"
    public string ContentType { get; set; } = "";

    // Selve filens binære indhold gemt i MongoDB
    public BsonBinaryData? Content { get; set; }
}
