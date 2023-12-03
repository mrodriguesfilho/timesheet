namespace TimeSheet.Domain.Entities;

public class Project : BaseEntity<long>
{
    public string Name { get; init; }

    public Dictionary<DateTime, TimeSpan> WorkedHoursByDay { get; init; }
    
    public Project(string name, Dictionary<DateTime, TimeSpan> workedHoursByDay)
    {
        Name = name;
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