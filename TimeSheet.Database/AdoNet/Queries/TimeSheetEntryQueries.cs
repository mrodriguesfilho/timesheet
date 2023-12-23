namespace TimeSheet.Database.AdoNet.Queries;

public class TimeSheetEntryQueries
{
    public const string SELECT_NOT_COMPLETED_ENTRIES = "SELECT id_entry, dt_work_start, dt_work_end, nr_worked_hours, nr_allocated_hours, fl_iscompleted FROM mowdb.tb_time_sheet_entry where id_employee = {0} and fl_iscompleted = false;";
    public const string INSERT_TIME_ENTRY_BASE = "INSERT INTO mowdb.tb_time_sheet_entry (id_employee, dt_work_start, dt_work_end, nr_worked_hours, nr_allocated_hours, fl_iscompleted) VALUES";
    public const string INSERT_TIME_ENTRY_VALUE = "({0}, {1}, {2}, {3}, {4}, {5}) ";
    public const string SELECT_ENTRIES_BY_DATE = "SELECT id_entry, id_employee, dt_work_start, dt_work_end, nr_worked_hours, nr_allocated_hours, fl_iscompleted FROM mowdb.tb_time_sheet_entry where id_employee = {0} and dt_work_start >= {1} and dt_work_start <= {2};";
}