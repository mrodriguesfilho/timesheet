using TimeSheet.Domain.Entities;

namespace TimeSheet.Application.DTO;

public record struct CreateEmployeeOutput(string Name, string GovernmentIdentification, TimeSheetEntity TimeSheetEntity, IList<Project> AllocatedProjects);