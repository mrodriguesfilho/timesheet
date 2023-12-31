using TimeSheet.Database.AdoNet.Models;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.AdoNet.Builders;

public class EmployeeBuilder
{
    private List<ProjectModel>? _projectModelList;
    private List<TimeSheetEntryModel>? _timeSheetEntryModelList;
    
    public EmployeeBuilder BuildWithAllocatedProjects(List<ProjectModel> projectModelList)
    {
        _projectModelList = projectModelList;
        return this;
    }
    
    public EmployeeBuilder BuildWithTimeSheetEntries(List<TimeSheetEntryModel> timeSheetEntryModelList)
    {
        _timeSheetEntryModelList = timeSheetEntryModelList;
        return this;
    }
    
    public Employee Build(EmployeeModel employeeModel)
    {
        List<Project> projects = new();
        TimeSheetEntity timeSheetEntity = new();
        
        if (_projectModelList is not null)
        {
            projects = _projectModelList.ConvertAll(projectModel => Project.CreateExistingProjectWithAllocation(
                projectModel.Id,
                projectModel.Name,
                projectModel.Ticker,
                projectModel.AllocationDate,
                projectModel.DeallocationDate));
        }

        if (_timeSheetEntryModelList is not null)
        {
            var timeSheetEntryList = _timeSheetEntryModelList.ConvertAll(timeEntry => new TimeSheetEntry(
                    timeEntry.WorkStartedAt,
                    timeEntry.WorkEndendAt,
                    timeEntry.AllocatedHours
                ));

            var timeSheetEntriesByDay = new Dictionary<DateTime, List<TimeSheetEntry>>();
            foreach (var timeSheetEntry in timeSheetEntryList)
            {
                if (timeSheetEntriesByDay.TryGetValue(timeSheetEntry.StartDate.Date, out var timeSheetEntries))
                    timeSheetEntries.Add(timeSheetEntry);
                
                timeSheetEntriesByDay.Add(timeSheetEntry.StartDate.Date, new List<TimeSheetEntry>(){ timeSheetEntry });
            }

            timeSheetEntity = new TimeSheetEntity(timeSheetEntriesByDay);
        }
        
        var employee = new Employee(
            employeeModel.Id,
            employeeModel.Name,
            employeeModel.GovernmentIdentification,
            timeSheetEntity,
            projects);
        
        return employee;
    }
}