using TimeSheet.Application.DTO;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Application.Mappers;

public static class ProjectMapper
{
    public static CreateProjectOutput MapToCreateProjectOutput(Project project)
    {
        var createProjectOutput = new CreateProjectOutput();
        createProjectOutput.Id = project.Id;
        createProjectOutput.Name = project.Name;
        createProjectOutput.Ticker = project.Ticker;
        createProjectOutput.AllocationDate = project.AllocationDate;
        createProjectOutput.DeallocationDate = project.DeallocationDate;
        return createProjectOutput;
    }
    
}