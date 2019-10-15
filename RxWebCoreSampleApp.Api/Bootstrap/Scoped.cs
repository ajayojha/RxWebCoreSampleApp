#region Namespace
using Microsoft.Extensions.DependencyInjection;
using RxWebCoreSampleApp.Infrastructure.Security;
using RxWeb.Core.Data;
using RxWeb.Core.Security;
using RxWeb.Core.Annotations;
using RxWeb.Core;
#endregion Namespace

namespace RxWebCoreSampleApp.Api.Bootstrap
{
    public static class ScopedExtension
    {

        public static void AddScopedService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepositoryProvider, RepositoryProvider>();
            serviceCollection.AddScoped<ITokenAuthorizer, TokenAuthorizer>();
            serviceCollection.AddScoped<ILocalizationInfo, LocalizationInfo>();
            serviceCollection.AddScoped<IModelValidation, ModelValidation>();

            #region ContextService

            #endregion ContextService


            #region DomainService
            
            #endregion DomainService
        }
    }
}




