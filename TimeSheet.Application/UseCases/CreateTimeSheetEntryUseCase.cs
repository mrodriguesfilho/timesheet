using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.Mappers;
using TimeSheet.Application.StaticData;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.UseCases;

public class CreateTimeSheetEntryUseCase : IUseCase<CreateTimeSheetEntryInput, Result<CreateTimeSheetEntryOutput>>
{
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IEmployeeRepository _employeeRepository;

    public CreateTimeSheetEntryUseCase(
        IEmployeeFactory employeeFactory, 
        IEmployeeRepository employeeRepository)
    {
        _employeeFactory = employeeFactory;
        _employeeRepository = employeeRepository;
    }
    
    public async Task<Result<CreateTimeSheetEntryOutput>> Execute(CreateTimeSheetEntryInput allocateEmployeeToProjectInput)
    {
        var employee = await _employeeFactory.Create(allocateEmployeeToProjectInput.GovernmentIdentification);

        if (employee is null)
        {
            return Result<CreateTimeSheetEntryOutput>
                .Fail(
                    default(CreateTimeSheetEntryOutput),
                    string.Format(ErrorMessages.EmployeeNotfound, allocateEmployeeToProjectInput.GovernmentIdentification));
        }
        
        employee.TimeSheet.AddTimeEntry(allocateEmployeeToProjectInput.timeEntry);

        _employeeRepository.Update(employee);
        
        var createTimeSheetEntryOutput = EmployeeMapper.MapToCreateTimeSheetEntryOutput(employee);
        
        return Result.Ok(createTimeSheetEntryOutput);
    }
}