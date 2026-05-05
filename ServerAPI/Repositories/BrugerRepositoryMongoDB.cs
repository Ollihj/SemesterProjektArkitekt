using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class BrugerRepositoryMongoDB : IBrugerRepository
{
    private readonly IMongoCollection<Bruger> _collection;

    public BrugerRepositoryMongoDB(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _collection = database.GetCollection<Bruger>("brugere");
    }

    public List<Bruger> GetAll() => _collection.Find(_ => true).ToList();

    public Bruger? GetById(int id) => _collection.Find(b => b.Id == id).FirstOrDefault();

    public void Add(Bruger bruger) => _collection.InsertOne(bruger);

    public void Update(Bruger bruger) => _collection.ReplaceOne(b => b.Id == bruger.Id, bruger);

    public void Delete(int id) => _collection.DeleteOne(b => b.Id == id);
}
