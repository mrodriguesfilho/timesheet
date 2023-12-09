using TimeSheet.Domain.Entities;

namespace TimeSheet.Domain.Interfaces;

public interface IProjectDao
{
    Task<int> Create(Project project);
    Task<Project?> GetByTicker(string ticker);
}