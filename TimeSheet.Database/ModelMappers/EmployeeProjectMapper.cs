using System.Data;
using TimeSheet.Database.Extensions;
using TimeSheet.Database.Models;

namespace TimeSheet.Database.ModelMappers;

public static class EmployeeProjectMapper
{
    public static EmployeeProjectModel Map(IDataRecord dataRecord)
    {
        var employeeProjectModel = new EmployeeProjectModel();
        employeeProjectModel.EmployeeId = Convert.ToInt64(dataRecord["id_employee"]);
        employeeProjectModel.ProjectId = Convert.ToInt64(dataRecord["id_project"]);
        var allocatedDate = Convert.ToString(dataRecord["dt_allocated"]);
        
        employeeProjectModel.DateAllocation = DateTimeExtension.FromPT0H0M0S(allocatedDate);
        var deallocatedDate = Convert.ToString(dataRecord["dt_dealloacted"]);
        
        if(!string.IsNullOrEmpty(deallocatedDate)) employeeProjectModel.DateDeallocation = DateTimeExtension.FromPT0H0M0S(deallocatedDate);

        return employeeProjectModel;
    }
}