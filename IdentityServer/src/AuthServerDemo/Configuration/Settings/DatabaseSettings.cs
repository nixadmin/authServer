using Microsoft.Extensions.Configuration;
using System;

namespace AuthServerDemo.Configuration.Settings
{
    public static class DatabaseSettings
    {
        private const string ROOT_SECTION = "DatabaseSettings";

        private const string MIGRATE_ON_STARTUP = "MigrateOnStartup";
        private const string MIGRATE_ON_STARTUP_WITH_TEST_DATA = "MigrateOnStatupWithTestingData";
        private const string USE_CONNECTION = "ConnectionName";

        private const string PATTERN = "{0}:{1}";

        public static bool IsMigrateDatabaseOnStartup(this IConfiguration config) {
            return Convert.ToBoolean(config.GetSection(string.Format(PATTERN, ROOT_SECTION, MIGRATE_ON_STARTUP)).Value);
        }

        public static bool IsMigrateDatabaseOnStatupWithTestingData(this IConfiguration config)
        {
            return Convert.ToBoolean(config.GetSection(string.Format(PATTERN, ROOT_SECTION, MIGRATE_ON_STARTUP_WITH_TEST_DATA)).Value);
        }

        public static string GetDatabaseConnectionStringName(this IConfiguration config)
        {
            return config.GetSection(string.Format(PATTERN, ROOT_SECTION, USE_CONNECTION)).Value;
        }
    }
}