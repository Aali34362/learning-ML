
using MongoDB.Driver;

namespace MachineLearningDataBaseFactory;

internal class MongoDbDatabaseFactory : IDatabaseFactory
{
    private readonly IMongoDatabase _database;

    public MongoDbDatabaseFactory(IMongoDatabase database)
    {
        _database = database;
    }

    public void Connect()
    {
        throw new NotImplementedException();
    }
    public void Disconnect()
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        await collection.InsertOneAsync(entity);
    }
    public async Task CreateListAsync<TEntity>(List<TEntity> entity) where TEntity : class
    {
        var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        await collection.InsertManyAsync(entity);
    }
    public async Task DeleteAsync<TEntity>(object id) where TEntity : class
    {
        var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        await collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id));
    }
    public async Task DeleteListAsync<TEntity>(List<object> ids) where TEntity : class
    {
        var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        await collection.DeleteManyAsync(Builders<TEntity>.Filter.Eq("_id", ids));
    }
    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        var entityId = GetEntityId(entity);
        var updateDefinitionBuilder = Builders<TEntity>.Update;
        var updateDefinitions = new List<UpdateDefinition<TEntity>>();
        var properties = typeof(TEntity).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(entity);
            if (value != null)
            {
                var update = updateDefinitionBuilder.Set(property.Name, value);
                updateDefinitions.Add(update);
            }
        }

        if (!updateDefinitions.Any())
            return;

        var combinedUpdate = Builders<TEntity>.Update.Combine(updateDefinitions);

        await collection.UpdateOneAsync(Builders<TEntity>.Filter.Eq("_id", entityId), combinedUpdate);
    }
    public async Task UpdateListAsync<TEntity>(List<TEntity> entities) where TEntity : class
    {
        var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        foreach (TEntity entity in entities)
        {
            var entityId = GetEntityId(entity);
            var updateDefinitionBuilder = Builders<TEntity>.Update;
            var updateDefinitions = new List<UpdateDefinition<TEntity>>();
            var properties = typeof(TEntity).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                if (value != null)
                {
                    var update = updateDefinitionBuilder.Set(property.Name, value);
                    updateDefinitions.Add(update);
                }
            }

            if (!updateDefinitions.Any())
                return;

            var combinedUpdate = Builders<TEntity>.Update.Combine(updateDefinitions);

            await collection.UpdateOneAsync(Builders<TEntity>.Filter.Eq("_id", entityId), combinedUpdate);
        }
    }



    private object GetEntityId<TEntity>(TEntity entity)
    {
        return entity?.GetType().GetProperty("Id")?.GetValue(entity)!;
    }
}
