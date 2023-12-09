using System.Data;

namespace TimeSheet.Database.AdoNet;

public interface IDatabaseAdapter
{
    Task<DatabaseResult<List<T>>> ExecuteQueryAsync<T>(string query, Func<IDataRecord, T> mapper);
    Task<DatabaseResult<object?>> ExecuteScalarAsync(string query);
    Task<DatabaseResult<int>> ExecuteNonQueryAsync(string query);
}