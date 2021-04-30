using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using KJ_API_Projekt.model;
using KJ_API_Projekt.data;
using Microsoft.EntityFrameworkCore;

namespace KJ_API_Projekt.ApiKey
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ApplicationDbContext _context;
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ApplicationDbContext context

            
            )
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = Request.Headers[ApiKeyConstants.HttpHeaderField];
            if (token == null)
                token = Request.Query[ApiKeyConstants.HttpQueryParamKey];

            if (token == null)
                return AuthenticateResult.Fail("Invalid Authorization Header");

            List<ApiToken> ApiKeys = _context.ApiTokens.Include(m => m.User).ToList();

            foreach (var key in ApiKeys)
            {
                if (token == key.Value) 
                {
                    var identity = new ClaimsIdentity(Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);

                }
            }
            return AuthenticateResult.Fail("fail");
           // string testToken = ApiKeys[1].Value;
           // if (token == testToken)
           // {
           //
           //     var identity = new ClaimsIdentity(Scheme.Name);
           //     var principal = new ClaimsPrincipal(identity);
           //     var ticket = new AuthenticationTicket(principal, Scheme.Name);
           //
           //     return AuthenticateResult.Success(ticket);
           // }
           // else
           // {
           //     return AuthenticateResult.Fail("FAIL!!!");
           // }

        }

    }
}
