namespace Core.Models;

public class Blokeret
{
    public int Id { get; set; }
    public int BrugerId { get; set; }
    public DateTime StartTid { get; set; }
    public DateTime SlutTid { get; set; }
    public string? Type { get; set; }
    public DateTime Oprettelse { get; set; }
}
