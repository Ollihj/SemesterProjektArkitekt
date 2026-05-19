namespace Core.Models;

// Repræsenterer ét billede i et projekt med dets filnavn og tilhørende beskrivelse
public class BilledeItem
{
    // Filnavnet bruges til at hente billedet fra serveren via download-endpointet
    public string FileName { get; set; } = "";

    // Valgfri beskrivelse der vises ved siden af billedet på detaljeside
    public string Beskrivelse { get; set; } = "";
}
