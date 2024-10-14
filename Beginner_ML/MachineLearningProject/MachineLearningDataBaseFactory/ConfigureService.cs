using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;

namespace MachineLearningDataBaseFactory;

public class ConfigureService
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add database context for SQL Server, MySQL, PostgreSQL
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
        services.AddDbContext<MyDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
        services.AddDbContext<MyDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PostgreSqlConnection")));

        // Register Redis and MongoDB clients
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
        services.AddSingleton<IMongoClient>(new MongoClient("mongodb://localhost:27017"));

        // Register database factories
        services.AddTransient<SqlServerDatabaseFactory>();
        services.AddTransient<MySqlDatabaseFactory>();
        services.AddTransient<MongoDbDatabaseFactory>();
        services.AddTransient<RedisCacheDatabaseFactory>();
        //services.AddTransient<PostgreSqlDatabaseFactory>();

        // Register the provider
        services.AddTransient<DatabaseFactoryProvider>();
    }
}
