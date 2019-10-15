using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RxWebCoreSampleApp.Infrastructure.Singleton;
using RxWeb.Core.Security.Authorization;
using System.Threading.Tasks;

namespace RxWebCoreSampleApp.Infrastructure.Security
{
    public class AccessPermissionHandler : AuthorizationHandler<AccessPermissionRequirement>
    {
        private UserAccessConfigInfo UserAccessConfig { get; set; }
        private IHttpContextAccessor ContextAccessor { get; set; }
        public AccessPermissionHandler(UserAccessConfigInfo userAccessConfig, IHttpContextAccessor contextAccessor)
        {
            UserAccessConfig = userAccessConfig;
            ContextAccessor = contextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessPermissionRequirement requirement)
        {
            var requestMethod = ContextAccessor.HttpContext.Request.Method.ToUpper();
            if (UserAccessConfig.Get(1, requirement.ApplicationModuleId, requestMethod))
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }
}


