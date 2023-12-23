namespace TimeSheet.Database.Models;

public class TimeSheetEntryModel
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public DateTime WorkStartedAt { get; set; }
    public DateTime? WorkEndendAt { get; set; }
    public TimeSpan WorkedHours { get; set; }
    public TimeSpan AllocatedHours { get; set; }
    public bool IsCompleted { get; set; }
}