using TimeSheet.Application.DTO;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Application.Mappers;

public static class EmployeeMapper
{
    public static CreateEmployeeOutput CreateEmployeeOutput(Employee employee)
    {
        return new CreateEmployeeOutput(
            employee.Name, 
            employee.GovernmentIdentification,
            employee.TimeSheet,
            employee.GetAllocatedProjects());
    }

    public static CreateTimeSheetEntryOutput CreateTimeSheetEntryOutput(Employee employee)
    {
        return new CreateTimeSheetEntryOutput(
                employee.GovernmentIdentification,
                employee.Name,
                employee.TimeSheet
            );
    }
}