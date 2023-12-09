using TimeSheet.Domain.Entities;

namespace TimeSheet.Domain.Factories;

public interface IProjectFactory
{
    Task<Project?> Create(string ticker);
    Task<Project?> Create(string ticker, string name);
}