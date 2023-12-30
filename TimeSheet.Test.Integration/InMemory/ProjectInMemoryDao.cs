using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Test.Integration.InMemory;

public class ProjectInMemoryDao : IProjectDao
{
    private readonly List<Project> _projects = new();
        
    private long _sequenceId;

    private long GetNewId()
    {
        var newId = ++_sequenceId;
        return newId;
    }
    
    public Task<long> Create(Project project)
    {
        var projectAlreadyCreated = _projects.FirstOrDefault(x => x.Id == project.Id);
        if (projectAlreadyCreated is not null) return Task.FromResult(default(long));

        project.SetId(GetNewId());
        _projects.Add(project);
        return Task.FromResult(project.Id);
    }

    public Task<Project?> GetByTicker(string ticker)
    {
        var projectFound = _projects.FirstOrDefault(x => x.Ticker == ticker);
        return Task.FromResult(projectFound);
    }
}