namespace TimeSheet.Database.AdoNet.Queries;

public static class EmployeeQueries
{
    public const string INSERT_EMPLOYEE =
        "INSERT INTO mowdb.tb_employee (nm_employee, cd_employee_government) VALUES('{0}', '{1}');";

    public const string SELECT_EMPLOYEE_BY_GOVERNMENT_CODE =
        "SELECT id_employee, nm_employee, cd_employee_government FROM mowdb.tb_employee where cd_employee_government = '{0}';";
}