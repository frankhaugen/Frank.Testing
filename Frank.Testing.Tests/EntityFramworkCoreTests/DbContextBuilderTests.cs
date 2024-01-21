using Frank.PulseFlow;
using Frank.PulseFlow.Logging;
using Frank.Testing.EntityFrameworkCore;
using Frank.Testing.Logging;
using Frank.Testing.Tests.TestingInfrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Xunit.Abstractions;

namespace Frank.Testing.Tests.EntityFramworkCoreTests;

public class DbContextBuilderTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Build_WithLoggerProvider_UsesLoggerProvider()
    {
        var dbContext = new DbContextBuilder<DbContext>()
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSqlRaw("SELECT 1");
        dbContext.Dispose();
    }
    
    [Fact]
    public void Build_WithService_UsesService()
    {
        var dbContext = new DbContextBuilder<DbContext>()
            .WithService<ITestService>(services => services.AddSingleton<ITestService, TestService>())
            .Build();
        Assert.NotNull(dbContext.GetService<ITestService>());
    }
    
    [Fact]
    public void Build_WithOptions_UsesOptions()
    {
        var dbContext = new DbContextBuilder<DbContext>()
            .WithOptions(options => options.UseSqlite("Data Source=:memory:"))
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSqlRaw("SELECT 1");
        dbContext.Dispose();
    }
    
    [Fact]
    public void Build_WithLoggerProviderAndService_UsesLoggerProviderAndService()
    {
        var conduit = new TestConduit();
        var dbContext = new DbContextBuilder<TestDbContext>()
            .WithSqliteConnectionString("Data Source=MyTestDatabase.db")
            .WithService<ITestService>(services => services.AddSingleton<ITestService, TestService>())
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Persons.Add(new TestPerson() { Name = "Frank" });
        dbContext.SaveChanges();
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

    [Fact]
    public void Build_WithLoggerProviderAndOptions_UsesLoggerProviderAndOptions()
    {
        var conduit = new TestConduit();
        var dbContext = new DbContextBuilder<TestDbContext>()
            .WithSqliteConnectionString("Data Source=MyTestDatabase.db")
            .Build();
        dbContext.Database.EnsureCreated();
        dbContext.Persons.Add(new TestPerson() { Name = "Frank" });
        dbContext.SaveChanges();
        dbContext.Database.ExecuteSqlRaw("SELECT 1");
        dbContext.Dispose();
        
        outputHelper.WriteJson(conduit.Logs);
    }
    
    public class TestConduit : IConduit
    {
        public async Task SendAsync(IPulse message)
        {
            await Task.CompletedTask;
            Logs.Add(message as LogPulse ?? throw new InvalidOperationException());
        }
    
        public List<LogPulse> Logs { get; } = new();
    }
}
