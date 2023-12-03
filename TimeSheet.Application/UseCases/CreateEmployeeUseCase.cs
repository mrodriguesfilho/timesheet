using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.Mappers;
using TimeSheet.Application.StaticData;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Repositories;

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

    public async Task<Result<CreateEmployeeOutput>> Execute(CreateEmployeeInput createTimeSheetEntryInput)
    {
        var employee = await _employeeFactory.Create(createTimeSheetEntryInput.Name, createTimeSheetEntryInput.GovernmentIdentification);

        if (employee.Id != default)
        {
            var employeeAlreadyExistsOutput = EmployeeMapper.CreateEmployeeOutput(employee);
            return Result<CreateEmployeeOutput>.Fail(employeeAlreadyExistsOutput, string.Format(ErrorMessages.EmployeeAlreadyExists, employee.GovernmentIdentification));
        }

        _employeeRepository.Create(employee);
        await _employeeRepository.SaveChangesAsync();
        var createEmployeeOutput = EmployeeMapper.CreateEmployeeOutput(employee);
        return Result.Ok(createEmployeeOutput);
    }
}