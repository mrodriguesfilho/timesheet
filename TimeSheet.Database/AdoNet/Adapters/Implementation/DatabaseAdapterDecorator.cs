using System.Data;

namespace TimeSheet.Database.AdoNet;

public class DatabaseAdapterDecorator : IDatabaseAdapter
{

    private IDatabaseAdapter _databaseAdapter;
    
    public DatabaseAdapterDecorator(IDatabaseAdapter databaseAdapter)
    {
        _databaseAdapter = databaseAdapter;
    }
    
    public async Task<T> Handle<T>(string query, Func<string, Task<T>> databaseFunction)
    {
        try
        {
            return await databaseFunction(query);
        }
        catch (Exception e)
        {
            return DatabaseResult.Fail<T>($"Failed to execute {nameof(databaseFunction)}", e);
        }
    }
    
    public async Task<DatabaseResult<DataTable>> ExecuteQueryAsync(string query)
    {
        return await Handle<DatabaseResult<DataTable>>(query, _databaseAdapter.ExecuteQueryAsync);
    }

    public Task<DatabaseResult<object?>> ExecuteScalarAsync(string query)
    {
        throw new NotImplementedException();
    }

    public Task<DatabaseResult<int>> ExecuteNonQueryAsync(string query)
    {
        throw new NotImplementedException();
    }
}