using Core.Models;
using MongoDB.Driver;
using ServerAPI.Services;

namespace ServerAPI.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IMongoCollection<Booking> _bookings;

    // Henter bookings-samlingen fra MongoDB via MongoDbService
    public BookingRepository(MongoDbService db)
    {
        _bookings = db.GetBookings();
    }

    // Henter alle bookings fra databasen
    public async Task<List<Booking>> GetAll()
    {
        // Henter alle bookings fra MongoDB og returnerer dem
        List<Booking> alleBookings = await _bookings.Find(FilterDefinition<Booking>.Empty).ToListAsync();
        return alleBookings;
    }

    // Henter én booking baseret på id
    public async Task<Booking?> GetById(int id)
    {
        // Henter alle bookings fra MongoDB
        List<Booking> alleBookings = await _bookings.Find(FilterDefinition<Booking>.Empty).ToListAsync();

        // Går igennem alle bookings og finder den med det rigtige id
        foreach (Booking booking in alleBookings)
        {
            if (booking.Id == id)
            {
                // Returnerer bookingen når den er fundet
                return booking;
            }
        }

        // Returnerer null hvis ingen booking havde det id
        return null;
    }

    // Henter alle bookings der tilhører en bestemt bruger
    public async Task<List<Booking>> GetByBrugerId(int brugerId)
    {
        // Henter alle bookings fra MongoDB
        List<Booking> alleBookings = await _bookings.Find(FilterDefinition<Booking>.Empty).ToListAsync();
        // Liste til at samle de bookings der tilhører brugeren
        List<Booking> brugerBookings = new List<Booking>();

        // Går igennem alle bookings og tilføjer dem der har det rigtige brugerId
        foreach (Booking booking in alleBookings)
        {
            if (booking.BrugerId == brugerId)
            {
                brugerBookings.Add(booking);
            }
        }

        // Returnerer listen med brugerens bookings
        return brugerBookings;
    }

    // Opretter en ny booking og giver den næste ledige id
    public async Task<Booking> Create(Booking booking)
    {
        // Henter alle eksisterende bookings for at finde det højeste id
        List<Booking> alleBookings = await _bookings.Find(FilterDefinition<Booking>.Empty).ToListAsync();

        if (alleBookings.Count == 0)
        {
            // Hvis der ingen bookings er, starter vi fra 1
            booking.Id = 1;
        }
        else
        {
            // Finder det højeste id blandt alle bookings
            int maxId = 0;
            foreach (Booking b in alleBookings)
            {
                if (b.Id > maxId)
                {
                    maxId = b.Id;
                }
            }
            // Giver den nye booking et id der er én højere end det højeste
            booking.Id = maxId + 1;
        }

        // Sætter oprettelsesdatoen til nu
        booking.Oprettelse = DateTime.Now;
        // Gemmer den nye booking i MongoDB
        await _bookings.InsertOneAsync(booking);
        // Returnerer den oprettede booking med det nye id
        return booking;
    }

    // Opdaterer en eksisterende booking med nye data
    public async Task Update(int id, Booking booking)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Booking>.Filter.Eq("_id", id);
        // Sætter id på det nye objekt så det ikke mistes ved erstatning
        booking.Id = id;
        // Erstatter det gamle dokument med det nye
        await _bookings.ReplaceOneAsync(filter, booking);
    }

    // Sletter en booking fra databasen
    public async Task Delete(int id)
    {
        // Laver et filter der finder dokumentet med det rigtige id i MongoDB
        var filter = Builders<Booking>.Filter.Eq("_id", id);
        // Sletter dokumentet fra MongoDB
        await _bookings.DeleteOneAsync(filter);
    }
}
