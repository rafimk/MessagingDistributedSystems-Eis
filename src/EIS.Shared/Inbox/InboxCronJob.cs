using EasyCronJob.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EIS.Shared.Inbox;

internal sealed class InboxCronJob : CronJobService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DeduplicationOptions _options;

    public InboxCronJob(ICronConfiguration<DeduplicationCronJob> cronConfiguration, IServiceProvider serviceProvider,
        DeduplicationOptions options) 
        : base(cronConfiguration.CronExpression,cronConfiguration.TimeZoneInfo,cronConfiguration.CronFormat)
    {
        _serviceProvider = serviceProvider;
        _options = options;
    }
    
    
    public override async Task DoWork(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<Func<DbContext>>()();

        var modelsToRemove = await context.Set<DeduplicationModel>()
            .Where(x => x.ProcessedAt.AddDays(_options.MessageEvictionWindowInDays) < DateTime.UtcNow)
            .ToListAsync(cancellationToken);
        
        context.Set<DeduplicationModel>().RemoveRange(modelsToRemove);
        await context.SaveChangesAsync(cancellationToken);
    }
}