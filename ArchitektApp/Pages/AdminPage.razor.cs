using Core.Models;
using Blazored.LocalStorage;
using ArchitektApp.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ArchitektApp.Pages;

public partial class AdminPage
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] private FileService FileService { get; set; } = default!;
    [Inject] private MedarbejderService MedarbejderService { get; set; } = default!;
    [Inject] private KalenderService KalenderService { get; set; } = default!;
    [Inject] private HenvendelseService HenvendelseService { get; set; } = default!;
    [Inject] private ProjektService ProjektService { get; set; } = default!;

    private string aktivSektion = "henvendelser";
    private string brugerNavn = "";
    private int adminId = 0;

    // HENVENDELSER
    private List<Core.Models.Booking>? henvendelser;

    // MEDARBEJDERE
    private List<Bruger> medarbejdere = new();
    private bool visOpretMedarbejder = false;
    private bool erRedigeringMedarbejder = false;
    private Bruger nyMedarbejder = new();
    private string medarbejderFejl = "";

    // PROJEKTER
    private List<Projekt>? projekter;
    private bool visFormular = false;
    private bool erRedigering = false;
    private Projekt aktivProjekt = new();
    private bool uploaderBilleder = false;
    private string projektStatus = "";
    private string projektFejl = "";

    // KALENDER
    private int kalenderMåned = DateTime.Now.Month;
    private int kalenderÅr = DateTime.Now.Year;
    private List<Aftale> aftaler = new();
    private List<Core.Models.Blokeret> blokereteDatoer = new();
    private DateTime? valgtDag = null;
    private bool visOpretAftale = false;
    private Aftale nyAftale = new();
    private TimeOnly nyAftaleStartTid = new TimeOnly(9, 0);
    private TimeOnly nyAftaleSlutTid = new TimeOnly(10, 0);

    protected override async Task OnInitializedAsync()
    {
        var userRole = await LocalStorage.GetItemAsync<string>("userRole");
        brugerNavn = await LocalStorage.GetItemAsync<string>("userName") ?? "";
        adminId = await LocalStorage.GetItemAsync<int>("userId");

        if (userRole != "admin")
        {
            Navigation.NavigateTo("/");
            return;
        }

        henvendelser = await HenvendelseService.GetHenvendelser();
        medarbejdere = await MedarbejderService.GetMedarbejdere();
        projekter = await ProjektService.GetProjekter();
        aftaler = await KalenderService.GetAftaler(adminId);
        blokereteDatoer = await KalenderService.GetBlokereteDatoer();
    }

    private void SkiftSektion(string sektion)
    {
        aktivSektion = sektion;
        LukFormular();
    }

    // =====================
    // HENVENDELSER
    // =====================

    private async Task AccepterHenvendelse(Core.Models.Booking h)
    {
        await HenvendelseService.Accepter(h, adminId);

        henvendelser = await HenvendelseService.GetHenvendelser();
        aftaler = await KalenderService.GetAftaler(adminId);

        kalenderMåned = h.OensketDato.Month;
        kalenderÅr = h.OensketDato.Year;
        valgtDag = h.OensketDato;
        visOpretAftale = false;
        aktivSektion = "kalender";
    }

    private async Task AfvisHenvendelse(int id)
    {
        henvendelser?.RemoveAll(h => h.Id == id);
        await HenvendelseService.Afvis(id);
        henvendelser = await HenvendelseService.GetHenvendelser();
    }

    // =====================
    // MEDARBEJDERE
    // =====================

    private void VisOpretMedarbejderFormular()
    {
        nyMedarbejder = new Bruger { Rolle = "medarbejder" };
        erRedigeringMedarbejder = false;
        medarbejderFejl = "";
        visOpretMedarbejder = true;
    }

    private void VisRedigerMedarbejderFormular(Bruger m)
    {
        nyMedarbejder = new Bruger { Id = m.Id, Navn = m.Navn, Email = m.Email, Password = m.Password, Telefon = m.Telefon, Rolle = "medarbejder" };
        erRedigeringMedarbejder = true;
        medarbejderFejl = "";
        visOpretMedarbejder = true;
    }

    private void LukMedarbejderFormular()
    {
        visOpretMedarbejder = false;
        erRedigeringMedarbejder = false;
        medarbejderFejl = "";
    }

    private async Task GemMedarbejder()
    {
        medarbejderFejl = "";

        if (string.IsNullOrWhiteSpace(nyMedarbejder.Navn)) { medarbejderFejl = "Navn er påkrævet."; return; }
        if (string.IsNullOrWhiteSpace(nyMedarbejder.Email)) { medarbejderFejl = "Email er påkrævet."; return; }
        if (string.IsNullOrWhiteSpace(nyMedarbejder.Password)) { medarbejderFejl = "Kodeord er påkrævet."; return; }

        nyMedarbejder.Rolle = "medarbejder";

        if (erRedigeringMedarbejder)
            await MedarbejderService.Opdater(nyMedarbejder);
        else
            await MedarbejderService.Opret(nyMedarbejder);

        medarbejdere = await MedarbejderService.GetMedarbejdere();
        LukMedarbejderFormular();
    }

    private async Task SletMedarbejder(int id)
    {
        medarbejdere.RemoveAll(m => m.Id == id);
        await MedarbejderService.Slet(id);
        medarbejdere = await MedarbejderService.GetMedarbejdere();
    }

    // =====================
    // PROJEKTER
    // =====================

    private void VisOpretFormular()
    {
        aktivProjekt = new Projekt { BrugerId = adminId };
        erRedigering = false;
        projektStatus = "";
        projektFejl = "";
        visFormular = true;
    }

    private void VisRedigerFormular(Projekt p)
    {
        aktivProjekt = new Projekt
        {
            Id = p.Id, BrugerId = p.BrugerId, Navn = p.Navn, Beskrivelse = p.Beskrivelse, År = p.År,
            Billeder = p.Billeder.Select(b => new BilledeItem { FileName = b.FileName, Beskrivelse = b.Beskrivelse }).ToList(),
            Oprettelse = p.Oprettelse
        };
        erRedigering = true;
        projektStatus = "";
        projektFejl = "";
        visFormular = true;
    }

    private void LukFormular()
    {
        visFormular = false;
        projektStatus = "";
        projektFejl = "";
    }

    private async Task UploadBilleder(InputFileChangeEventArgs e)
    {
        uploaderBilleder = true;
        foreach (var file in e.GetMultipleFiles())
        {
            var stream = file.OpenReadStream(maxAllowedSize: 10_000_000);
            var (success, fileName) = await FileService.SendFile(file.Name, stream);
            if (success)
                aktivProjekt.Billeder.Add(new BilledeItem { FileName = fileName });
        }
        uploaderBilleder = false;
    }

    private async Task FjernBillede(BilledeItem billede)
    {
        aktivProjekt.Billeder.Remove(billede);
        await FileService.DeleteFile(billede.FileName);
    }

    private async Task GemProjekt()
    {
        projektFejl = "";

        if (string.IsNullOrWhiteSpace(aktivProjekt.Navn)) { projektFejl = "Projektet skal have et navn."; return; }
        if (aktivProjekt.År <= 0) { projektFejl = "Projektet skal have et årstal."; return; }
        if (!aktivProjekt.Billeder.Any()) { projektFejl = "Projektet skal have mindst ét billede."; return; }

        if (erRedigering)
        {
            await ProjektService.Opdater(aktivProjekt);
            projektStatus = "Projektet er opdateret.";
        }
        else
        {
            await ProjektService.Opret(aktivProjekt);
            projektStatus = "Projektet er oprettet.";
        }

        projekter = await ProjektService.GetProjekter();
        LukFormular();
    }

    private async Task SletProjekt(int id)
    {
        var projekt = projekter?.FirstOrDefault(p => p.Id == id);
        if (projekt != null)
            foreach (var billede in projekt.Billeder)
                await FileService.DeleteFile(billede.FileName);

        await ProjektService.Slet(id);
        projekter = await ProjektService.GetProjekter();
    }

    private async Task LogUd()
    {
        await LocalStorage.RemoveItemAsync("userId");
        await LocalStorage.RemoveItemAsync("userName");
        await LocalStorage.RemoveItemAsync("userRole");
        Navigation.NavigateTo("/");
    }

    // =====================
    // KALENDER
    // =====================

    private void ForrigeMåned()
    {
        if (kalenderMåned == 1) { kalenderMåned = 12; kalenderÅr--; }
        else kalenderMåned--;
        valgtDag = null;
    }

    private void NæsteMåned()
    {
        if (kalenderMåned == 12) { kalenderMåned = 1; kalenderÅr++; }
        else kalenderMåned++;
        valgtDag = null;
    }

    private static bool SammeDag(DateTime a, DateTime b) =>
        a.Year == b.Year && a.Month == b.Month && a.Day == b.Day;

    private bool KalenderHarAftaler(DateTime dag) => aftaler.Any(a => SammeDag(a.StartTid, dag));
    private bool ErDagBlokeret(DateTime dag) => blokereteDatoer.Any(b => SammeDag(b.StartTid, dag));
    private List<Aftale> KalenderAftalerForDag(DateTime dag) => aftaler.Where(a => SammeDag(a.StartTid, dag)).ToList();

    private void VælgDag(DateTime dag)
    {
        valgtDag = dag;
        visOpretAftale = false;
        nyAftale = new Aftale { BrugerId = adminId };
        nyAftaleStartTid = new TimeOnly(9, 0);
        nyAftaleSlutTid = new TimeOnly(10, 0);
    }

    private void VisOpretAftaleForm()
    {
        visOpretAftale = true;
        nyAftale = new Aftale { BrugerId = adminId };
    }

    private async Task GemAftale()
    {
        nyAftale.StartTid = valgtDag!.Value.Date + nyAftaleStartTid.ToTimeSpan();
        nyAftale.SlutTid = valgtDag!.Value.Date + nyAftaleSlutTid.ToTimeSpan();
        nyAftale.Oprettelse = DateTime.Now;

        await KalenderService.OpretAftale(nyAftale);
        aftaler = await KalenderService.GetAftaler(adminId);
        visOpretAftale = false;
        nyAftale = new Aftale { BrugerId = adminId };
    }

    private async Task SletAftale(int id)
    {
        await KalenderService.SletAftale(id);
        aftaler = await KalenderService.GetAftaler(adminId);
    }

    private async Task BlokerDag(DateTime dag)
    {
        var temp = await KalenderService.BlokerDag(adminId, dag);
        blokereteDatoer.Add(temp);
        blokereteDatoer = await KalenderService.GetBlokereteDatoer();
    }

    private async Task FjernBlokering(DateTime dag)
    {
        var blokeret = blokereteDatoer.FirstOrDefault(b => SammeDag(b.StartTid, dag));
        if (blokeret != null)
        {
            blokereteDatoer.Remove(blokeret);
            await KalenderService.FjernBlokering(blokeret.Id);
            blokereteDatoer = await KalenderService.GetBlokereteDatoer();
        }
    }
}
