namespace TimeSheet.Domain.Entities;

public class Employee : BaseEntity<long>
{
    public string Name { get; init; }
    public string GovernmentIdentification { get; init; }
    public TimeSheetEntity TimeSheet { get; private set; }
    public List<Project> AllocatedProjects { get; init; }

    public Employee(string name, string governmentIdentification)
    {
        Name = name;
        GovernmentIdentification = governmentIdentification;
        TimeSheet = new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>());
        AllocatedProjects = new List<Project>();
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

    public void AllocateHoursToProject(long projectId, DateTime dayOfWork, TimeSpan hoursToAllocate)
    {
        var timeSheetEntriesOfTheDay = TimeSheet.GetTimeSheetEntriesByDayOfWork(dayOfWork);

        var projectIndex = AllocatedProjects.FindIndex(x => x.Id == projectId);

        if (projectIndex < 0) return;

        AllocatedProjects[projectIndex].AllocateHours(dayOfWork, hoursToAllocate, timeSheetEntriesOfTheDay);
    }

    public Dictionary<DateTime, TimeSpan> GetHoursAllocatedByProjectId(long projectId)
    {
        var project = AllocatedProjects.FirstOrDefault(x => x.Id == projectId);

        return project.WorkedHoursByDay;
    }
}