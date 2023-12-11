using System.Collections.Concurrent;
using System.Data;
using Npgsql;
using TimeSheet.Database.AdoNet.Adapters.Interface;

namespace TimeSheet.Database.AdoNet.Adapters.Implementation;

public class PostgresSqlAdapter : IDatabaseAdapter
{
    private readonly ConcurrentBag<NpgsqlConnection> _npgsqlConnectionPool;

    public PostgresSqlAdapter(string connectionString, int poolOfConnections)
    {
        _npgsqlConnectionPool = CreateConnections(connectionString, poolOfConnections);
    }

    private static ConcurrentBag<NpgsqlConnection> CreateConnections(string connectionString, int poolOfConnections)
    {
        var connectionPool = new ConcurrentBag<NpgsqlConnection>();

        for (var index = 0; index < poolOfConnections; index++)
            connectionPool.Add(new NpgsqlConnection(connectionString));

        return connectionPool;
    }

    public async Task<DatabaseResult<int>> ExecuteNonQueryAsync(string query)
    {
        var command = await GetCommand(query);

        try
        {
            var affectedRows = await command.ExecuteNonQueryAsync();
            return DatabaseResult.Ok(affectedRows);
        }
        catch (Exception e)
        {
            return DatabaseResult.Fail<int>(0, $"Failed to run {nameof(ExecuteNonQueryAsync)}", e);
        }
        finally
        {
            _npgsqlConnectionPool.Add(command.Connection);
        }
    }

    public async Task<DatabaseResult<List<T>>> ExecuteQueryAsync<T>(string query, Func<IDataRecord, T> mapper)
    {
        var command = await GetCommand(query);

        try
        {
            var dataTable = new DataTable();
            await using var reader = await command.ExecuteReaderAsync();
            dataTable.Load(reader);
            var dataTableReader = dataTable.CreateDataReader();

            var resultList = new List<T>();

            while (await dataTableReader.ReadAsync())
            {
                var resultEntry = mapper(dataTableReader);
                resultList.Add(resultEntry);
            }

            return DatabaseResult.Ok(resultList);
        }
        catch (Exception e)
        {
            return DatabaseResult.Fail<List<T>>(default(List<T>), $"Failed to run {nameof(ExecuteQueryAsync)}", e);
        }
        finally
        {
            _npgsqlConnectionPool.Add(command.Connection);
        }
    }

    public async Task<DatabaseResult<object?>> ExecuteScalarAsync(string query)
    {
        var command = await GetCommand(query);

        try
        {
            var commandResult = await command.ExecuteScalarAsync();
            return DatabaseResult.Ok(commandResult);
        }
        catch (Exception e)
        {
            return DatabaseResult.Fail<object>(null, "Failed to run {}", e);
        }
        finally
        {
            _npgsqlConnectionPool.Add(command.Connection);
        }
    }

    private async Task<NpgsqlCommand> GetCommand(string query)
    {
        var connection = await GetConnection();
        var command = new NpgsqlCommand(query, connection);
        return command;
    }
    
    private async Task<NpgsqlConnection> GetConnection()
    {
        if (!_npgsqlConnectionPool.TryTake(out var availableConnection))
            throw new DataException("No connection available on NgsqlConnectionArray");

        if (availableConnection.State == ConnectionState.Closed) await availableConnection.OpenAsync();

        return availableConnection;
    }
}