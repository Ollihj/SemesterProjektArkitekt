using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class BlokeretRepositoryMongoDB : IBlokeretRepository
{
    private readonly IMongoCollection<Blokeret> _collection;

    public BlokeretRepositoryMongoDB(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _collection = database.GetCollection<Blokeret>("blokeringer");
    }

    public List<Blokeret> GetAll() => _collection.Find(_ => true).ToList();

    public Blokeret? GetById(int id) => _collection.Find(b => b.Id == id).FirstOrDefault();

    public void Add(Blokeret blokeret) => _collection.InsertOne(blokeret);

    public void Delete(int id) => _collection.DeleteOne(b => b.Id == id);
}
