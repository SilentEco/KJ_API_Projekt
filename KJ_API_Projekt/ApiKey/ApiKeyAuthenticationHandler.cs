using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace KJ_API_Projekt.ApiKey
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            
            )
            : base(options, logger, encoder, clock)
        {
            
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = Request.Headers[ApiKeyConstants.HttpHeaderField];
            if (token == null)
                token = Request.Query[ApiKeyConstants.HttpQueryParamKey];

            if (token == null)
                return AuthenticateResult.Fail("Invalid Authorization Header");


            string testToken = "DemoKey";
            if (token == testToken)
            {

                var identity = new ClaimsIdentity(Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("FAIL!!!");
            }

        }

    }
}
