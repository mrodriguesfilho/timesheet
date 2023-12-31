namespace TimeSheet.Database.AdoNet.Queries;

public static class HoursAllocatedToProjectQueries
{
    private static string SELECT_ALLOCATED_HOURS_BY_PROJECT_TICKER_DAY_OF_WORK_AND_EMPLOYEE_ID_QUERY =
        "select id_employee, id_project, dt_allocation, nr_allocated_hours from mowdb.tb_hours_allocated_to_project where id_employee = {0} and id_project = {1} and dt_allocation = {2};";

    private static string INSERT_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_WORK_DAY_QUERY =
        "insert into mowdb.tb_hours_allocated_to_project (id_employee, id_project, dt_allocation, nr_allocated_hours) VALUES ({0}, {1}, {2}, {3});";

    private static string UPDATE_NUMBER_OF_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_WORK_DAY_QUERY =
        "update mowdb.tb_hours_allocated_to_project set nr_allocated_hours = {0} where id_employee = {1} and id_project = {2} and dt_allocation = {3};";
    
    public static string SELECT_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_DAY_OF_WORK(long employeeId, long projectId, string workDay)
        => string.Format(SELECT_ALLOCATED_HOURS_BY_PROJECT_TICKER_DAY_OF_WORK_AND_EMPLOYEE_ID_QUERY, employeeId, projectId, workDay);

    public static string INSERT_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_WORK_DAY(long employeeId,
        long projectId, string workDay, string allocatedHours)
        => string.Format(INSERT_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_WORK_DAY_QUERY, employeeId,
            projectId, workDay, allocatedHours);

    public static string UPDATE_NUMBER_OF_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_WORK_DAY(long employeeId,
        long projectId, string workDay, string allocatedHours)
        => string.Format(UPDATE_NUMBER_OF_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_WORK_DAY_QUERY,
            allocatedHours, employeeId, projectId, workDay);

}