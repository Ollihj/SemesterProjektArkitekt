namespace Core.Models;

// Repræsenterer en aftale i adminens kalender
// Kan oprettes manuelt af admin eller automatisk når en booking accepteres
public class Aftale
{
    public int Id { get; set; }

    // Id på den admin der ejer aftalen
    public int BrugerId { get; set; }

    public string? Titel { get; set; }
    public DateTime StartTid { get; set; }
    public DateTime SlutTid { get; set; }
    public string? Beskrivelse { get; set; }

    // Bruges til at skelne mellem manuelle aftaler og dem der kom fra en booking ("booking")
    public string? Type { get; set; }

    // Udfyldes kun når aftalen er oprettet fra en booking
    public string? Kundemail { get; set; }
    public string? KundeTlf { get; set; }

    public DateTime Oprettelse { get; set; }
}
