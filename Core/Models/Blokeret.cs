namespace Core.Models;

// Repræsenterer en blokeret dato i kalenderen
// Blokerede datoer vises som utilgængelige i booking-formularen så kunder ikke kan vælge dem
public class Blokeret
{
    public int Id { get; set; }

    // Id på den admin der har blokeret datoen
    public int BrugerId { get; set; }

    // StartTid gemmes altid som UTC for at undgå timezone-fejl i MongoDB
    public DateTime StartTid { get; set; }

    // SlutTid sættes til slutningen af dagen (StartTid + 24 timer - 1 sekund)
    public DateTime SlutTid { get; set; }

    // Type bruges til at kategorisere blokeringen, fx "dag"
    public string? Type { get; set; }

    public DateTime Oprettelse { get; set; }
}
