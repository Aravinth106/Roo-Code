using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RooCode.Chat.Server.Models;

namespace RooCode.Chat.Server.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        _settings = settings.Value;
        var connection = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? _settings.ConnectionString;
        var client = new MongoClient(connection);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>(_settings.UsersCollectionName);
    public IMongoCollection<Message> Messages => _database.GetCollection<Message>(_settings.MessagesCollectionName);
}
