namespace TimeSheet.Application.DTO;

public record struct AllocateHoursToProjectInput(string GovernmentIdentification, string Ticker, DateTime Date, TimeSpan HoursToAllocates);