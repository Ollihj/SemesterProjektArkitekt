using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class AftaleRepositoryMongoDB : IAftaleRepository
{
    private readonly IMongoCollection<Aftale> _collection;

    public AftaleRepositoryMongoDB(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _collection = database.GetCollection<Aftale>("aftaler");
    }

    public List<Aftale> GetAll() => _collection.Find(_ => true).ToList();

    public Aftale? GetById(int id) => _collection.Find(a => a.Id == id).FirstOrDefault();

    public void Add(Aftale aftale) => _collection.InsertOne(aftale);

    public void Update(Aftale aftale) => _collection.ReplaceOne(a => a.Id == aftale.Id, aftale);

    public void Delete(int id) => _collection.DeleteOne(a => a.Id == id);
}
