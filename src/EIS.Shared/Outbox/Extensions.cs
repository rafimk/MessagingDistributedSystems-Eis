using EasyCronJob.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eis.Shared.Options;

namespace Eis.Shared.Outbox;

public static class Extensions
{
    public static IMessagingConfiguration AddOutbox<TContext>(this IMessagingConfiguration msgConfiguration, 
        IConfiguration configuration, string sectionName = "Outbox")
        where TContext : DbContext
    {
        var options = configuration.GetOptions<DeduplicationOptions>(sectionName);

        if (options.Enabled)
        {
            msgConfiguration.Services.AddSingleton(options);
            msgConfiguration.Services.TryDecorate(typeof(IMessageHandler<>), typeof(DeduplicationMessageHandlerDecorator<>));
            msgConfiguration.Services.AddScoped<Func<DbContext>>(sp => sp.GetRequiredService<TContext>);
            
            msgConfiguration.Services.ApplyResulation<DeduplicationCronJob>(o =>
            {
                o.CronExpression = options.Interval;
                o.TimeZoneInfo = TimeZoneInfo.Local;
                o.CronFormat = Cronos.CronFormat.Standard;
            });
        }

        return msgConfiguration;
    }
}