using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.Mappers;
using TimeSheet.Application.StaticData;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.UseCases;

public class AddTimeSheetEntryUseCase : IUseCase<CreateTimeSheetEntryInput, Result<CreateTimeSheetEntryOutput>>
{
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IEmployeeRepository _employeeRepository;

    public AddTimeSheetEntryUseCase(
        IEmployeeFactory employeeFactory,
        IEmployeeRepository employeeRepository)
    {
        _employeeFactory = employeeFactory;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<CreateTimeSheetEntryOutput>> Execute(
        CreateTimeSheetEntryInput allocateEmployeeToProjectInput)
    {
        var employee = await _employeeFactory.CreateWithTimeSheetEntries(
            allocateEmployeeToProjectInput.GovernmentIdentification, DateTime.Today,
            DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));

        if (employee is null)
        {
            return Result.Fail(
                default(CreateTimeSheetEntryOutput),
                string.Format(ErrorMessages.EmployeeNotfound, allocateEmployeeToProjectInput.GovernmentIdentification));
        }

        employee.TimeSheet.AddTimeEntry(allocateEmployeeToProjectInput.timeEntry);

        await _employeeRepository.Update(employee);

        var createTimeSheetEntryOutput = EmployeeMapper.MapToCreateTimeSheetEntryOutput(employee);

        return Result.Ok(createTimeSheetEntryOutput);
    }
}