
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RxWebCoreSampleApp.Api.Bootstrap;
using RxWeb.Core.Extensions;
using RxWeb.Core.AspNetCore.Extensions;

namespace RxWebCoreSampleApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            
            services.AddConfigurationOptions(Configuration);

            services.AddHttpContextAccessor();
            services.AddPerformance();
            services.AddSecurity();
            services.AddSingletonService();
            services.AddScopedService();
            services.AddDbContextService();
            services.AddControllers();
            services.AddMvc(options=> {
                options.AddRxWebSanitizers();
                options.AddValidation();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePerformance();

            app.UseRouting();

            app.UseSecurity(env);

            

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization(); ;
            });
        }
    }
}



