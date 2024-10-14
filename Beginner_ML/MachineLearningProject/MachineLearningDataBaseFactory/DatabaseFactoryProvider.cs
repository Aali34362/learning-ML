using Microsoft.Extensions.DependencyInjection;

namespace MachineLearningDataBaseFactory;

public class DatabaseFactoryProvider
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseFactoryProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDatabaseFactory GetFactory(DatabaseType dbType)
    {
        return dbType switch
        {
            DatabaseType.SqlServer => _serviceProvider.GetService<SqlServerDatabaseFactory>(),
            DatabaseType.MySql => _serviceProvider.GetService<MySqlDatabaseFactory>(),
            DatabaseType.MongoDb => _serviceProvider.GetService<MongoDbDatabaseFactory>(),
            DatabaseType.Redis => _serviceProvider.GetService<RedisCacheDatabaseFactory>(),
            //DatabaseType.PostgreSql => _serviceProvider.GetService<PostgreSqlDatabaseFactory>(),
            _ => throw new ArgumentException("Invalid database type")
        };
    }
}
