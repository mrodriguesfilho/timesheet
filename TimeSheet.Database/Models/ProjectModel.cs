namespace TimeSheet.Database.Models;

public class ProjectModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Ticker { get; set; }
    public ICollection<EmployeeModel> EmployeesAllocated { get; set; }
}