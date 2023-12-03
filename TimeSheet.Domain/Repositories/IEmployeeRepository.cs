using TimeSheet.Domain.Entities;

namespace TimeSheet.Domain.Repositories;

public interface IEmployeeRepository : IRepository
{
    void Create(Employee employee);
    void Update(Employee employee);
    Task<Employee> GetById(long id);
    Task<Employee> GetByGovernmentId(string governmentIdentification);
}