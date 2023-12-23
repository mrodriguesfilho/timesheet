using TimeSheet.Domain.Entities;
using Xunit;

namespace TimeSheet.Test.Unit;

public class AllocateEmployeeToProjectTest
{
    [Fact]
    public void It_should_allocate_an_employee_to_a_project()
    {
        var employee = new Employee(1, "João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        var project = Project.CreateExistingProject(1, "McDonald's", "MCP");
        var employeeAllocated = employee.AllocateToProject(project);
        
        Assert.True(employeeAllocated);
        Assert.Single(employee.AllocatedProjects);
        Assert.Equal("McDonald's", employee.AllocatedProjects[0].Name);
        Assert.Equal("MCP", employee.AllocatedProjects[0].Ticker);
        Assert.Equal(DateTime.Today, employee.AllocatedProjects[0].AllocationDate.Date);
    }
    
    [Fact]
    public void It_should_deallocate_an_employee_from_a_project()
    {
        var employee = new Employee(1, "João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        var project = Project.CreateExistingProject(1, "McDonald's", "MCP");
        var employeeAllocated = employee.AllocateToProject(project);
        var employeeDeallocated = employee.DeallocateFromProject(project.Id);
        Assert.True(employeeAllocated);
        Assert.True(employeeDeallocated);
        Assert.Single(employee.AllocatedProjects);
        Assert.Equal("McDonald's", employee.AllocatedProjects[0].Name);
        Assert.Equal("MCP", employee.AllocatedProjects[0].Ticker);
        Assert.Equal(DateTime.Today, employee.AllocatedProjects[0].AllocationDate.Date);
        Assert.Equal(DateTime.Today, employee.AllocatedProjects[0].DeallocationDate.GetValueOrDefault().Date);
    }
    
    [Fact]
    public void It_shouldnt_allocate_an_employee_to_an_already_allocated_project()
    {
        var employee = new Employee(1, "João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        var project = Project.CreateExistingProject(1, "McDonald's", "MCP");
        var employeeAllocated1 = employee.AllocateToProject(project);
        var employeeAllocated2 = employee.AllocateToProject(project);
        
        Assert.True(employeeAllocated1);
        Assert.False(employeeAllocated2);
        Assert.Single(employee.AllocatedProjects);
        Assert.Equal("McDonald's", employee.AllocatedProjects[0].Name);
        Assert.Equal("MCP", employee.AllocatedProjects[0].Ticker);
        Assert.Equal(DateTime.Today, employee.AllocatedProjects[0].AllocationDate.Date);
    }

    [Fact]
    public void It_shouldnt_deallocate_an_employee_if_he_isnt_allocated()
    {
        var employee = new Employee(1, "João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        var project = Project.CreateExistingProject(1, "McDonald's", "MCP");
        var employeeDeallocated = employee.DeallocateFromProject(project.Id);
        
        Assert.False(employeeDeallocated);
        Assert.Empty(employee.AllocatedProjects);
    }
}
