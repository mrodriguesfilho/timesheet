namespace TimeSheet.Database.AdoNet.Queries;

public class TimeSheetEntryQueries
{
    public const string SELECT_NOT_COMPLETED_ENTRIES = "SELECT id_entry, dt_start, dt_end, nr_worked_hours, nr_allocated_hours, fl_iscompleted FROM mowdb.tb_time_sheet_entry where id_employee = {0} and fl_iscompleted = false;";
    public const string INSERT_TIME_ENTRY_BASE = "INSERT INTO mowdb.tb_time_sheet_entry (dt_start, dt_end, nr_worked_hours, nr_allocated_hours, fl_iscompleted) VALUES";
    public const string INSERT_TIME_ENTRY_VALUE = "({0}, '{1}', '{2}', '{3}', {4}) ";
}