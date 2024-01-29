using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Frank.Testing.EntityFrameworkCore;

/// <summary>
/// Builder class for constructing an instance of DbContext.
/// </summary>
/// <typeparam name="T">The type of DbContext.</typeparam>
public class DbContextBuilder<T> where T : DbContext
{
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    
    private Action<DbContextOptionsBuilder>? _configuredOptions;
    
    private ILoggerFactory? _loggerFactory;
    private string _sqliteConnectionString = "Data Source=:memory:";
    
    private string _databaseName = "TestDatabase";

    public DbContextBuilder<T> WithLoggerProvider(ILoggerProvider loggerProvider)
    {
        _loggerFactory = LoggerFactory.Create(builder => builder.ClearProviders().AddProvider(loggerProvider));
        return this;
    }
    
    public DbContextBuilder<T> WithSqliteConnectionString(string sqliteConnectionString)
    {
        _sqliteConnectionString = sqliteConnectionString;
        return this;
    }
    
    public DbContextBuilder<T> WithService<TService>(Action<IServiceCollection> configureService)
    {
        configureService(_serviceCollection);
        return this;
    }
    
    public DbContextBuilder<T> WithLoggerFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        return this;
    }
    
    public DbContextBuilder<T> WithOptions(Action<DbContextOptionsBuilder<T>> configureOptions)
    {
        _configuredOptions = configureOptions as Action<DbContextOptionsBuilder>;
        return this;
    }
    
    public DbContextBuilder<T> WithDatabaseName(string databaseName)
    {
        _databaseName = databaseName;
        return this;
    }
    
    public DbContextBuilder<T> WithRandomDatabaseName()
    {
        _databaseName = Guid.NewGuid().ToString();
        return this;
    }
    
    public T Build()
    {
        _serviceCollection.AddDbContext<T>(OptionsAction);
        var context = _serviceCollection.BuildServiceProvider().GetRequiredService<T>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    private void OptionsAction(IServiceProvider arg1, DbContextOptionsBuilder arg2)
    {
        _configuredOptions?.Invoke(arg2);
        if (_loggerFactory != null) 
            arg2.UseLoggerFactory(_loggerFactory);
        
        arg2.EnableSensitiveDataLogging();
        arg2.EnableDetailedErrors();
        arg2.UseSqlite(_sqliteConnectionString);
    }
}