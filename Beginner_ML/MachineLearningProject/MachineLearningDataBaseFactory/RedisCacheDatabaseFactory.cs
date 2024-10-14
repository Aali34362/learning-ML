using MongoDB.Bson.IO;
using StackExchange.Redis;
using System.Text.Json;
using RedisDatabase = StackExchange.Redis.IDatabase;

namespace MachineLearningDataBaseFactory;

public class RedisCacheDatabaseFactory : IDatabaseFactory
{
    private readonly IConnectionMultiplexer _connection;
    private readonly RedisDatabase _cache;

    public RedisCacheDatabaseFactory(IConnectionMultiplexer connection)
    {
        _connection = connection;
        _cache = connection.GetDatabase();
    }


    public void Connect() { /* Redis manages connections automatically */ }    
    public void Disconnect() => _connection.Dispose();

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var id = entity.GetType().GetProperty("Id")?.GetValue(entity)!.ToString();
        var json = JsonSerializer.Serialize(entity);
        await _cache.StringSetAsync(id, json);
    }

    public Task CreateListAsync<TEntity>(List<TEntity> entity) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync<TEntity>(object id) where TEntity : class
    {
        await _cache.KeyDeleteAsync(id.ToString());
    }

    public Task DeleteListAsync<TEntity>(List<object> ids) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var id = entity.GetType().GetProperty("Id")?.GetValue(entity)!.ToString();
        var json = JsonSerializer.Serialize(entity);
        await _cache.StringSetAsync(id, json);
    }

    public Task UpdateListAsync<TEntity>(List<TEntity> entity) where TEntity : class
    {
        throw new NotImplementedException();
    }
}
