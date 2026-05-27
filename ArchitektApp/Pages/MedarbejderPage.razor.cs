using Core.Models;
using Blazored.LocalStorage;
using ArchitektApp.Service;
using Microsoft.AspNetCore.Components;

namespace ArchitektApp.Pages;

// Code-behind til MedarbejderPage — håndterer kalender-logik for medarbejdere
public partial class MedarbejderPage
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] private KalenderService KalenderService { get; set; } = default!;

    private string brugerNavn = "";
    private int medarbejderId = 0;

    // Kalender-tilstand
    private int kalenderMåned = DateTime.Now.Month;
    private int kalenderÅr = DateTime.Now.Year;
    private List<Aftale> aftaler = new();
    private DateTime? valgtDag = null;
    private bool visOpretAftale = false;
    private Aftale nyAftale = new();
    private TimeOnly nyAftaleStartTid = new TimeOnly(9, 0);
    private TimeOnly nyAftaleSlutTid = new TimeOnly(10, 0);

    // Tjekker om brugeren er medarbejder og henter kalenderdata ved sideindlæsning
    protected override async Task OnInitializedAsync()
    {
        var userRole = await LocalStorage.GetItemAsync<string>("userRole");
        brugerNavn = await LocalStorage.GetItemAsync<string>("userName") ?? "";
        medarbejderId = await LocalStorage.GetItemAsync<int>("userId");

        // Kun medarbejdere må se denne side
        if (userRole != "medarbejder")
        {
            Navigation.NavigateTo("/");
            return;
        }

        aftaler = await KalenderService.GetAftaler(medarbejderId);
    }

    // Navigerer kalendervisningen én måned tilbage
    private void ForrigeMåned()
    {
        if (kalenderMåned == 1) { kalenderMåned = 12; kalenderÅr--; }
        else kalenderMåned--;
        valgtDag = null;
    }

    // Navigerer kalendervisningen én måned frem
    private void NæsteMåned()
    {
        if (kalenderMåned == 12) { kalenderMåned = 1; kalenderÅr++; }
        else kalenderMåned++;
        valgtDag = null;
    }

    // Hjælpemetode der sammenligner to datoer uden at tage tidspunktet i betragtning
    private static bool SammeDag(DateTime a, DateTime b) =>
        a.Year == b.Year && a.Month == b.Month && a.Day == b.Day;

    // Returnerer true hvis der er mindst én aftale på den givne dag
    private bool KalenderHarAftaler(DateTime dag) =>
        aftaler.Any(a => SammeDag(a.StartTid, dag));

    // Returnerer alle aftaler der falder på den givne dag
    private List<Aftale> KalenderAftalerForDag(DateTime dag) =>
        aftaler.Where(a => SammeDag(a.StartTid, dag)).ToList();

    // Sætter den valgte dag og nulstiller opret-aftale formularen
    private void VælgDag(DateTime dag)
    {
        valgtDag = dag;
        visOpretAftale = false;
        nyAftale = new Aftale { BrugerId = medarbejderId };
        nyAftaleStartTid = new TimeOnly(9, 0);
        nyAftaleSlutTid = new TimeOnly(10, 0);
    }

    // Viser formularen til at oprette en ny aftale på den valgte dag
    private void VisOpretAftaleForm()
    {
        visOpretAftale = true;
        nyAftale = new Aftale { BrugerId = medarbejderId };
    }

    // Kombinerer den valgte dag med tidspunkterne og gemmer aftalen
    private async Task GemAftale()
    {
        nyAftale.StartTid = valgtDag!.Value.Date + nyAftaleStartTid.ToTimeSpan();
        nyAftale.SlutTid = valgtDag!.Value.Date + nyAftaleSlutTid.ToTimeSpan();
        nyAftale.Oprettelse = DateTime.Now;

        await KalenderService.OpretAftale(nyAftale);
        aftaler = await KalenderService.GetAftaler(medarbejderId);
        visOpretAftale = false;
        nyAftale = new Aftale { BrugerId = medarbejderId };
    }

    // Sletter en aftale fra kalenderen baseret på id
    private async Task SletAftale(int id)
    {
        await KalenderService.SletAftale(id);
        aftaler = await KalenderService.GetAftaler(medarbejderId);
    }

    // Logger brugeren ud ved at fjerne alle session-data fra LocalStorage
    private async Task LogUd()
    {
        await LocalStorage.RemoveItemAsync("userId");
        await LocalStorage.RemoveItemAsync("userName");
        await LocalStorage.RemoveItemAsync("userRole");
        Navigation.NavigateTo("/");
    }
}
