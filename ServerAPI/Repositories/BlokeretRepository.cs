using Core.Models;
using MongoDB.Driver;
using ServerAPI.Services;

namespace ServerAPI.Repositories;

public class BlokeretRepository : IBlokeretRepository
{
    private readonly IMongoCollection<Blokeret> _blokerede;

    // Henter blokerede-samlingen fra MongoDB via MongoDbService
    public BlokeretRepository(MongoDbService db)
    {
        _blokerede = db.GetBlokerede();
    }

    // Henter alle blokerede tider fra databasen
    public async Task<List<Blokeret>> GetAll()
    {
        // Henter alle blokerede tider fra MongoDB og returnerer dem
        List<Blokeret> alleBlokerede = await _blokerede.Find(FilterDefinition<Blokeret>.Empty).ToListAsync();
        return alleBlokerede;
    }

    // Henter én blokeret tid baseret på id
    public async Task<Blokeret?> GetById(int id)
    {
        // Henter alle blokerede tider fra MongoDB
        List<Blokeret> alleBlokerede = await _blokerede.Find(FilterDefinition<Blokeret>.Empty).ToListAsync();

        // Går igennem alle blokerede tider og finder den med det rigtige id
        foreach (Blokeret blokeret in alleBlokerede)
        {
            if (blokeret.Id == id)
            {
                // Returnerer den blokerede tid når den er fundet
                return blokeret;
            }
        }

        // Returnerer null hvis ingen blokeret tid havde det id
        return null;
    }

    // Henter alle blokerede tider der tilhører en bestemt bruger
    public async Task<List<Blokeret>> GetByBrugerId(int brugerId)
    {
        // Henter alle blokerede tider fra MongoDB
        List<Blokeret> alleBlokerede = await _blokerede.Find(FilterDefinition<Blokeret>.Empty).ToListAsync();
        // Liste til at samle de blokerede tider der tilhører brugeren
        List<Blokeret> brugerBlokerede = new List<Blokeret>();

        // Går igennem alle blokerede tider og tilføjer dem der har det rigtige brugerId
        foreach (Blokeret blokeret in alleBlokerede)
        {
            if (blokeret.BrugerId == brugerId)
            {
                brugerBlokerede.Add(blokeret);
            }
        }

        // Returnerer listen med brugerens blokerede tider
        return brugerBlokerede;
    }

    // Opretter en ny blokeret tid og giver den næste ledige id
    public async Task<Blokeret> Create(Blokeret blokeret)
    {
        // Henter alle eksisterende blokerede tider for at finde det højeste id
        List<Blokeret> alleBlokerede = await _blokerede.Find(FilterDefinition<Blokeret>.Empty).ToListAsync();

        if (alleBlokerede.Count == 0)
        {
            // Hvis der ingen blokerede tider er, starter vi fra 1
            blokeret.Id = 1;
        }
        else
        {
            // Finder det højeste id blandt alle blokerede tider
            int maxId = 0;
            foreach (Blokeret b in alleBlokerede)
            {
                if (b.Id > maxId)
                {
                    maxId = b.Id;
                }
            }
            // Giver den nye blokerede tid et id der er én højere end det højeste
            blokeret.Id = maxId + 1;
        }

        // Sætter oprettelsesdatoen til nu
        blokeret.Oprettelse = DateTime.Now;
        // Gemmer den nye blokerede tid i MongoDB
        await _blokerede.InsertOneAsync(blokeret);
        // Returnerer den oprettede blokerede tid med det nye id
        return blokeret;
    }

    // Opdaterer en eksisterende blokeret tid med nye data
    public async Task Update(int id, Blokeret blokeret)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Blokeret>.Filter.Eq("_id", id);
        // Sætter id på det nye objekt så det ikke mistes ved erstatning
        blokeret.Id = id;
        // Erstatter det gamle dokument med det nye
        await _blokerede.ReplaceOneAsync(filter, blokeret);
    }

    // Sletter en blokeret tid fra databasen
    public async Task Delete(int id)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Blokeret>.Filter.Eq("_id", id);
        // Sletter dokumentet fra MongoDB
        await _blokerede.DeleteOneAsync(filter);
    }
}
