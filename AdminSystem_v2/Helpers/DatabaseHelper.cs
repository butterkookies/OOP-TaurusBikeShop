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
                .Build();

            return config.GetConnectionString("Taurus-bike-shop-sqlserver-2026")
                ?? throw new InvalidOperationException(
                    "Connection string 'Taurus-bike-shop-sqlserver-2026' not found in appsettings.json.");
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
