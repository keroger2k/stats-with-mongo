using StatsApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace StatsApi.Services;

public class StatsService
{
    private readonly IMongoCollection<Favorite> _statsCollection;

    public StatsService(
        IOptions<StatsDatabaseSettings> statsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            statsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            statsDatabaseSettings.Value.DatabaseName);

        _statsCollection = mongoDatabase.GetCollection<Favorite>(
            statsDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Favorite>> GetAsync() =>
        await _statsCollection.Find(_ => true).ToListAsync();

    public async Task<Favorite?> GetAsync(string id) =>
        await _statsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Favorite newBook) =>
        await _statsCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Favorite updatedBook) =>
        await _statsCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _statsCollection.DeleteOneAsync(x => x.Id == id);
}