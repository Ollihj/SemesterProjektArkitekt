namespace Core.Models;

public class Booking
{
    public int Id { get; set; }
    public int BrugerId { get; set; }
    public string? Kundenavn { get; set; }
    public string? Kundemail { get; set; }
    public string? KundeTlf { get; set; }
    public string? Beskrivelse { get; set; }
    public DateTime Oprettelse { get; set; }
    public DateTime ØnsketDato { get; set; }
}
