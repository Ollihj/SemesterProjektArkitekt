using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _database;

    // Læser connection string og databasenavn fra appsettings.json og opretter forbindelsen til MongoDB
    public MongoDbService(IConfiguration config)
    {
        // Henter connection string fra appsettings.json
        string connectionString = config["MongoDB:ConnectionString"];
        // Henter databasenavnet fra appsettings.json
        string databaseName = config["MongoDB:DatabaseName"];

        // Opretter en forbindelse til MongoDB med connection string
        MongoClient client = new MongoClient(connectionString);
        // Henter den specifikke database vi vil arbejde med
        _database = client.GetDatabase(databaseName);
    }

    // Returnerer samlingen af brugere fra databasen
    public IMongoCollection<Bruger> GetBrugere()
    {
        return _database.GetCollection<Bruger>("brugere");
    }

    // Returnerer samlingen af bookings fra databasen
    public IMongoCollection<Booking> GetBookings()
    {
        return _database.GetCollection<Booking>("bookings");
    }

    // Returnerer samlingen af aftaler fra databasen
    public IMongoCollection<Aftale> GetAftaler()
    {
        return _database.GetCollection<Aftale>("aftaler");
    }

    // Returnerer samlingen af projekter fra databasen
    public IMongoCollection<Projekt> GetProjekter()
    {
        return _database.GetCollection<Projekt>("projekter");
    }

    // Returnerer samlingen af blokerede tider fra databasen
    public IMongoCollection<Blokeret> GetBlokerede()
    {
        return _database.GetCollection<Blokeret>("blokerede");
    }
}