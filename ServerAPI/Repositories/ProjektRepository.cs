using Core.Models;
using MongoDB.Driver;
using ServerAPI.Services;

namespace ServerAPI.Repositories;

public class ProjektRepository : IProjektRepository
{
    private readonly IMongoCollection<Projekt> _projekter;

    // Henter projekter-samlingen fra MongoDB via MongoDbService
    public ProjektRepository(MongoDbService db)
    {
        _projekter = db.GetProjekter();
    }

    // Henter alle projekter fra databasen
    public async Task<List<Projekt>> GetAll()
    {
        // Henter alle projekter fra MongoDB og returnerer dem
        List<Projekt> alleProjekter = await _projekter.Find(FilterDefinition<Projekt>.Empty).ToListAsync();
        return alleProjekter;
    }

    // Henter ét projekt baseret på id
    public async Task<Projekt?> GetById(int id)
    {
        // Henter alle projekter fra MongoDB
        List<Projekt> alleProjekter = await _projekter.Find(FilterDefinition<Projekt>.Empty).ToListAsync();

        // Går igennem alle projekter og finder det med det rigtige id
        foreach (Projekt projekt in alleProjekter)
        {
            if (projekt.Id == id)
            {
                // Returnerer projektet når det er fundet
                return projekt;
            }
        }

        // Returnerer null hvis intet projekt havde det id
        return null;
    }

    // Henter alle projekter der tilhører en bestemt bruger
    public async Task<List<Projekt>> GetByBrugerId(int brugerId)
    {
        // Henter alle projekter fra MongoDB
        List<Projekt> alleProjekter = await _projekter.Find(FilterDefinition<Projekt>.Empty).ToListAsync();
        // Liste til at samle de projekter der tilhører brugeren
        List<Projekt> brugerProjekter = new List<Projekt>();

        // Går igennem alle projekter og tilføjer dem der har det rigtige brugerId
        foreach (Projekt projekt in alleProjekter)
        {
            if (projekt.BrugerId == brugerId)
            {
                brugerProjekter.Add(projekt);
            }
        }

        // Returnerer listen med brugerens projekter
        return brugerProjekter;
    }

    // Opretter et nyt projekt, giver det næste ledige id og returnerer den opdaterede liste
    public async Task<List<Projekt>> Save(Projekt projekt)
    {
        // Henter alle eksisterende projekter for at finde det højeste id
        List<Projekt> alleProjekter = await _projekter.Find(FilterDefinition<Projekt>.Empty).ToListAsync();

        if (alleProjekter.Count == 0)
        {
            // Hvis der ingen projekter er, starter vi fra 1
            projekt.Id = 1;
        }
        else
        {
            // Finder det højeste id blandt alle projekter
            int maxId = 0;
            foreach (Projekt p in alleProjekter)
            {
                if (p.Id > maxId)
                {
                    maxId = p.Id;
                }
            }
            // Giver det nye projekt et id der er én højere end det højeste
            projekt.Id = maxId + 1;
        }

        // Sætter oprettelsesdatoen til nu
        projekt.Oprettelse = DateTime.Now;
        // Gemmer det nye projekt i MongoDB
        await _projekter.InsertOneAsync(projekt);
        // Returnerer den fulde opdaterede liste
        return await GetAll();
    }

    // Opdaterer et eksisterende projekt med nye data
    public async Task Update(int id, Projekt projekt)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Projekt>.Filter.Eq("_id", id);
        // Sætter id på det nye objekt så det ikke mistes ved erstatning
        projekt.Id = id;
        // Erstatter det gamle dokument med det nye
        await _projekter.ReplaceOneAsync(filter, projekt);
    }

    // Sletter et projekt fra databasen
    public async Task Delete(int id)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Projekt>.Filter.Eq("_id", id);
        // Sletter dokumentet fra MongoDB
        await _projekter.DeleteOneAsync(filter);
    }
}
