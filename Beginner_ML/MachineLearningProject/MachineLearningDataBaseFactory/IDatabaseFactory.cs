namespace MachineLearningDataBaseFactory;

public interface IDatabaseFactory
{
    void Connect();
    void Disconnect();
    Task CreateAsync<TEntity>(TEntity entity) where TEntity : class;
    Task CreateListAsync<TEntity>(List<TEntity> entity) where TEntity : class;
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
    Task UpdateListAsync<TEntity>(List<TEntity> entity) where TEntity : class;
    Task DeleteAsync<TEntity>(object id) where TEntity : class;
    Task DeleteListAsync<TEntity>(List<object> ids) where TEntity : class;
}
