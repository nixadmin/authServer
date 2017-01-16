using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerDemo.Configuration.Settings
{
    public static class ServerAuthentication
    {
        private const string ROOT_SECTION = "ServerAuthentication";

        private const string REQUIRE_HTTPS = "RequireHttpsMetadata";
        private const string AUTHORITY = "Authority";
        private const string AUTO_AUTHENTICATION = "AutomaticAuthenticate";
        private const string AUTO_CHALLENGE = "AutomaticChallenge";
        private const string API_NAME = "ApiName";

        private const string PATTERN = "{0}:{1}";

        public static bool IsRequireHttps(this IConfiguration config)
        {
            return Convert.ToBoolean(config.GetSection(string.Format(PATTERN, ROOT_SECTION, REQUIRE_HTTPS)).Value);
        }

        public static string GetAuthenticationAuthority(this IConfiguration config)
        {
            return config.GetSection(string.Format(PATTERN, ROOT_SECTION, AUTHORITY)).Value;
        }

        public static bool IsAutoAuthenticate(this IConfiguration config)
        {
            return Convert.ToBoolean(config.GetSection(string.Format(PATTERN, ROOT_SECTION, AUTO_AUTHENTICATION)).Value);
        }

        public static bool IsAutoChallenge(this IConfiguration config)
        {
            return Convert.ToBoolean(config.GetSection(string.Format(PATTERN, ROOT_SECTION, AUTO_CHALLENGE)).Value);
        }

        public static string GetAuthScope(this IConfiguration config)
        {
            return config.GetSection(string.Format(PATTERN, ROOT_SECTION, API_NAME)).Value;
        }
    }
}
