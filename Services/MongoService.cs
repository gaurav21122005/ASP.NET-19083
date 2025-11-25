using MongoDB.Driver;

namespace RestaurantApp.Services;

public class MongoService
{
    private readonly IMongoDatabase _database;

    public MongoService(IMongoClient client, IConfiguration config)
    {
        var dbName = config.GetSection("MongoSettings:DatabaseName").Value ?? "RestaurantAppDB";
        _database = client.GetDatabase(dbName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
