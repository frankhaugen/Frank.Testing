using Microsoft.EntityFrameworkCore;

namespace Frank.Testing.Tests.TestingInfrastructure;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
    
    public DbSet<TestPerson> Persons { get; set; }
    
    public DbSet<TestAddress> Addresses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestPerson>().HasKey(e => e.Id);
        modelBuilder.Entity<TestAddress>().HasKey(e => e.Id);
        base.OnModelCreating(modelBuilder);
    }
}