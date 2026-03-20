using System.Collections.Generic;
using System.Data;
using AdminSystem.Helpers;
using Dapper;
using System.Data.SqlClient;

namespace AdminSystem.Repositories
{
    public abstract class Repository<T> where T : class
    {
        protected SqlConnection GetConnection() => DatabaseHelper.GetConnection();

        protected IEnumerable<T> Query(string sql, object param = null)
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<T>(sql, param);
        }

        protected T QueryFirstOrDefault(string sql, object param = null)
        {
            using (SqlConnection conn = GetConnection())
                return conn.QueryFirstOrDefault<T>(sql, param);
        }

        protected int Execute(string sql, object param = null)
        {
            using (SqlConnection conn = GetConnection())
                return conn.Execute(sql, param);
        }

        protected int ExecuteScalar(string sql, object param = null)
        {
            using (SqlConnection conn = GetConnection())
                return conn.ExecuteScalar<int>(sql, param);
        }

        protected void ExecuteTransaction(
            System.Action<SqlConnection, IDbTransaction> work)
        {
            using (SqlConnection conn = GetConnection())
            using (IDbTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    work(conn, tx);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
