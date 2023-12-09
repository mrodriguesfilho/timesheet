using TimeSheet.Domain.Entities;

namespace TimeSheet.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task Create(Employee employee);
    Task Update(Employee employee);
    Task<Employee?> GetById(long id);
    Task<Employee?> GetByGovernmentId(string governmentIdentification);
}