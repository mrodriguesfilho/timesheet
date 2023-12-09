using TimeSheet.Application.DTO;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Application.Mappers;

public static class EmployeeMapper
{
    public static CreateEmployeeOutput MapToCreateEmployeeOutput(Employee employee)
    {
        return new CreateEmployeeOutput(
            employee.Name, 
            employee.GovernmentIdentification,
            employee.TimeSheet,
            employee.AllocatedProjects);
    }

    public static CreateTimeSheetEntryOutput MapToCreateTimeSheetEntryOutput(Employee employee)
    {
        return new CreateTimeSheetEntryOutput(
                employee.GovernmentIdentification,
                employee.Name,
                employee.TimeSheet
        );
    }

    public static AllocateEmployeeToProjectOutput MapToAllocateEmployeeToProjectOutput(Employee employee)
    {
        return new AllocateEmployeeToProjectOutput(
            employee.GovernmentIdentification,
            employee.Name,
            employee.AllocatedProjects.ConvertAll(x => ProjectMapper.MapToCreateProjectOutput(x))
        );
    }
}