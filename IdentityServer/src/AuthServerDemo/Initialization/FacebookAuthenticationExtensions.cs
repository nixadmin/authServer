using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using IdentityServer4;
using AuthServerDemo.Configuration.Settings;

namespace AuthServerDemo.Initialization
{
    public static class FacebookAuthenticationExtensions
    {
        public static IApplicationBuilder UseFacebook(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseFacebookAuthentication(new FacebookOptions
            {
                AuthenticationScheme = config.GetFacebookAuthScheme(),
                DisplayName = config.GetFacebookDisplayName(),
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                ClientId = config.GetFacebookClientId(),
                ClientSecret = config.GetFacebookSecret()
            });

            return app;
        }
    }
}