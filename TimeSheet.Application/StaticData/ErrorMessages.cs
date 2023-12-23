namespace TimeSheet.Application.StaticData;

public static class ErrorMessages
{
    public static string EmployeeAlreadyExistsMsg = "Employee with Governement Id [{0}] already exists";
    public static string EmployeeNotfound = "Employee with Governement Id [{0}] not found";
    public static string ProjectAlreadyExists = "Project with Ticker [{0}] already exists";
    public static string ProjectNotFound = "Project with Ticker [{0}] not found";
    public static string UnableToCreateProject = "Unable to create project with Name [{0}] and Ticker [{0}]";

    public static string EmployeeAlreadyExists(string governementId)
    {
        return string.Format(EmployeeAlreadyExistsMsg, governementId);
    }
}