namespace TimeSheet.Application.StaticMessages;

public static class ErrorMessages
{
    private static string _employeeAlreadyExistsMsg = "Employee with Governement Id [{0}] already exists";
    public static string _employeeNotfoundMsg = "Employee with Governement Id [{0}] not found";
    private static string _projectAlreadyExistsMsg = "Project with Ticker [{0}] already exists";
    public static string _projectNotFoundMsg = "Project with Ticker [{0}] not found";
    public static string UnableToCreateProject = "Unable to create project with Name [{0}] and Ticker [{0}]";
    private static string _employeeAlreadyAllocatedToProjectMsg = "Employee already allocated to project [{0}]";
    
    public static string EmployeeAlreadyExists(string governmentId)
    {
        return string.Format(_employeeAlreadyExistsMsg, governmentId);
    }

    public static string EmployeeNotFound(string governmentId)
    {
        return string.Format(_employeeNotfoundMsg, governmentId);
    }
    
    public static string ProjectAlreadyExists(string ticker)
    {
        return string.Format(_projectAlreadyExistsMsg, ticker);
    }

    public static string ProjectNotFound(string ticker)
    {
        return string.Format(_projectNotFoundMsg, ticker);
    }

    public static string EmployeeAlreadyAllocatedToProject(string ticker)
    {
        return string.Format(_employeeAlreadyAllocatedToProjectMsg, ticker);    
    }
}