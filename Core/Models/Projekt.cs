namespace Core.Models;

public class Projekt
{
    public int Id { get; set; }
    public int BrugerId { get; set; }
    public string? Navn { get; set; }
    public string? Beskrivelse { get; set; }
    public int År { get; set; }
    // Liste af filnavne der bruges til at hente billederne fra serveren
    public List<string> Billeder { get; set; } = new();
    public DateTime Oprettelse { get; set; }
}
