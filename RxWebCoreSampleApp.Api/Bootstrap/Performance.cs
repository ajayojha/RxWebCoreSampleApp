using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using RxWebCoreSampleApp.Infrastructure.Singleton;
using System.IO.Compression;
using System.Linq;

namespace RxWebCoreSampleApp.Api.Bootstrap
{
    public static class Performance
    {
        public static void AddPerformance(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddMemoryCache();

            serviceCollection.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });


            serviceCollection.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

        }

        public static void UsePerformance(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseResponseCompression();
        }
    }
}




