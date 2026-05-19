namespace Core.Models;

public class Projekt
{
    public int Id { get; set; }
    public int BrugerId { get; set; }
    public string? Navn { get; set; }
    public string? Beskrivelse { get; set; }
    public int År { get; set; }
    // Hvert billede har sit eget filnavn og en valgfri beskrivelse
    public List<BilledeItem> Billeder { get; set; } = new();
    public DateTime Oprettelse { get; set; }
}
