using Microsoft.EntityFrameworkCore;

namespace MachineLearningDataBaseFactory;

public class MySqlDatabaseFactory : IDatabaseFactory
{
    private readonly DbContext _context;

    public MySqlDatabaseFactory(DbContext context)
    {
        _context = context;
    }

    public void Connect() => _context.Database.OpenConnection();
    public void Disconnect() => _context.Database.CloseConnection();

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(object id) where TEntity : class
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateListAsync<TEntity>(List<TEntity> entity) where TEntity : class
    {
        _context.Set<TEntity>().AddRange(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateListAsync<TEntity>(List<TEntity> entity) where TEntity : class
    {
        _context.Set<TEntity>().UpdateRange(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteListAsync<TEntity>(List<object> ids) where TEntity : class
    {
        foreach (object id in ids)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}