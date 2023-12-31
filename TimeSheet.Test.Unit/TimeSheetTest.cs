using TimeSheet.Domain.Entities;
using Xunit;

namespace TimeSheet.Test.Unit;

public class TimeSheetTest
{
    [Fact]
    public void It_should_create_a_time_entry()
    {
        var employee = Employee.CreateNewEmployee("João Pedro", "111-222-333-00");
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
        var employee = Employee.CreateNewEmployee("João Pedro", "111-222-333-00");
        var arriveAtWorkTimeEntry = new DateTime(2023, 12, 04, 08, 05, 30);
        var lunchLeaveTimeEntry = new DateTime(2023, 12, 04, 12, 05, 15);
        var lunchReturnTimeEntry = new DateTime(2023, 12, 04, 13, 15, 15);
        var finishedWorkTimeEntry = new DateTime(2023, 12, 04, 18, 00, 15);
        employee.TimeSheet.AddTimeEntry(arriveAtWorkTimeEntry);
        employee.TimeSheet.AddTimeEntry(lunchLeaveTimeEntry);
        employee.TimeSheet.AddTimeEntry(lunchReturnTimeEntry);
        employee.TimeSheet.AddTimeEntry(finishedWorkTimeEntry);
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();
        var timeSheetEntries = employee.TimeSheet.GetAllTimeSheetEntries();
        
        Assert.Single(workedTimeByDay);
        Assert.Equal(2, timeSheetEntries.Count);
        Assert.Equal(arriveAtWorkTimeEntry, timeSheetEntries[0].StartDate);
        Assert.Equal(lunchLeaveTimeEntry, timeSheetEntries[0].EndDate);
        Assert.Equal(lunchReturnTimeEntry, timeSheetEntries[1].StartDate);
        Assert.Equal(finishedWorkTimeEntry, timeSheetEntries[1].EndDate);
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 04), out var timeWorked));
        Assert.Equal(new TimeSpan(8, 44, 45),timeWorked);
    }
    
    [Fact]
    public void It_should_create_an_entry_and_leave_another_entry_open()
    {
        var employee = Employee.CreateNewEmployee("João Pedro", "111-222-333-00");
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
        var employee = Employee.CreateNewEmployee("João Pedro", "111-222-333-00");
        var arriveAtWorkTimeEntryDay1 = new DateTime(2023, 12, 04, 09, 05, 30);
        var arriveAtWorkTimeEntryDay2 = new DateTime(2023, 12, 05, 09, 05, 30);
        employee.TimeSheet.AddTimeEntry(arriveAtWorkTimeEntryDay1);
        employee.TimeSheet.AddTimeEntry(arriveAtWorkTimeEntryDay2);
        var workedTimeByDay = employee.TimeSheet.CalculateWorkedTime();
        var timeSheetEntries = employee.TimeSheet.GetAllTimeSheetEntries();
        
        Assert.Equal(2, workedTimeByDay.Count);
        Assert.Equal(2, timeSheetEntries.Count);
        Assert.Equal(arriveAtWorkTimeEntryDay1, timeSheetEntries[0].StartDate);
        Assert.Null(timeSheetEntries[0].EndDate);
        Assert.Equal(arriveAtWorkTimeEntryDay2, timeSheetEntries[1].StartDate);
        Assert.Null(timeSheetEntries[1].EndDate);
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 04), out var timeWorkedOnFirstDay));
        Assert.True(workedTimeByDay.TryGetValue(new DateTime(2023, 12, 05), out var timeWorkedOnSecondDay));
        Assert.Equal(TimeSpan.Zero,timeWorkedOnFirstDay);
        Assert.Equal(TimeSpan.Zero,timeWorkedOnSecondDay);
    }
    
}