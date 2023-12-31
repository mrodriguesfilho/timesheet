namespace TimeSheet.Domain.Entities;

public class TimeSheetEntity
{
    private readonly Dictionary<DateTime, List<TimeSheetEntry>> _timeEntriesByDay;

    public TimeSheetEntity()
    {
        _timeEntriesByDay = new Dictionary<DateTime, List<TimeSheetEntry>>();
    }
    
    public TimeSheetEntity(Dictionary<DateTime, List<TimeSheetEntry>> timeEntriesByDay)
    {
        _timeEntriesByDay = timeEntriesByDay;
    }

    public void AddTimeEntry(DateTime dateTime)
    {
        if (dateTime == default) return;

        if (_timeEntriesByDay.TryGetValue(dateTime.Date, out var entriesByDayList))
        {
            var lastTimeSheetEntry = entriesByDayList.Last();

            if (!lastTimeSheetEntry.IsCompleted) 
            {
                lastTimeSheetEntry.SetEndDate(dateTime);
                return;
            }   

            entriesByDayList.Add(new TimeSheetEntry(dateTime));
            return;
        }
        
        _timeEntriesByDay.Add(dateTime.Date, new List<TimeSheetEntry>(){ new TimeSheetEntry(dateTime) });
    }

    public Dictionary<DateTime, TimeSpan> CalculateWorkedTime()
    {
        var timeWorkedByDay = new Dictionary<DateTime, TimeSpan>();
        foreach (var workedDayEntryDict in _timeEntriesByDay)
        {
            var timeWorked = new TimeSpan();
            foreach (var timeSheetEntry in workedDayEntryDict.Value)
            {
                timeWorked += timeSheetEntry.WorkedHours;
            }

            timeWorkedByDay.Add(workedDayEntryDict.Key, timeWorked);
        }

        return timeWorkedByDay;
    }

    public List<TimeSheetEntry> GetTimeSheetEntriesByDayOfWork(DateTime dayOfWork)
    {
        if (!_timeEntriesByDay.TryGetValue(dayOfWork, out var dayOfWorkTimeSheetEntries)) return new List<TimeSheetEntry>();
        
        return dayOfWorkTimeSheetEntries;
    }

    public List<TimeSheetEntry> GetAllTimeSheetEntries()
    {
        var timeSheetEntries = new List<TimeSheetEntry>();
        
        foreach (var timeSheetEntry in _timeEntriesByDay)
        {
            timeSheetEntries.AddRange(timeSheetEntry.Value);
        }

        return timeSheetEntries;
    }
}