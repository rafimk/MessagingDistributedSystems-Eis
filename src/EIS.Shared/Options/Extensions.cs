using Microsoft.Extensions.Configuration;

namespace EIS.Shared.Options;

internal static class Extensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string sectionName) where TOptions : new()
    {
        var options = new TOptions();
        configuration.GetSection(sectionName).Bind(options);

        return options;
    }
    
    public static T BindOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        => BindOptions<T>(configuration.GetSection(sectionName));

    public static T BindOptions<T>(this IConfigurationSection section) where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }
}