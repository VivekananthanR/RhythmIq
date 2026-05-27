namespace mct_API.IService.IDapper
{
    public interface IDapperSqlService
    {
        Task<IEnumerable<T>> SelectQuery<T>(string sql, object parameters = null, string connectionString = null);
        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> SelectQuery<T1, T2>(string sql, object parameters = null, string connectionString = null);
        Task<int> Execute(string sql, object parameters = null, string connectionString = null);
        Task<int> ExecuteAndGetId(string sql, object parameters = null, string connectionString = null);
        Task<(T1 TotalCount, IEnumerable<T2> Data)> SelectQueryWithCount<T1, T2>(string sql, object parameters = null, string connectionString = null);
        Task<T> QuerySingle<T>(string sql, object parameters = null, string connectionString = null);
    }
}
