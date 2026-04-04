using AdminSystem_v2.Helpers;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AdminSystem_v2.Repositories
{
    public abstract class Repository<T> where T : class
    {
        protected SqlConnection GetConnection() => DatabaseHelper.GetConnection();

        protected async Task<IEnumerable<T>> QueryAsync(string sql, object? param = null)
        {
            await using SqlConnection conn = GetConnection();
            return await conn.QueryAsync<T>(sql, param);
        }

        protected async Task<T?> QueryFirstOrDefaultAsync(string sql, object? param = null)
        {
            await using SqlConnection conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        // Allows querying a different type than T (e.g. querying Category from ProductRepository)
        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object? param = null)
        {
            await using SqlConnection conn = GetConnection();
            return await conn.QueryAsync<TResult>(sql, param);
        }

        protected async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            await using SqlConnection conn = GetConnection();
            return await conn.ExecuteAsync(sql, param);
        }

        protected async Task<int> ExecuteScalarAsync(string sql, object? param = null)
        {
            await using SqlConnection conn = GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, param);
        }

        protected async Task<TResult?> ExecuteScalarAsync<TResult>(string sql, object? param = null)
        {
            await using SqlConnection conn = GetConnection();
            return await conn.ExecuteScalarAsync<TResult>(sql, param);
        }
    }
}
