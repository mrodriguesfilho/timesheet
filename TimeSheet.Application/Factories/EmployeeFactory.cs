using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Repositories;

namespace TimeSheet.Application.Factories;

public class EmployeeFactory : IEmployeeFactory
{
    private IEmployeeRepository _employeeRepository;

    public EmployeeFactory(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee?> Create(string governmentIdentification)
    {
        var employee = await GetEmployeeInstance(governmentIdentification);

        if (employee is null) return null;

        return employee;
    }

    public async Task<Employee?> Create(string name, string governmentIdentification)
    {
        var employee = await GetEmployeeInstance(governmentIdentification, name);

        if (employee is null) return null;
        
        return employee;
    }

    private async Task<Employee?> GetEmployeeInstance(string governmentIdentification, string name = "")
    {
        var employee = await _employeeRepository.GetByGovernmentId(governmentIdentification);

        if (employee is not null) return employee;

        if (!string.IsNullOrEmpty(name)) employee = new Employee(name, governmentIdentification);

        return employee;
    } 
}