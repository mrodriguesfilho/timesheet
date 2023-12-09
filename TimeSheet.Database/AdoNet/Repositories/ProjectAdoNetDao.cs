using TimeSheet.Database.AdoNet.Queries;
using TimeSheet.Database.ModelMappers;
using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Database.AdoNet.Repositories;

public class ProjectAdoNetDao : IProjectDao
{
    public IDatabaseAdapter _databaseAdapter;

    public ProjectAdoNetDao(IDatabaseAdapter databaseAdapter)
    {
        _databaseAdapter = databaseAdapter;
    }
    
    public async Task<int> Create(Project project)
    {
        var databaseResult = await _databaseAdapter.ExecuteNonQueryAsync(string.Format(ProjectQueries.INSERT_PROJECT, project.Name, project.Ticker));

        if (!databaseResult.Success) return 0;

        return databaseResult.Value;
    }

    public async Task<Project?> GetByTicker(string ticker)
    {
        var databaseResult =
            await _databaseAdapter.ExecuteQueryAsync<Project>(string.Format(ProjectQueries.SELECT_PROJECT_BY_TICKER, ticker), dataRecord => ProjectModelMapper.Map(dataRecord));

        if (!databaseResult.Success || !databaseResult.Value.Any()) return null;

        return databaseResult.Value.First();
    }
}