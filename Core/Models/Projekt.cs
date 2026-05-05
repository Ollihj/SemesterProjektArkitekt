namespace Core.Models;

public class Projekt
{
    public int Id { get; set; }
    public int BrugerId { get; set; }
    public string? Navn { get; set; }
    public string? Beskrivelse { get; set; }
    public int År { get; set; }
    public string? Billede { get; set; }
    public DateTime Oprettelse { get; set; }
}
