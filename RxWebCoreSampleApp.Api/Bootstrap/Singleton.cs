using Microsoft.Extensions.DependencyInjection;
using RxWebCoreSampleApp.Infrastructure.Singleton;
using RxWebCoreSampleApp.BoundedContext.Singleton;

namespace RxWebCoreSampleApp.Api.Bootstrap
{
    public static class Singleton
    {
        public static void AddSingletonService(this IServiceCollection serviceCollection) {
            serviceCollection.AddSingleton(typeof(TenantDbConnectionInfo));
            serviceCollection.AddSingleton(typeof(UserAccessConfigInfo));
        }

    }
}




