using Microsoft.Extensions.Configuration;

namespace AuthServerDemo.Configuration.Settings
{
    public static class FacebookSettings
    {
        private const string ROOT_SECTION = "Facebook";

        private const string CLIENT_ID = "ClientId";
        private const string SECRET = "Secret";
        private const string AUTH_SCHEME = "AuthScheme";
        private const string DISPLAY_NAME = "DisplayName";

        private const string PATTERN = "{0}:{1}";

        public static string GetFacebookClientId(this IConfiguration config)
        {
            return config.GetSection(string.Format(PATTERN, ROOT_SECTION, CLIENT_ID)).Value;
        }

        public static string GetFacebookSecret(this IConfiguration config)
        {
            return config.GetSection(string.Format(PATTERN, ROOT_SECTION, SECRET)).Value;
        }

        public static string GetFacebookAuthScheme(this IConfiguration config)
        {
            return config.GetSection(string.Format(PATTERN, ROOT_SECTION, AUTH_SCHEME)).Value;
        }

        public static string GetFacebookDisplayName(this IConfiguration config)
        {
            return config.GetSection(string.Format(PATTERN, ROOT_SECTION, DISPLAY_NAME)).Value;
        }
    }
}
