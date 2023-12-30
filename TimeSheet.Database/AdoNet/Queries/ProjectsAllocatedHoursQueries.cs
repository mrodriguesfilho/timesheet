namespace TimeSheet.Database.AdoNet.Queries;

public static class ProjectsAllocatedHoursQueries
{
    private static string SELECT_ALLOCATED_HOURS_BY_PROJECT_TICKER_DAY_OF_WORK_AND_EMPLOYEE_ID_QUERY =
        "select id_employee, id_project, dt_allocationm nr_allocated_hours from tb_project_hours_allocation where id_employee = {0}, cd_project_ticker = {1}, dt_allocation = {2};";

    public static string SELECT_ALLOCATED_HOURS_BY_ID_EMPLOYEE_PROJECT_TICKER_AND_DAY_OF_WORK(long employeeId, string projectTicker, DateTime workDay)
    {
        return string.Format(SELECT_ALLOCATED_HOURS_BY_PROJECT_TICKER_DAY_OF_WORK_AND_EMPLOYEE_ID_QUERY, employeeId, projectTicker, workDay);
    }
}