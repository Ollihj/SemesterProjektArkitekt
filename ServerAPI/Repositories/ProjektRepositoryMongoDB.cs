using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class ProjektRepositoryMongoDB : IProjektRepository
{
    private readonly IMongoCollection<Projekt> _collection;

    public ProjektRepositoryMongoDB(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _collection = database.GetCollection<Projekt>("projekter");
    }

    public List<Projekt> GetAll() => _collection.Find(_ => true).ToList();

    public Projekt? GetById(int id) => _collection.Find(p => p.Id == id).FirstOrDefault();

    public void Add(Projekt projekt) => _collection.InsertOne(projekt);

    public void Update(Projekt projekt) => _collection.ReplaceOne(p => p.Id == projekt.Id, projekt);

    public void Delete(int id) => _collection.DeleteOne(p => p.Id == id);
}
