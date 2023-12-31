namespace TimeSheet.Database.AdoNet.Models;

public class HoursAllocatedToProjectModel
{
    public long EmployeeId { get; set; }
    public long ProjectId { get; set; }
    public DateTime DayOfAllocation { get; set; }
    public TimeSpan HoursAllocated { get; set; }
}