using Microsoft.EntityFrameworkCore;
using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Repositories;

namespace TimeSheet.Database.Repositories.EFCore;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _employeeDbContext;

    public EmployeeRepository(EmployeeDbContext employeeDbContext)
    {
        _employeeDbContext = employeeDbContext;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _employeeDbContext.SaveChangesAsync() > 0;
    }
    
    public void Create(Employee employee)
    {
        _employeeDbContext.Employees.Add(employee);
    }

    public void Update(Employee employee)
    {
        _employeeDbContext.Employees.Update(employee);
    }

    public async Task<Employee> GetById(long id)
    {
        var employee = await _employeeDbContext.Employees
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        return employee!;
    }

    public async Task<Employee> GetByGovernmentId(string governmentIdentification)
    {
        var employee = await _employeeDbContext.Employees
            .Where(x => x.GovernmentIdentification == governmentIdentification)
            .FirstOrDefaultAsync();

        return employee!;
    }
}