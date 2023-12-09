using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.Mappers;
using TimeSheet.Application.StaticData;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.UseCases;

public class AllocateEmployeeToProjectUseCase : IUseCase<AllocateEmployeeToProjectInput, Result<AllocateEmployeeToProjectOutput>>
{
    private readonly IProjectFactory _projectFactory;
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IEmployeeRepository _employeeRepository;
    
    public AllocateEmployeeToProjectUseCase(IProjectFactory projectFactory, IEmployeeFactory employeeFactory, IEmployeeRepository employeeRepository)
    {
        _projectFactory = projectFactory;
        _employeeFactory = employeeFactory;
        _employeeRepository = employeeRepository;
    }
    
    public async Task<Result<AllocateEmployeeToProjectOutput>> Execute(AllocateEmployeeToProjectInput allocateEmployeeToProjectInput)
    {
        var project = await _projectFactory.Create(allocateEmployeeToProjectInput.ProjectTicker);

        if (project is null)
            return Result.Fail(
                default(AllocateEmployeeToProjectOutput),
                string.Format(ErrorMessages.ProjectNotFound,
                    allocateEmployeeToProjectInput.ProjectTicker));

        var employee = await _employeeFactory.Create(allocateEmployeeToProjectInput.EmployeeGovernmentIdentification);

        if (employee is null)
            return Result.Fail(default(AllocateEmployeeToProjectOutput),
                string.Format(ErrorMessages.EmployeeNotfound,
                    allocateEmployeeToProjectInput.EmployeeGovernmentIdentification));
        
        employee.AllocateToProject(project);
        
        await _employeeRepository.Update(employee);

        var allocateEmployeeToProjectOutput = EmployeeMapper.MapToAllocateEmployeeToProjectOutput(employee);
        
        return Result.Ok(allocateEmployeeToProjectOutput);
    }
}