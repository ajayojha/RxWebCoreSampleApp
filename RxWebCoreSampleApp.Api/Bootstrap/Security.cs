using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RxWebCoreSampleApp.Infrastructure.Security;
using RxWeb.Core.Extensions;
using RxWeb.Core.Security;
using RxWeb.Core.Security.Authorization;
using RxWeb.Core.Security.JwtToken;
using System;

namespace RxWebCoreSampleApp.Api.Bootstrap
{
    public static class Security
    {
        readonly static string AllowMySpecificOrigins = "allowMySpecificOrigins";


        public static void AddSecurity(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAuthorizationPolicyProvider, AccessPolicyProvider>();
            serviceCollection.AddSingleton<IAuthorizationHandler, AccessPermissionHandler>();

            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(AllowMySpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader()
                                .AllowAnyMethod().AllowCredentials();
                });
            });
            
            serviceCollection.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });


            serviceCollection.AddDataProtection().
                 SetDefaultKeyLifetime(TimeSpan.FromDays(14))
                 .SetApplicationName("Test Application")
                 .DisableAutomaticKeyGeneration()
                 .UseCryptographicAlgorithms(new Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorConfiguration()
                 {
                     EncryptionAlgorithm = Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.EncryptionAlgorithm.AES_256_CBC,
                     ValidationAlgorithm = Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ValidationAlgorithm.HMACSHA256
                 });


            serviceCollection.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
            serviceCollection.AddAuthorization();
            serviceCollection.AddRxWebJwtAuthentication();


        }

        public static void UseSecurity(this IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {

            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
                applicationBuilder.UseCors(AllowMySpecificOrigins);
            }
            else
            {
                applicationBuilder.UseHsts();
                applicationBuilder.UseHttpsRedirection();
            }

            applicationBuilder.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always,
            });
            applicationBuilder.UseRouting();

            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();

            HandleException(applicationBuilder);

            SetSecurityHeaders(applicationBuilder);

        }


        private static void HandleException(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    // Log.Error(String.Format("Stacktrace of error: {0}", exception.StackTrace.ToString()));
                });
            });
        }

        private static void SetSecurityHeaders(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use((context, next) =>
            {
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-Content-Type-Options"] = "NOSNIFF";
                context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000";
                context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "master-only";
                context.Response.Headers["Content-Security-Policy"] = "default-src 'none'; style-src 'self'; img-src 'self'; font-src 'self'; script-src 'self'";
                return next();
            });
        }
    }
}




