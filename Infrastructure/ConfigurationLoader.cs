using Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigurationLoader
{
    public static IConfiguration LoadInfrastructureConfiguration(string path = "appsettings.infrastructure.json")
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Ensure System.IO is referenced
            .AddJsonFile(path, optional: false, reloadOnChange: true)
            .Build();
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Auth0Config>(configuration.GetSection(nameof(Auth0Config)));
        return services;
    }
}
