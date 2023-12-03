using System.Reflection;
using TimeSheet.Domain.Entities;
using Xunit;

namespace TimeSheet.Tests;

public class TimeSheetTest
{
    [Fact]
    public void It_should_create_a_time_entry()
    {
        var employee = new Employee("João Pedro", "111-222-333-00");
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 08, 05, 30));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 12, 05, 15));
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();
        
        Assert.Equal("João Pedro", employee.Name);
        Assert.Equal("111-222-333-00", employee.GovernmentIdentification);
        Assert.Single(workedTimeByDay);
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 04), out var timeWorked));
        Assert.Equal(new TimeSpan(3, 59, 45),timeWorked);
    }
    
    [Fact]
    public void It_should_create_time_entries_for_a_day()
    {
        var employee = new Employee("João Pedro", "111-222-333-00");
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 08, 05, 30));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 12, 05, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 13, 15, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 18, 00, 15));
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();
        
        Assert.Single(workedTimeByDay);
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 04), out var timeWorked));
        Assert.Equal(new TimeSpan(8, 44, 45),timeWorked);
    }
    
    [Fact]
    public void It_should_create_an_entry_and_leave_another_entry_open()
    {
        var employee = new Employee("João Pedro", "111-222-333-00");
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 08, 05, 30));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 12, 05, 15));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 13, 15, 15));
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();
        
        Assert.Single(workedTimeByDay);
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 04), out var timeWorked));
        Assert.Equal(new TimeSpan(3, 59, 45),timeWorked);
    }
    
    [Fact]
    public void It_shouldnt_compute_worked_hours_with_a_single_entry_in_two_consecutive_days()
    {
        var employee = new Employee("João Pedro", "111-222-333-00");
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 04, 09, 05, 30));
        employee.TimeSheet.AddTimeEntry(new DateTime(2023, 12, 05, 17, 15, 15));
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();
        
        Assert.Equal(2, workedTimeByDay.Count);
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 04), out var timeWorkedOnFirstDay));
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 05), out var timeWorkedOnSecondDay));
        Assert.Equal(TimeSpan.Zero,timeWorkedOnFirstDay);
        Assert.Equal(TimeSpan.Zero,timeWorkedOnSecondDay);
    }
}