namespace TimeSheet.Domain.Entities;

public class Project : BaseEntity<long>
{
    public string Name { get; init; }
    public string Ticker { get; init; }
    public Dictionary<DateTime, TimeSpan> WorkedHoursByDay { get; init; }

    public Project(string name, string ticker)
    {
        Name = name;
        Ticker = ticker;
    }

    public Project(long id, string name, string ticker)
    {
        Id = id;
        Name = name;
        Ticker = ticker;
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
}