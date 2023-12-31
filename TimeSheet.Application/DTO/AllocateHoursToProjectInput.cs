namespace TimeSheet.Application.DTO;

public record struct AllocateHoursToProjectInput(string GovernmentIdentification, string Ticker, DateTime WorkDay, TimeSpan HoursToAllocate);