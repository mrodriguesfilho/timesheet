using TimeSheet.Domain.Entities;

namespace TimeSheet.Domain.Factories;

public interface IEmployeeFactory
{
    Task<Employee?> Create(string governmentIdentification);
    Task<Employee?> Create(string name, string governmentIdentification);
    Task<Employee?> CreateWithTimeSheetEntries(string governmentIdentification, DateTime startDate, DateTime endDate);
}