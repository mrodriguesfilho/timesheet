namespace TimeSheet.Application.DTO;

public record struct AllocateEmployeeToProjectInput(string EmployeeGovernmentIdentification, string ProjectTicker);
