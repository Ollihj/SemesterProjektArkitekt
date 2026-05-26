namespace Core.Models;

// Repræsenterer en henvendelse fra en kunde der ønsker et møde med arkitekten
// Når admin accepterer en booking oprettes der en Aftale og bookingen slettes
public class Booking
{
    public int Id { get; set; }

    // Id på den bruger (admin) der modtager bookingen
    public int BrugerId { get; set; }

    public string? Kundenavn { get; set; }
    public string? Kundemail { get; set; }
    public string? KundeTlf { get; set; }
    public string? Beskrivelse { get; set; }

    // Tidspunkt for hvornår bookingen blev oprettet
    public DateTime Oprettelse { get; set; }

    // Det tidspunkt kunden ønsker mødet afholdt
    public DateTime OensketDato { get; set; }
}
