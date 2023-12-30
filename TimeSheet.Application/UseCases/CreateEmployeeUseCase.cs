using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.Mappers;
using TimeSheet.Application.StaticMessages;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.UseCases;

public class CreateEmployeeUseCase : IUseCase<CreateEmployeeInput, Result<CreateEmployeeOutput>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeFactory _employeeFactory;
    
    public CreateEmployeeUseCase(IEmployeeRepository employeeRepository, IEmployeeFactory employeeFactory)
    {
        _employeeRepository = employeeRepository;
        _employeeFactory = employeeFactory;
    }

    public async Task<Result<CreateEmployeeOutput>> Execute(CreateEmployeeInput allocateEmployeeToProjectInput)
    {
        var employee = await _employeeFactory.Create(allocateEmployeeToProjectInput.Name, allocateEmployeeToProjectInput.GovernmentIdentification);
        
        if (employee.Id != default)
        {
            var employeeAlreadyExistsOutput = EmployeeMapper.MapToCreateEmployeeOutput(employee);
            return Result.Fail(employeeAlreadyExistsOutput, ErrorMessages.EmployeeAlreadyExists(employee.GovernmentIdentification));
        }

        await _employeeRepository.Create(employee);
        var createEmployeeOutput = EmployeeMapper.MapToCreateEmployeeOutput(employee);
        return Result.Ok(createEmployeeOutput);
    }
}