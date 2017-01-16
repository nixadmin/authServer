using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using AuthServerDemo.Configuration.Settings;

namespace AuthServerDemo.Initialization
{
    public static class IdentityAuthenticationExtensions
    {
        public static IApplicationBuilder UseIdentityAuthentication(this IApplicationBuilder app, IConfiguration config)
        {
            var scope = config.GetAuthScope();

            IdentityServerAuthenticationOptions identityServerValidationOptions = new IdentityServerAuthenticationOptions
            {
                Authority = config.GetAuthenticationAuthority(),
                AllowedScopes = new List<string> { scope },
                ApiName = scope,
                AutomaticAuthenticate = config.IsAutoAuthenticate(),
                AutomaticChallenge = config.IsAutoChallenge(),
                RequireHttpsMetadata = config.IsRequireHttps()
            };

            app.UseIdentityServerAuthentication(identityServerValidationOptions);

            return app;
        }
    }
}