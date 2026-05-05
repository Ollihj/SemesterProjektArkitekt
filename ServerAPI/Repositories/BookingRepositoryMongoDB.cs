using Core.Models;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class BookingRepositoryMongoDB : IBookingRepository
{
    private readonly IMongoCollection<Booking> _collection;

    public BookingRepositoryMongoDB(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _collection = database.GetCollection<Booking>("bookinger");
    }

    public List<Booking> GetAll() => _collection.Find(_ => true).ToList();

    public Booking? GetById(int id) => _collection.Find(b => b.Id == id).FirstOrDefault();

    public void Add(Booking booking) => _collection.InsertOne(booking);

    public void Delete(int id) => _collection.DeleteOne(b => b.Id == id);
}
