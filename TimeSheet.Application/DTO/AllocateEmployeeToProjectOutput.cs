namespace TimeSheet.Application.DTO;

public record struct AllocateEmployeeToProjectOutput(string GovernmentIdentfication, string Name, List<CreateProjectOutput> AllocatedProjects);