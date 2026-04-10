using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AdminSystem_v2.Helpers
{
    public static class DatabaseHelper
    {
        private static readonly string _connectionString = LoadConnectionString();

        private static string LoadConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .AddEnvironmentVariables()
                .Build();

            string raw = config.GetConnectionString("Taurus-bike-shop-sqlserver-2026")
                ?? throw new InvalidOperationException(
                    "Connection string 'Taurus-bike-shop-sqlserver-2026' not found. " +
                    "Set it via User Secrets (dev) or the CONNECTIONSTRINGS__TAURUS-BIKE-SHOP-SQLSERVER-2026 environment variable (prod).");

            // If a separate DbPassword secret is set, inject it into the connection
            // string via SqlConnectionStringBuilder. This avoids parsing issues when
            // passwords contain characters that conflict with connection string syntax
            // ({, }, ; etc.) in Microsoft.Data.SqlClient.
            string? passwordOverride = config["DbPassword"];
            if (!string.IsNullOrEmpty(passwordOverride))
            {
                var builder = new SqlConnectionStringBuilder(raw);
                builder.Password = passwordOverride;
                return builder.ConnectionString;
            }

            return raw;
        }

        /// <summary>
        /// Returns an open SqlConnection. Caller must dispose it (use 'await using').
        /// </summary>
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Tests whether the database is reachable.
        /// </summary>
        public static bool TestConnection()
        {
            try { using var conn = GetConnection(); return true; }
            catch { return false; }
        }
    }
}
