using TimeSheet.Database.AdoNet.Adapters.Interface;
using TimeSheet.Database.AdoNet.Queries;
using TimeSheet.Database.ModelMappers;
using TimeSheet.Database.Models;
using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Database.AdoNet.DAO;

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
            await _databaseAdapter.ExecuteQueryAsync<ProjectModel>(string.Format(ProjectQueries.SELECT_PROJECT_BY_TICKER, ticker), dataRecord => ProjectModelMapper.Map(dataRecord));

        if (!databaseResult.Success || !databaseResult.Value.Any()) return null;

        var projectModel = databaseResult.Value.FirstOrDefault();
        var project = new Project(
            projectModel.Id,
            projectModel.Name,
            projectModel.Ticker
            );
        
        return project;
    }
}