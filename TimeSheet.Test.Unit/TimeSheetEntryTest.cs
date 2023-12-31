using TimeSheet.Domain.Entities;
using Xunit;

namespace TimeSheet.Test.Unit;

public class TimeSheetEntryTest
{
    [Fact]
    public void It_should_create_a_time_sheet_entry_with_start_date()
    {
        var entryTime = new DateTime(2023, 12, 29, 08, 01, 52);
        var timeSheetEntry = new TimeSheetEntry(entryTime);
        
        Assert.Equal(entryTime, timeSheetEntry.StartDate);
        Assert.Null(timeSheetEntry.EndDate);
        Assert.Equal(TimeSpan.Zero, timeSheetEntry.WorkedHours);
        Assert.Equal(TimeSpan.Zero, timeSheetEntry.HoursAllocated);
        Assert.Equal(TimeSpan.Zero, timeSheetEntry.HoursAvailableToAllocate);
        Assert.False(timeSheetEntry.IsCompleted);
    } 
    
    [Fact]
    public void It_should_create_an_entry_with_start_date__and_set_end_date()
    {
        var entryTime = new DateTime(2023, 12, 29, 08, 01, 52);
        var leaveTime = new DateTime(2023, 12, 29, 12, 01, 52);
        
        var timeSheetEntry = new TimeSheetEntry(entryTime);
        timeSheetEntry.SetEndDate(leaveTime);
        
        Assert.Equal(entryTime, timeSheetEntry.StartDate);
        Assert.Equal(leaveTime, timeSheetEntry.EndDate);
        Assert.Equal(TimeSpan.FromHours(4), timeSheetEntry.WorkedHours);
        Assert.Equal(TimeSpan.FromHours(0), timeSheetEntry.HoursAllocated);
        Assert.Equal(TimeSpan.FromHours(4), timeSheetEntry.HoursAvailableToAllocate);
        Assert.True(timeSheetEntry.IsCompleted);
    } 
    
    [Fact]
    public void It_should_create_a_complete_time_sheet_entry_and_allocate_hours()
    {
        var entryTime = new DateTime(2023, 12, 29, 08, 01, 52);
        var leaveTime = new DateTime(2023, 12, 29, 12, 01, 52);
        
        var timeSheetEntry = new TimeSheetEntry(entryTime);
        timeSheetEntry.SetEndDate(leaveTime);
        timeSheetEntry.AllocateHours(TimeSpan.FromHours(4));
        
        Assert.Equal(entryTime, timeSheetEntry.StartDate);
        Assert.Equal(leaveTime, timeSheetEntry.EndDate);
        Assert.Equal(TimeSpan.FromHours(4), timeSheetEntry.WorkedHours);
        Assert.Equal(TimeSpan.FromHours(4), timeSheetEntry.HoursAllocated);
        Assert.Equal(TimeSpan.Zero, timeSheetEntry.HoursAvailableToAllocate);
        Assert.True(timeSheetEntry.IsCompleted);
    } 
    
    [Fact]
    public void It_should_create_a_time_sheet_entry_without_end_date()
    {
        var entryTime = new DateTime(2023, 12, 29, 08, 01, 52);

        var timeSheetEntry = new TimeSheetEntry(
            entryTime,
            new DateTime(),
            TimeSpan.Zero);
        
        Assert.Equal(entryTime, timeSheetEntry.StartDate);
        Assert.Null(timeSheetEntry.EndDate);
        Assert.Equal(TimeSpan.Zero, timeSheetEntry.WorkedHours);
        Assert.Equal(TimeSpan.Zero, timeSheetEntry.HoursAllocated);
        Assert.Equal(TimeSpan.Zero, timeSheetEntry.HoursAvailableToAllocate);
        Assert.False(timeSheetEntry.IsCompleted);
    } 
    
    [Fact]
    public void It_shouldnt_allow_to_set_end_date_twice()
    {
        var entryTime = new DateTime(2023, 12, 29, 08, 01, 52);
        var leaveTime = new DateTime(2023, 12, 29, 12, 01, 52);
        var secondLeaveTime = new DateTime(2023, 12, 29, 12, 01, 53);
        
        var timeSheetEntry = new TimeSheetEntry(entryTime);
        timeSheetEntry.SetEndDate(leaveTime);
        timeSheetEntry.SetEndDate(secondLeaveTime);
        
        Assert.Equal(entryTime, timeSheetEntry.StartDate);
        Assert.Equal(leaveTime, timeSheetEntry.EndDate);
        Assert.Equal(TimeSpan.FromHours(4), timeSheetEntry.WorkedHours);
        Assert.Equal(TimeSpan.FromHours(0), timeSheetEntry.HoursAllocated);
        Assert.Equal(TimeSpan.FromHours(4), timeSheetEntry.HoursAvailableToAllocate);
        Assert.True(timeSheetEntry.IsCompleted);
    } 
}