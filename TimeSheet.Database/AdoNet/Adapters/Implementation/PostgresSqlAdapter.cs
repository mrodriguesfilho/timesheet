using System.Data;
using Npgsql;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.AdoNet;

public class PostgresSqlAdapter : IDatabaseAdapter
{
    private readonly NpgsqlConnection[] _npgsqlConnectionArray;

    public PostgresSqlAdapter(string connectionString, int poolOfConnections)
    {
        _npgsqlConnectionArray = CreateConnections(connectionString, poolOfConnections);
    }

    private static NpgsqlConnection[] CreateConnections(string connectionString, int poolOfConnections)
    {
        var connectionsArray = new NpgsqlConnection[poolOfConnections];

        for (var index = 0; index < poolOfConnections; index++)
            connectionsArray[index] = new NpgsqlConnection(connectionString);

        return connectionsArray;
    }

    public async Task<DatabaseResult<int>> ExecuteNonQueryAsync(string query)
    {
        var command = await GetCommand(query);
        var affectedRows = await command.ExecuteNonQueryAsync();
        return DatabaseResult.Ok(affectedRows);
    }
    
    public async Task<DatabaseResult<List<T>>> ExecuteQueryAsync<T>(string query, Func<IDataRecord, T> mapper)
    {
        var command = await GetCommand(query);
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

    public async Task<DatabaseResult<object?>> ExecuteScalarAsync(string query)
    {
        var command = await GetCommand(query);
        var commandResult = await command.ExecuteScalarAsync();
        return DatabaseResult.Ok(commandResult);
    }

    private async Task<NpgsqlCommand> GetCommand(string query)
    {
        var connection = await GetConnection();
        var command = new NpgsqlCommand(query, connection);
        return command;
    }

    private async Task<NpgsqlConnection> GetConnection()
    {
        var availableConnection = _npgsqlConnectionArray.FirstOrDefault(
            x => x.State != ConnectionState.Fetching
                 && x.State != ConnectionState.Executing
                 && x.State != ConnectionState.Connecting);

        if (availableConnection is null) throw new DataException("No connection available on NgsqlConnectionArray");

        if (availableConnection.State is ConnectionState.Open) return availableConnection;

        await availableConnection.OpenAsync();

        return availableConnection;
    }
}