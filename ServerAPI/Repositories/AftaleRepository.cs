using Core.Models;
using MongoDB.Driver;
using ServerAPI.Services;

namespace ServerAPI.Repositories;

public class AftaleRepository : IAftaleRepository
{
    private readonly IMongoCollection<Aftale> _aftaler;

    // Henter aftaler-samlingen fra MongoDB via MongoDbService
    public AftaleRepository(MongoDbService db)
    {
        _aftaler = db.GetAftaler();
    }

    // Henter alle aftaler fra databasen
    public async Task<List<Aftale>> GetAll()
    {
        // Henter alle aftaler fra MongoDB og returnerer dem
        List<Aftale> alleAftaler = await _aftaler.Find(FilterDefinition<Aftale>.Empty).ToListAsync();
        return alleAftaler;
    }

    // Henter én aftale baseret på id
    public async Task<Aftale?> GetById(int id)
    {
        // Henter alle aftaler fra MongoDB
        List<Aftale> alleAftaler = await _aftaler.Find(FilterDefinition<Aftale>.Empty).ToListAsync();

        // Går igennem alle aftaler og finder den med det rigtige id
        foreach (Aftale aftale in alleAftaler)
        {
            if (aftale.Id == id)
            {
                // Returnerer aftalen når den er fundet
                return aftale;
            }
        }

        // Returnerer null hvis ingen aftale havde det id
        return null;
    }

    // Henter alle aftaler der tilhører en bestemt bruger
    public async Task<List<Aftale>> GetByBrugerId(int brugerId)
    {
        // Henter alle aftaler fra MongoDB
        List<Aftale> alleAftaler = await _aftaler.Find(FilterDefinition<Aftale>.Empty).ToListAsync();
        // Liste til at samle de aftaler der tilhører brugeren
        List<Aftale> brugerAftaler = new List<Aftale>();

        // Går igennem alle aftaler og tilføjer dem der har det rigtige brugerId
        foreach (Aftale aftale in alleAftaler)
        {
            if (aftale.BrugerId == brugerId)
            {
                brugerAftaler.Add(aftale);
            }
        }

        // Returnerer listen med brugerens aftaler
        return brugerAftaler;
    }

    // Opretter en ny aftale og giver den næste ledige id
    public async Task<Aftale> Save(Aftale aftale)
    {
        // Henter alle eksisterende aftaler for at finde det højeste id
        List<Aftale> alleAftaler = await _aftaler.Find(FilterDefinition<Aftale>.Empty).ToListAsync();

        if (alleAftaler.Count == 0)
        {
            // Hvis der ingen aftaler er, starter vi fra 1
            aftale.Id = 1;
        }
        else
        {
            // Finder det højeste id blandt alle aftaler
            int maxId = 0;
            foreach (Aftale a in alleAftaler)
            {
                if (a.Id > maxId)
                {
                    maxId = a.Id;
                }
            }
            // Giver den nye aftale et id der er én højere end det højeste
            aftale.Id = maxId + 1;
        }

        // Sætter oprettelsesdatoen til nu
        aftale.Oprettelse = DateTime.Now;
        // Gemmer den nye aftale i MongoDB
        await _aftaler.InsertOneAsync(aftale);
        // Returnerer den oprettede aftale med det nye id
        return aftale;
    }

    // Opdaterer en eksisterende aftale med nye data
    public async Task Update(int id, Aftale aftale)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Aftale>.Filter.Eq("_id", id);
        // Sætter id på det nye objekt så det ikke mistes ved erstatning
        aftale.Id = id;
        // Erstatter det gamle dokument med det nye
        await _aftaler.ReplaceOneAsync(filter, aftale);
    }

    // Sletter en aftale fra databasen
    public async Task Delete(int id)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Aftale>.Filter.Eq("_id", id);
        // Sletter dokumentet fra MongoDB
        await _aftaler.DeleteOneAsync(filter);
    }
}
