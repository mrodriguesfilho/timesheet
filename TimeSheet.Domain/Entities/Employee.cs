namespace TimeSheet.Domain.Entities;

public class Employee : BaseEntity<long>
{
    public string Name { get; init; }
    public string GovernmentIdentification { get; init; }
    public TimeSheetEntity TimeSheet { get; private set; }
    public List<Project> AllocatedProjects { get; init; }
    
    public static Employee CreateNewEmployee(string name, string governmentIdentification)
    {
        var newEmployee = new Employee(0, name, governmentIdentification, new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        return newEmployee;
    }
    
    public Employee(long id, string name, string governmentIdentification, TimeSheetEntity timeSheetEntity, List<Project>? allocatedProjects)
    {
        Id = id;
        Name = name;
        GovernmentIdentification = governmentIdentification;
        TimeSheet = new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>());
        AllocatedProjects = allocatedProjects ?? new List<Project>();
    }

    public bool AllocateToProject(Project project)
    {
        if (AllocatedProjects.FirstOrDefault(x => x.Id == project.Id) is not null) return false;
        
        project.SetAllocationDate();
        AllocatedProjects.Add(project);
        return true;

    }
    
    public bool DeallocateFromProject(long projectId)
    {
        var index = AllocatedProjects.FindIndex(x => x.Id == projectId);

        if (index == -1) return false;

        AllocatedProjects[index].SetDeallocationDate();

        return true;
    }
    
    public void AllocateHoursToProject(string projectTicker, DateTime dayOfWork, TimeSpan hoursToAllocate)
    {
        var timeSheetEntriesOfTheDay = TimeSheet.GetTimeSheetEntriesByDayOfWork(dayOfWork);

        var projectIndex = AllocatedProjects.FindIndex(x => x.Ticker == projectTicker);

        if (projectIndex < 0) return;

        AllocatedProjects[projectIndex].AllocateHours(dayOfWork, hoursToAllocate, timeSheetEntriesOfTheDay);
    }

    public Dictionary<DateTime, TimeSpan> GetHoursAllocatedByProjectTicker(string projectTicker)
    {
        var project = AllocatedProjects.FirstOrDefault(x => x.Ticker == projectTicker);
        return project?.WorkedHoursByDay;
    }

    public void SetTimeSheet(TimeSheetEntity timeSheet)
    {
        TimeSheet = timeSheet;
    }
}