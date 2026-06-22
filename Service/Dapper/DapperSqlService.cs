using Dapper;
using elr_API.IService.IDapper;
using System.Data;
using System.Data.SqlClient;

namespace elr_API.Service.Dapper
{
    public class DapperSqlService : IDapperSqlService
    {

        public async Task<int> Execute(string sql, object parameters = null, string connectionString = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                return await dbConnection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<int> ExecuteAndGetId(string sql, object parameters = null, string connectionString = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Combine the insert statement with the query for the identitybuilder.Services.AddScoped<IDapperSqlService, DapperSql>();

                var query = $"{sql}; SELECT CAST(SCOPE_IDENTITY() AS INT);";

                // Execute the combined query and retrieve the newly inserted ID
                var newId = await dbConnection.ExecuteScalarAsync<int>(query, parameters);

                return newId;
            }
        }

        public async Task<T> QuerySingle<T>(string sql, object parameters = null, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QuerySingleAsync<T>(sql, parameters);
            }
        }
        public async Task<T> QuerySingleOrDefault<T>(string sql, object parameters = null, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
            }
        }

        public async Task<IEnumerable<T>> SelectQuery<T>(string sql, object parameters = null, string connectionString = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                return await dbConnection.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> SelectQuery<T1, T2>(string sql, object parameters = null, string connectionString = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                using (var result = await dbConnection.QueryMultipleAsync(sql, parameters))
                {
                    var result1 = await result.ReadAsync<T1>();
                    var result2 = await result.ReadAsync<T2>();

                    return Tuple.Create(result1, result2);
                }
            }
        }

        public async Task<(T1 TotalCount, IEnumerable<T2> Data)> SelectQueryWithCount<T1, T2>(string sql, object parameters = null, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var result = await connection.QueryMultipleAsync(sql, parameters))
                {
                    // Read the total count (ensure this matches the order in the query)
                    var totalCount = await result.ReadSingleOrDefaultAsync<T1>();

                    //Read the  paginated data
                    var data = await result.ReadAsync<T2>();

                    return (totalCount, data);
                }
            }
        }
    }
}
