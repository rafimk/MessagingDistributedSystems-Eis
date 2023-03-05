using EIS.MessageSubscriber.Api.Domain;
using EIS.Shared.Messaging.Inbox;
using Microsoft.EntityFrameworkCore;

namespace EIS.MessageSubscriber.Api.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<WeatherForecast> WeatherForecast { get; set; }
   // public DbSet<InboxMessage> InboxMessages { get; set; }
    

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<WeatherForecast>()
            .HasKey(x => x.Id);
        
        // builder.Entity<InboxMessage>()
        //     .HasKey(x => x.Id);
    }
}