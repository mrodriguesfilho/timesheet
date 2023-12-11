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
        var projectCreateTask = _projectFactory.Create(allocateEmployeeToProjectInput.ProjectTicker);
        var employeeCreateTask = _employeeFactory.Create(allocateEmployeeToProjectInput.EmployeeGovernmentIdentification);

        await Task.WhenAll(projectCreateTask, employeeCreateTask);

        var employee = employeeCreateTask.Result;
        var project = projectCreateTask.Result;
        
        if (project is null)
            return Result.Fail(
                default(AllocateEmployeeToProjectOutput),
                string.Format(ErrorMessages.ProjectNotFound,
                    allocateEmployeeToProjectInput.ProjectTicker));
        
        if (employee is null)
            return Result.Fail(default(AllocateEmployeeToProjectOutput),
                string.Format(ErrorMessages.EmployeeNotfound,
                    allocateEmployeeToProjectInput.EmployeeGovernmentIdentification));
        
        var isAllocated = employee.AllocateToProject(project);
        
        if(!isAllocated) return Result.Fail(EmployeeMapper.MapToAllocateEmployeeToProjectOutput(employee), $"Employee already allocated to project {project.Ticker}");
        
        await _employeeRepository.Update(employee);

        var allocateEmployeeToProjectOutput = EmployeeMapper.MapToAllocateEmployeeToProjectOutput(employee);
        
        return Result.Ok(allocateEmployeeToProjectOutput);
    }
}