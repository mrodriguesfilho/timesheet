using TimeSheet.Domain.Entities;
using Xunit;

namespace TimeSheet.Test.Unit;

public class AllocateHoursToProjectTest
{
    [Fact]
    public void It_should_allocate_an_employee_to_a_project()
    {
        var employee = new Employee(1, "João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        var project = Project.CreateExistingProject(1, "McDonald's", "MCP");
        employee.AllocateToProject(project);
        
    }
    
    [Fact]
    public void It_should_allocate_hours_to_a_project()
    {
        var dayOfWork = new DateTime(2023, 12, 04);
        var employee = new Employee(1, "João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 08, 05, 30));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 12, 05, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 13, 15, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 18, 00, 15));
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();

        var project = new Project("McDonalds Project", "MCP", new Dictionary<DateTime, TimeSpan>());
        project.SetId(1);
        employee.AllocateToProject(project);
        employee.AllocateHoursToProject(project.Id, dayOfWork, TimeSpan.FromHours(8));
        var allocatedProjects = employee.AllocatedProjects;
        var allocatedHoursByDay = employee.GetHoursAllocatedByProjectId(1);
        
        Assert.Single(workedTimeByDay);
        Assert.Single(allocatedProjects);
        Assert.Equal("McDonalds Project", allocatedProjects[0].Name);
        Assert.True(workedTimeByDay.TryGetValue(dayOfWork, out var timeWorked));
        Assert.Equal(new TimeSpan(8, 44, 45),timeWorked);
        Assert.Single(allocatedHoursByDay);
        Assert.Equal(TimeSpan.FromHours(8), allocatedHoursByDay[dayOfWork]);
    }
    
    [Fact]
    public void It_should_allocate_hours_to_two_projects()
    {
        var dayOfWork = new DateTime(2023, 12, 04);
        var employee = new Employee(1, "João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 08, 05, 30));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 12, 05, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 13, 15, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 18, 00, 15));
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();

        var mcDonaldsProject = new Project("McDonalds Project", "MCP",new Dictionary<DateTime, TimeSpan>());
        mcDonaldsProject.SetId(1);
        employee.AllocateToProject(mcDonaldsProject);
        employee.AllocateHoursToProject(mcDonaldsProject.Id, dayOfWork, TimeSpan.FromHours(4));
        
        var burguerKingProject = new Project("Burguer King Project", "BKPJ", new Dictionary<DateTime, TimeSpan>());
        burguerKingProject.SetId(2);
        employee.AllocateToProject(burguerKingProject);
        employee.AllocateHoursToProject(burguerKingProject.Id, dayOfWork, TimeSpan.FromHours(4));
        
        var mcDonaldsAllocatedHours = employee.GetHoursAllocatedByProjectId(1);
        var burguerKingAllocatedHours = employee.GetHoursAllocatedByProjectId(2);
        
        Assert.Single(workedTimeByDay);
        Assert.True(workedTimeByDay.TryGetValue(dayOfWork, out var timeWorked));
        Assert.Equal(new TimeSpan(8, 44, 45),timeWorked);
        Assert.Single(mcDonaldsAllocatedHours);
        Assert.Equal(TimeSpan.FromHours(4), mcDonaldsAllocatedHours[dayOfWork]);
        Assert.Single(burguerKingAllocatedHours);
        Assert.Equal(TimeSpan.FromHours(4), burguerKingAllocatedHours[dayOfWork]);
    }

    [Fact]
    public void It_shouldnt_allocate_more_hours_than_worked_hours_on_projects()
    {
        var dayOfWork = new DateTime(2023, 12, 04);
        var employee = new Employee(1,"João Pedro", "111-222-333-00", new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 08, 05, 30));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 12, 05, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 13, 15, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 18, 00, 15));
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();

        var mcDonaldsProject = new Project("McDonalds Project", "MCP",new Dictionary<DateTime, TimeSpan>());
        mcDonaldsProject.SetId(1);
        employee.AllocateToProject(mcDonaldsProject);
        employee.AllocateHoursToProject(mcDonaldsProject.Id, dayOfWork, TimeSpan.FromHours(8));
        
        var burguerKingProject = new Project("Burguer King Project", "BKPJ",new Dictionary<DateTime, TimeSpan>());
        burguerKingProject.SetId(2);
        employee.AllocateToProject(burguerKingProject);
        employee.AllocateHoursToProject(burguerKingProject.Id, dayOfWork, TimeSpan.FromHours(8));
        
        var mcDonaldsAllocatedHours = employee.GetHoursAllocatedByProjectId(1);
        var burguerKingAllocatedHours = employee.GetHoursAllocatedByProjectId(2);
        
        Assert.Single(workedTimeByDay);
        Assert.True(workedTimeByDay.TryGetValue(dayOfWork, out var timeWorked));
        Assert.Equal(new TimeSpan(8, 44, 45),timeWorked);
        Assert.Single(mcDonaldsAllocatedHours);
        Assert.Equal(TimeSpan.FromHours(8), mcDonaldsAllocatedHours[dayOfWork]);
        Assert.Single(burguerKingAllocatedHours);
        Assert.Equal(new TimeSpan(0, 44, 45), burguerKingAllocatedHours[dayOfWork]);
    }
    
}