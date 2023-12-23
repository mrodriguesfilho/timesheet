using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Test.Integration.InMemory;

public class EmployeeInMemoryRepository : IEmployeeRepository
{
    private long SequenceId = 0;
    
    private static readonly List<Employee> _employees = new();

    private long GetNewId()
    {
        var newId = ++SequenceId;
        return newId;
    }
    
    public Task Create(Employee employee)
    {
        var employeeAlreadyAdded = _employees.FirstOrDefault(x => x.Id == employee.Id);
        
        if (employeeAlreadyAdded is not null) return Task.FromResult(employeeAlreadyAdded);

        employee.SetId(GetNewId());
        _employees.Add(employee);

        return Task.CompletedTask;
    }

    public Task Update(Employee employee)
    {
        var employeeIndex = _employees.FindIndex(x => x.Id == employee.Id);
        
        if (employeeIndex == -1) return Task.CompletedTask;

        _employees[employeeIndex] = employee;
        
        return Task.CompletedTask;
    }

    public Task<Employee?> GetByGovernmentId(string governmentIdentification)
    {
        var employeeFound = _employees.FirstOrDefault(x => x.GovernmentIdentification == governmentIdentification);
        return Task.FromResult(employeeFound);
    }

    public Task<Employee?> GetByGovernmentIdWithTimeSheetEntries(string governmentIdentification, DateTime startDate, DateTime endDate)
    {
        var employeeFound = _employees.FirstOrDefault(x => x.GovernmentIdentification == governmentIdentification);

        if (employeeFound is null) return Task.FromResult(employeeFound);

        var timeSheetEntries = employeeFound.TimeSheet?.GetAllTimeSheetEntries();

        var filteredTimeSheetEntries = timeSheetEntries
            .Where(x => x.StartDate >= startDate && x.EndDate <= endDate);

        var timeSheet = new Dictionary<DateTime, List<TimeSheetEntry>>();
        foreach (var filteredTimeSheetEntry in filteredTimeSheetEntries)
        {
            if (timeSheet.TryGetValue(filteredTimeSheetEntry.StartDate, out var dayTimeSheetEntries))
            {
                dayTimeSheetEntries.Add(filteredTimeSheetEntry);
                continue;
            }
            
            timeSheet.Add(filteredTimeSheetEntry.StartDate.Date, new List<TimeSheetEntry>(){ filteredTimeSheetEntry });
        }

        var timeSheetEntity = new TimeSheetEntity(timeSheet);
        employeeFound.SetTimeSheet(timeSheetEntity);
        return Task.FromResult(employeeFound)!;
    }
}