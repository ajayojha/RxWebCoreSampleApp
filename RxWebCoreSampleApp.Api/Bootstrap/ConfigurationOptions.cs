using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RxWebCoreSampleApp.Models;

namespace RxWebCoreSampleApp.Api.Bootstrap
{
    public static class ConfigurationOptions
    {
        public static void AddConfigurationOptions(this IServiceCollection serviceCollection, IConfiguration configuration) {
            serviceCollection.Configure<DatabaseConfig>(configuration.GetSection("Database"));
        }
    }
}



