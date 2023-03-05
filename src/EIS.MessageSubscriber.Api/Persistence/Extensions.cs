using Microsoft.EntityFrameworkCore;

namespace EIS.MessageSubscriber.Api.Persistence;

public static class Extensions
{
    private const string OptionsSectionName = "postgres";

    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetRequiredSection(OptionsSectionName));
        var postgresOptions = configuration.GetOptions<PostgresOptions>(OptionsSectionName);
        services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(postgresOptions.ConnectionString));

        return services;
    }
    
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string sectionName) where TOptions : new()
    {
        var options = new TOptions();
        configuration.GetSection(sectionName).Bind(options);

        return options;
    }
}