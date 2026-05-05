namespace Core.Models;

public class Aftale
{
    public int Id { get; set; }
    public int BrugerId { get; set; }
    public string? Titel { get; set; }
    public DateTime StartTid { get; set; }
    public DateTime SlutTid { get; set; }
    public string? Beskrivelse { get; set; }
    public string? Type { get; set; }
    public DateTime Oprettelse { get; set; }
}
