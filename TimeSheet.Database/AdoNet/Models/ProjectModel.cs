namespace TimeSheet.Database.AdoNet.Models;

public class ProjectModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Ticker { get; set; }
    public DateTime AllocationDate { get; set; }
    public DateTime? DeallocationDate { get; set; }
}