using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.Mappers;
using TimeSheet.Application.StaticMessages;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.UseCases;

public class AllocateHoursToProjectUseCase : IUseCase<AllocateHoursToProjectInput, Result<AllocateHoursToProjectOutput>>
{
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IEmployeeRepository _employeeRepository;

    public AllocateHoursToProjectUseCase(IEmployeeFactory employeeFactory, IEmployeeRepository employeeRepository)
    {
        _employeeFactory = employeeFactory;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<AllocateHoursToProjectOutput>> Execute(
        AllocateHoursToProjectInput allocateEmployeeToProjectInput)
    {
        var employee = await _employeeFactory.CreateWithTimeSheetEntries(
            allocateEmployeeToProjectInput.GovernmentIdentification,
            allocateEmployeeToProjectInput.Date,
            allocateEmployeeToProjectInput.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999));

        if (employee is null)
            return Result.Fail(default(AllocateHoursToProjectOutput),
                ErrorMessages.EmployeeNotFound(allocateEmployeeToProjectInput.GovernmentIdentification));

        employee.AllocateHoursToProject(
            allocateEmployeeToProjectInput.Ticker,
            allocateEmployeeToProjectInput.Date,
            allocateEmployeeToProjectInput.HoursToAllocates);

        await _employeeRepository.UpdateProjectAllocatedHours(employee, allocateEmployeeToProjectInput.Ticker, allocateEmployeeToProjectInput.Date);

        var allocateHoursToProjectOutput = EmployeeMapper.MapToAllocateHoursToProjectOutput(employee);
        return Result.Ok(allocateHoursToProjectOutput);
    }
}