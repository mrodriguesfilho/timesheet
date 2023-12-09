namespace TimeSheet.Database.AdoNet.Queries;

public static class EmployeeProjectsQueries
{
    public static string ALLOCATE_EMPLOYEE_TO_PROJECT_BASE = "INSERT INTO mowdb.tb_employee_project (id_employee, id_project, dt_allocated) VALUES ";
    public static string ALLOCATE_EMPLOYEE_TO_PROJECT_VALUES = "({0}, {1}, '{2}') ";
    public static string DEALLOCATE_EMPLOYEE_TO_PROJECT = "UPDATE mowdb.tb_employeee_project set dt_deallocated = {0} where id_employee = {1} and id_project = {2};";
    public static string SELECT_EMPLOYEE_ALLOCATED_PROJECTS_BY_ID_EMPLOYEE = "select id_employee, id_project, dt_allocated, dt_deallocated from mowdb.tb_employee_project where dt_deallocated <> null and id_employee = {0}";
}