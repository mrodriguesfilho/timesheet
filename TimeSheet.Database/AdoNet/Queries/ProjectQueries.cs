namespace TimeSheet.Database.AdoNet.Queries;

public static class ProjectQueries
{
    public const string INSERT_PROJECT = "INSERT INTO mowdb.tb_project (nm_project, cd_project_ticker) VALUES('{0}', '{1}');";

    public const string SELECT_PROJECT_BY_TICKER =
        "SELECT ID_PROJECT, NM_PROJECT, CD_PROJECT_TICKER FROM mowdb.tb_project WHERE CD_PROJECT_TICKER = '{0}'";

    public const string SELECT_ALLOCATED_PROJECTS_BY_EMPLOYEE_ID = 
        "SELECT TP.ID_PROJECT, NM_PROJECT, CD_PROJECT_TICKER, DT_ALLOCATED, DT_DEALLOCATED FROM mowdb.tb_project tp inner join mowdb.tb_employee_project tep on tep.id_project = tp.id_project where tep.id_employee = {0}";
}