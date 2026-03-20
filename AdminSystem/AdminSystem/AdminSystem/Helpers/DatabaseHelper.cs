// AdminSystem/Helpers/DatabaseHelper.cs

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AdminSystem.Helpers
{
    /// <summary>
    /// Provides SQL Server connections for all Dapper repository operations.
    /// Connection string is read from App.config "TaurusBikeShopDB".
    /// </summary>
    public static class DatabaseHelper
    {
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["TaurusBikeShopDB"]?.ConnectionString
            ?? throw new InvalidOperationException(
                "Connection string 'TaurusBikeShopDB' not found in App.config.");

        /// <summary>
        /// Opens and returns a new SQL Server connection.
        /// Callers must dispose the connection (use 'using').
        /// </summary>
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Creates a new SQL Server connection without opening it.
        /// Useful when Dapper manages the connection lifecycle.
        /// </summary>
        public static SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        /// <summary>
        /// Tests the connection to the database.
        /// Returns true if successful, false if the connection fails.
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    return conn.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}