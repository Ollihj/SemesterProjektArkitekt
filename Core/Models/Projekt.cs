namespace Core.Models;

// Repræsenterer et arkitektprojekt med billeder der vises på forsiden og projektssiden
public class Projekt
{
    public int Id { get; set; }

    // Id på den admin der ejer projektet
    public int BrugerId { get; set; }

    public string? Navn { get; set; }
    public string? Beskrivelse { get; set; }

    // Årstallet projektet blev udført — vises på projektsiden
    public int År { get; set; }

    // Hvert billede har sit eget filnavn og en valgfri beskrivelse
    public List<BilledeItem> Billeder { get; set; } = new();

    public DateTime Oprettelse { get; set; }
}
