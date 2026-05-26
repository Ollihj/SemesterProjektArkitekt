namespace Core.Models;

// Repræsenterer en bruger i systemet — kan være admin eller medarbejder
public class Bruger
{
    public int Id { get; set; }

    // Styrer adgangsniveau — "admin" har adgang til dashboard, "medarbejder" har begrænset adgang
    public string? Rolle { get; set; }

    public string? Navn { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Telefon { get; set; }
}
