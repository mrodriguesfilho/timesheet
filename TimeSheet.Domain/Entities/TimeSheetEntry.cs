using TimeSheet.Domain.Enums;

namespace TimeSheet.Domain.Entities;

public class TimeSheetEntry : BaseEntity<long>
{
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; private set; }
    public TimeSpan WorkedHours { get; private set; }
    public TimeSpan HoursAllocated { get; private set; }
    public DateTime? LastHoursAlloactionDate { get; private set; }
    public TimeSpan HoursAvailableToAllocate => WorkedHours - HoursAllocated;
    public bool IsCompleted { get; private set; }
    
    public TimeSheetEntry(DateTime startDate)
    {
        StartDate = startDate;
    }

    public TimeSheetEntry(DateTime startDate, DateTime? endDate, TimeSpan workedHours, TimeSpan hoursAllocated, bool isCompleted)
    {
        StartDate = startDate;
        EndDate = endDate;
        WorkedHours = workedHours;
        HoursAllocated = hoursAllocated;
        IsCompleted = isCompleted;
        SetWorkedHours();
    }
    
    public void SetEndDate(DateTime endDate)
    {
        if (IsCompleted) return;
        
        EndDate = endDate;
        IsCompleted = true;
        SetWorkedHours();
    }

    private void SetWorkedHours()
    {
        if(!IsCompleted || EndDate is null) WorkedHours = TimeSpan.Zero;
            
        WorkedHours = EndDate.GetValueOrDefault() - StartDate;
    }

    public void AllocateHours(TimeSpan hoursAvailableToAllocate)
    {
        HoursAllocated += hoursAvailableToAllocate;
        EntityStatus = EntityStatus.PendingCommit;
        LastHoursAlloactionDate = DateTime.Now;
    }
}