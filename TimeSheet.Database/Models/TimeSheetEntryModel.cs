namespace TimeSheet.Database.Models;

public class TimeSheetEntryModel
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public DateTime WorkStartedAt { get; set; }
    public DateTime WorkEndendAt { get; set; }
    public DateTime WorkedHours { get; set; }
    public DateTime AllocatedHours { get; set; }
    public bool IsCompleted { get; set; }
}