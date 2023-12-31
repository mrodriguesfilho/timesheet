namespace TimeSheet.Database.AdoNet.Models;

public class EmployeeProjectModel
{
    public long EmployeeId { get; set; }
    public long ProjectId { get; set; }
    public DateTime DateAllocation { get; set; }
    public DateTime? DateDeallocation { get; set; }
}