using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Factories;

public class ProjectFactory : IProjectFactory
{
    private readonly IProjectDao _projectDao;

    public ProjectFactory(IProjectDao projectDao)
    {
        _projectDao = projectDao;
    }
    
    public async Task<Project?> Create(string ticker)
    {
        var project = await GetProjectInstance(ticker);

        if (project is null) return null;

        return project;
    }

    public async Task<Project?> Create(string ticker, string name)
    {
        var project = await GetProjectInstance(ticker, name);

        if (project is null) return null;

        return project;
    }

    private async Task<Project?> GetProjectInstance(string ticker, string name = "")
    {
        var project = await _projectDao.GetByTicker(ticker);

        if (project is not null) return project;

        if (!string.IsNullOrEmpty(ticker) && !string.IsNullOrEmpty(name)) project = Project.CreateNewProject(name, ticker);

        return project;
    }
}