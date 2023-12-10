using TimeSheet.Database.Models;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.Builders;

public class EmployeeBuilder
{
    private List<ProjectModel>? _projectModelList;
    
    public EmployeeBuilder BuildWithAllocatedProjects(List<ProjectModel> projectModelList)
    {
        _projectModelList = projectModelList;
        return this;
    }
    
    public Employee Build(EmployeeModel employeeModel)
    {
        List<Project> projects = null;
  
        if (_projectModelList is not null)
        {
            projects = _projectModelList.ConvertAll(projectModel => new Project(
                projectModel.Id,
                projectModel.Name,
                projectModel.Ticker,
                projectModel.AllocationDate,
                projectModel.DeallocationDate));
        }

        var employee = new Employee(
            employeeModel.Id,
            employeeModel.Name,
            employeeModel.GovernmentIdentification,
            new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()),
            projects);
        
        return employee;
    }


}