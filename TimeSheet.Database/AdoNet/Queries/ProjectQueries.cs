namespace TimeSheet.Database.AdoNet.Queries;

public static class ProjectQueries
{
    public const string INSERT_PROJECT = "INSERT INTO mowdb.tb_project (nm_project, cd_project_ticker) VALUES('{0}', '{1}');";

    public const string SELECT_PROJECT_BY_TICKER =
        "SELECT ID_PROJECT, NM_PROJECT, CD_PROJECT_TICKER FROM mowdb.tb_project WHERE CD_PROJECT_TICKER = '{0}'";
}