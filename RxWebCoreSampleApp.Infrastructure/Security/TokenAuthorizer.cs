using Microsoft.AspNetCore.Authentication.JwtBearer;
using RxWeb.Core.Security;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RxWebCoreSampleApp.Infrastructure.Security
{
    public class TokenAuthorizer : ITokenAuthorizer
    {
        public TokenAuthorizer(IJwtTokenProvider tokenProvider) {
            TokenProvider = tokenProvider;
        }

        public Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            throw new NotImplementedException();
        }

        public Task Challenge(JwtBearerChallengeContext context)
        {
            context.Error = "Token not exists";
            context.ErrorDescription = "You have to send the token";
            context.ErrorUri = context.Request.Path.ToString();

            return Task.FromResult(0);
        }

        public Task MessageReceived(MessageReceivedContext context)
        {
            var principal = this.ValidateToken(context.HttpContext);
            if (principal != null)
            {
                context.Principal = principal;
                context.Success();
            }
            else
                context.Fail("  Token Not Found");
            return Task.CompletedTask;
        }

        public Task TokenValidated(TokenValidatedContext context)
        {
            throw new NotImplementedException();
        }

		public ClaimsPrincipal ValidateToken(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization") && context.Request.Headers.ContainsKey("x-request"))
            {
                return this.TokenProvider.ValidateToken(context.Request.Headers["x-request"], context.Request.Headers["Authorization"]);
            }
            return null;
        }

        private IJwtTokenProvider TokenProvider { get; set; }

    }
}


