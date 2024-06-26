using System.Data;
using TimeSheet.Database.AdoNet.Models;

namespace TimeSheet.Database.AdoNet.Mappers;

public static class EmployeeProjectMapper
{
    public static EmployeeProjectModel Map(IDataRecord dataRecord)
    {
        var employeeProjectModel = new EmployeeProjectModel();
        employeeProjectModel.EmployeeId = Convert.ToInt64(dataRecord["id_employee"]);
        employeeProjectModel.ProjectId = Convert.ToInt64(dataRecord["id_project"]);
        employeeProjectModel.DateAllocation = Convert.ToDateTime(dataRecord["dt_allocated"]);
        employeeProjectModel.DateDeallocation = dataRecord["dt_dellocated"] == DBNull.Value ? null : Convert.ToDateTime(dataRecord["dt_dealloacted"]);
        
        return employeeProjectModel;
    }
}