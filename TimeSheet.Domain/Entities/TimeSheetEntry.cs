using TimeSheet.Domain.Enums;

namespace TimeSheet.Domain.Entities;

public class TimeSheetEntry : BaseEntity<long>
{
    public DateTime StartDate { get; }
    public DateTime? EndDate { get; private set; }
    public TimeSpan WorkedHours { get; private set; }
    public TimeSpan HoursAllocated { get; private set; }
    public TimeSpan HoursAvailableToAllocate => WorkedHours - HoursAllocated;
    public bool IsCompleted { get; private set; }
    
    public TimeSheetEntry(DateTime startDate)
    {
        StartDate = startDate;
    }

    public TimeSheetEntry(DateTime startDate, DateTime? endDate, TimeSpan hoursAllocated)
    {
        StartDate = startDate;
        HoursAllocated = hoursAllocated;
        SetEndDate(endDate);
    }
    
    public void SetEndDate(DateTime? endDate)
    {
        if (endDate is null || endDate.GetValueOrDefault() == default || IsCompleted) return;
        
        EndDate = endDate;
        IsCompleted = true;
        SetWorkedHours();
    }

    private void SetWorkedHours()
    {
        if(EndDate is null) WorkedHours = TimeSpan.Zero;
            
        WorkedHours = EndDate.GetValueOrDefault() - StartDate;
    }

    public void AllocateHours(TimeSpan hoursAvailableToAllocate)
    {
        HoursAllocated += hoursAvailableToAllocate;
        EntityStatus = EntityStatus.PendingCommit;
    }
}