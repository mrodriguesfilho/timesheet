namespace TimeSheet.Domain.Entities;

public class Employee : BaseEntity<long>
{
    public string Name { get; init; }
    public string GovernmentIdentification { get; init; }
    
    public TimeSheetEntity TimeSheet { get; private set; }

    private readonly List<Project> _allocatedProjects;

    public Employee(string name, string governmentIdentification)
    {
        Name = name;
        GovernmentIdentification = governmentIdentification;
        TimeSheet = new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>());
        _allocatedProjects = new List<Project>();
    }
    
    public Employee(string name, string governmentIdentification, TimeSheetEntity timeSheetEntity, List<Project> allocatedProjects)
    {
        Name = name;
        GovernmentIdentification = governmentIdentification;
        TimeSheet = new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>());
        _allocatedProjects = allocatedProjects;
    }

    public void AllocateToProject(Project project)
    {
        if (!_allocatedProjects.Contains(project))
        {
            _allocatedProjects.Add(project);
        }
    }

    public Project[] GetAllocatedProjects()
    {
        return _allocatedProjects.ToArray();
    }

    public void AllocateHoursToProject(long projectId, DateTime dayOfWork, TimeSpan hoursToAllocate)
    {
        var timeSheetEntriesOfTheDay = TimeSheet.GetTimeSheetEntriesByDayOfWork(dayOfWork);

        var projectIndex = _allocatedProjects.FindIndex(x => x.Id == projectId);

        if (projectIndex < 0) return;

        _allocatedProjects[projectIndex].AllocateHours(dayOfWork, hoursToAllocate, timeSheetEntriesOfTheDay);
    }

    public Dictionary<DateTime, TimeSpan> GetHoursAllocatedByProjectId(long projectId)
    {
        var project = _allocatedProjects.FirstOrDefault(x => x.Id == projectId);

        return project.WorkedHoursByDay;
    }
}