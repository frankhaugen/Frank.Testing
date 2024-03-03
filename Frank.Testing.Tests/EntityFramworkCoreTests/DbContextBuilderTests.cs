using Frank.Testing.EntityFrameworkCore;
using Frank.Testing.Logging;
using Frank.Testing.Tests.TestingInfrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NSubstitute;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.EntityFramworkCoreTests;

public class DbContextBuilderTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Build_WithLoggerProvider_UsesLoggerProvider()
    {
        var dbContext = new DbContextBuilder<DbContext>()
            .WithLoggerProvider(new SimpleTestLoggerProvider(outputHelper, Options.Create(new LoggerFilterOptions() { MinLevel = LogLevel.Debug })))
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSqlRaw("SELECT 1");
        dbContext.Dispose();
    }
    
    [Fact]
    public void Build_WithService_UsesService()
    {
        var dbContext = new DbContextBuilder<DbContext>()
            .WithLoggerProvider(new SimpleTestLoggerProvider(outputHelper, Options.Create(new LoggerFilterOptions() { MinLevel = LogLevel.Debug })))
            .WithService<ITestService>(services => services.AddSingleton<ITestService, TestService>())
            .Build();
        Assert.NotNull(dbContext.GetService<ITestService>());
    }
    
    [Fact]
    public void Build_WithOptions_UsesOptions()
    {
        var dbContext = new DbContextBuilder<DbContext>()
            .WithLoggerProvider(new SimpleTestLoggerProvider(outputHelper, Options.Create(new LoggerFilterOptions() { MinLevel = LogLevel.Debug })))
            .WithOptions(options => options.UseSqlite("Data Source=:memory:"))
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSqlRaw("SELECT 1");
        dbContext.Dispose();
    }
    
    [Fact]
    public void Build_WithLoggerProviderAndService_UsesLoggerProviderAndService()
    {
        var dbContext = new DbContextBuilder<TestDbContext>()
            .WithLoggerProvider(new SimpleTestLoggerProvider(outputHelper, Options.Create(new LoggerFilterOptions() { MinLevel = LogLevel.Debug })))
            .WithSqliteConnectionString("Data Source=MyTestDatabase.db")
            .WithService<ITestService>(services => services.AddSingleton<ITestService, TestService>())
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Persons.Add(new TestPerson() { Name = "Frank" });
        dbContext.SaveChanges();
        dbContext.Dispose();
    }

    [Fact]
    public void Build_WithLoggerProviderAndOptions_UsesLoggerProviderAndOptions()
    {
        var dbContext = new DbContextBuilder<TestDbContext>()
            .WithLoggerProvider(new SimpleTestLoggerProvider(outputHelper, Options.Create(new LoggerFilterOptions() { MinLevel = LogLevel.Debug })))
            .WithSqliteConnectionString("Data Source=MyTestDatabase.db")
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Persons.Add(new TestPerson() { Name = "Frank" });
        dbContext.SaveChanges();
        dbContext.Database.ExecuteSqlRaw("SELECT 1");
        dbContext.Dispose();
        
    }
    
    

    public class TestService : ITestService
    {
        public async Task DoSomethingAsync()
        {
            await Task.CompletedTask;
        }
    }

    public interface ITestService
    {
        Task DoSomethingAsync();
    }
}
