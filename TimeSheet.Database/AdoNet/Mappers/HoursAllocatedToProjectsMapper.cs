using System.Data;
using TimeSheet.Database.AdoNet.Models;

namespace TimeSheet.Database.AdoNet.Mappers;

public static class HoursAllocatedToProjectsMapper
{
    public static HoursAllocatedToProjectModel Map(IDataRecord dataRecord)
    {
        var hoursAllocateToProjectModel = new HoursAllocatedToProjectModel();
        hoursAllocateToProjectModel.EmployeeId = Convert.ToInt64(dataRecord["id_employee"]);
        hoursAllocateToProjectModel.ProjectId = Convert.ToInt64(dataRecord["id_project"]);
        hoursAllocateToProjectModel.DayOfAllocation = Convert.ToDateTime(dataRecord["dt_allocation"]);
        var hoursAllocated = Convert.ToString(dataRecord["nr_hours_allocated"]);
        if(hoursAllocated is not null) hoursAllocateToProjectModel.HoursAllocated = TimeSpan.Parse(hoursAllocated);
        return hoursAllocateToProjectModel;
    }
}