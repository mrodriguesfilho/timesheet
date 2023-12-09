namespace TimeSheet.Application.DTO;

public class EmployeeDTO
{
    public string Name { get; set; }
    public string GovernmentIdentification { get; set; }
    public List<ProjectDTO> AllocatedProjects { get; set; }
}