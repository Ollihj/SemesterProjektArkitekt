using Core.Models;
using MongoDB.Driver;
using ServerAPI.Services;

namespace ServerAPI.Repositories;

public class BrugerRepository : IBrugerRepository
{
    private readonly IMongoCollection<Bruger> _brugere;

    // Henter brugere-samlingen fra MongoDB via MongoDbService
    public BrugerRepository(MongoDbService db)
    {
        _brugere = db.GetBrugere();
    }

    // Henter alle brugere fra databasen
    public async Task<List<Bruger>> GetAll()
    {
        // Henter alle brugere fra MongoDB og returnerer dem
        List<Bruger> alleBrugere = await _brugere.Find(FilterDefinition<Bruger>.Empty).ToListAsync();
        return alleBrugere;
    }

    // Henter én bruger baseret på id
    public async Task<Bruger?> GetById(int id)
    {
        // Henter alle brugere fra MongoDB
        List<Bruger> alleBrugere = await _brugere.Find(FilterDefinition<Bruger>.Empty).ToListAsync();

        // Går igennem alle brugere og finder den med det rigtige id
        foreach (Bruger bruger in alleBrugere)
        {
            if (bruger.Id == id)
            {
                // Returnerer brugeren når den er fundet
                return bruger;
            }
        }

        // Returnerer null hvis ingen bruger havde det id
        return null;
    }

    // Opretter en ny bruger og giver den næste ledige id
    public async Task<Bruger> Save(Bruger bruger)
    {
        // Henter alle eksisterende brugere for at finde det højeste id
        List<Bruger> alleBrugere = await _brugere.Find(FilterDefinition<Bruger>.Empty).ToListAsync();

        if (alleBrugere.Count == 0)
        {
            // Hvis der ingen brugere er, starter vi fra 1
            bruger.Id = 1;
        }
        else
        {
            // Finder det højeste id blandt alle brugere
            int maxId = 0;
            foreach (Bruger b in alleBrugere)
            {
                if (b.Id > maxId)
                {
                    maxId = b.Id;
                }
            }
            // Giver den nye bruger et id der er én højere end det højeste
            bruger.Id = maxId + 1;
        }

        // Gemmer den nye bruger i MongoDB
        await _brugere.InsertOneAsync(bruger);
        // Returnerer den oprettede bruger med det nye id
        return bruger;
    }

    // Opdaterer en eksisterende bruger med nye data
    public async Task Update(int id, Bruger bruger)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Bruger>.Filter.Eq("_id", id);
        // Sætter id på det nye objekt så det ikke mistes ved erstatning
        bruger.Id = id;
        // Erstatter det gamle dokument med det nye
        await _brugere.ReplaceOneAsync(filter, bruger);
    }

    // Sletter en bruger fra databasen
    public async Task Delete(int id)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Bruger>.Filter.Eq("_id", id);
        // Sletter dokumentet fra MongoDB
        await _brugere.DeleteOneAsync(filter);
    }

    // Tjekker om email og password matcher en bruger i databasen
    public async Task<Bruger?> Login(string email, string password)
    {
        // Henter alle brugere fra MongoDB
        List<Bruger> alleBrugere = await _brugere.Find(FilterDefinition<Bruger>.Empty).ToListAsync();

        // Går igennem alle brugere og tjekker om email og password passer
        foreach (Bruger bruger in alleBrugere)
        {
            if (bruger.Email == email && bruger.Password == password)
            {
                // Returnerer brugeren hvis både email og password matcher
                return bruger;
            }
        }

        // Returnerer null hvis ingen bruger matchede
        return null;
    }
}
