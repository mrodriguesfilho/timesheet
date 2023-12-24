namespace TimeSheet.Application.StaticData;

public static class ErrorMessages
{
    private static string _employeeAlreadyExistsMsg = "Employee with Governement Id [{0}] already exists";
    public static string EmployeeNotfound = "Employee with Governement Id [{0}] not found";
    private static string _projectAlreadyExistsMsg = "Project with Ticker [{0}] already exists";
    public static string ProjectNotFound = "Project with Ticker [{0}] not found";
    public static string UnableToCreateProject = "Unable to create project with Name [{0}] and Ticker [{0}]";

    public static string EmployeeAlreadyExists(string governementId)
    {
        return string.Format(_employeeAlreadyExistsMsg, governementId);
    }

    public static string ProjectAlreadyExists(string ticker)
    {
        return string.Format(_projectAlreadyExistsMsg, ticker);
    }
}