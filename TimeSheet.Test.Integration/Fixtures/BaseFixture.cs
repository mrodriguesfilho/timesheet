using TimeSheet.Application.DTO;
using TimeSheet.Test.Integration.InMemory;

namespace TimeSheet.Test.Integration.Fixtures;

public class BaseFixture
{
    private const string GovernmentIdentification = "111-111-111-22";
    private const string EmployeeName = "João Pedro";

    private const string ProjectName = "McDonalds Project";
    private const string ProjectTicker = "MCDP";
    
    protected BaseFixture()
    {
    }

    public EmployeeInMemoryRepository CreateEmployeeRepository() => new EmployeeInMemoryRepository();

    public ProjectInMemoryDao CreateProjectDao() => new ProjectInMemoryDao();
    
    public CreateEmployeeInput GetValidEmployeeInput()
    {
        var createEmployeeInput = new CreateEmployeeInput(EmployeeName, GovernmentIdentification);
        return createEmployeeInput;
    }
    
    public CreateProjectInput GetValidProjectInput()
    {
        var createProjectInput = new CreateProjectInput(ProjectName, ProjectTicker);
        return createProjectInput;
    }
}