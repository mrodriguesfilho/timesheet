using System.Data;
using TimeSheet.Database.Models;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.ModelMappers;

public static class TimeSheetEntryMapper
{
    public static TimeSheetEntryModel Map(IDataRecord dataRecord)
    {
        var timeSheetEntryModel = new TimeSheetEntryModel();
        timeSheetEntryModel.Id = Convert.ToInt64(dataRecord["id_entry"]);
        timeSheetEntryModel.EmployeeId = Convert.ToInt64(dataRecord["id_employee"]);
        timeSheetEntryModel.WorkStartedAt = Convert.ToDateTime(dataRecord["dt_work_start"]);
        timeSheetEntryModel.WorkEndendAt = Convert.ToDateTime(dataRecord["dt_work_end"]);
        timeSheetEntryModel.WorkedHours = Convert.ToDateTime(dataRecord["nr_workd_hours"]);
        timeSheetEntryModel.AllocatedHours = Convert.ToDateTime(dataRecord["nr_allocated_hours"]);
        timeSheetEntryModel.IsCompleted = Convert.ToBoolean(dataRecord["fl_iscompleted"]);
        return timeSheetEntryModel;
    }
}