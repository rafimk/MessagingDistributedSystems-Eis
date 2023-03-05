using EIS.Shared.Messaging.Inbox;
using Microsoft.EntityFrameworkCore;
using TestAPI.Domain;

namespace TestAPI.Persistence;


internal class TestDbContext : DbContext
{
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<InboxMessage> Inbox { get; set; }
        
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("messages");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}