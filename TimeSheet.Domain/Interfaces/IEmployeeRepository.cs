using TimeSheet.Domain.Entities;

namespace TimeSheet.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task Create(Employee employee);
    Task Update(Employee employee);
    Task UpdateProjectAllocatedHours(Employee employee, string projectTicker, DateTime dateTime);
    Task<Employee?> GetByGovernmentId(string governmentIdentification);
    Task<Employee?> GetByGovernmentIdWithTimeSheetEntries(string governmentIdentification, DateTime startDate, DateTime endDate);
}