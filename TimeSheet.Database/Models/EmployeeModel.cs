namespace TimeSheet.Database.Models;

public class EmployeeModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string GovernmentIdentification { get; set; }
    public ICollection<ProjectModel> AllocatedProjects { get; set; }
}