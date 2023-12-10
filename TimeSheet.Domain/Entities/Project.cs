namespace TimeSheet.Domain.Entities;

public class Project : BaseEntity<long>
{
    public string Name { get; private set; }
    public string Ticker { get; private set; }
    public DateTime AllocationDate { get; private set; }
    public DateTime? DeallocationDate { get; private set; }
    public Dictionary<DateTime, TimeSpan> WorkedHoursByDay { get; init; }

    private Project(){}
    
    public static Project CreateNewProject(string name, string ticker)
    {
        var newProject = new Project();
        newProject.Name = name;
        newProject.Ticker = ticker;
        return newProject;
    }

    public static Project CreateExistingProject(long id, string name, string ticker)
    {
        var existingProject = CreateNewProject(name, ticker);
        existingProject.Id = id;
        return existingProject;
    }
    
    public static Project CreateExistingProjectWithAllocation(
        long id, 
        string name, 
        string ticker, 
        DateTime allocationDate, 
        DateTime? deAllocationDate = null)
    {
        var existingProjectWithAllocation = CreateExistingProject(id, name, ticker);
        existingProjectWithAllocation.AllocationDate = allocationDate;
        existingProjectWithAllocation.DeallocationDate = deAllocationDate;
        return existingProjectWithAllocation;
    }
    
    public Project(string name, string ticker, Dictionary<DateTime, TimeSpan> workedHoursByDay)
    {
        Name = name;
        Ticker = ticker;
        WorkedHoursByDay = workedHoursByDay;
    }

    public void AllocateHours(DateTime dayOfWork, TimeSpan hoursToAllocate, List<TimeSheetEntry> timeSheetEntries)
    {
        foreach (var timeSheetEntry in timeSheetEntries)
        {
            var ticksAvailableToAllocate = Math.Min(hoursToAllocate.Ticks, timeSheetEntry.HoursAvailableToAllocate.Ticks);

            if (ticksAvailableToAllocate == 0) continue;
            
            var hoursAvailableToAllocate = TimeSpan.FromTicks(ticksAvailableToAllocate);
            
            timeSheetEntry.AllocateHours(hoursAvailableToAllocate);

            hoursToAllocate -= hoursAvailableToAllocate;
            
            if (WorkedHoursByDay.TryGetValue(dayOfWork, out var workedHours))
            {
                workedHours += hoursAvailableToAllocate;
                WorkedHoursByDay[dayOfWork] = workedHours;
                continue;
            }
        
            WorkedHoursByDay.Add(dayOfWork, hoursAvailableToAllocate);
        }
    }

    public void SetAllocationDate()
    {
        AllocationDate = DateTime.Now;
    }

    public void SetDeallocationDate()
    {
        DeallocationDate = DateTime.Now;
    }
}