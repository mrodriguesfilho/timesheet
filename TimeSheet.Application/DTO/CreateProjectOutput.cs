namespace TimeSheet.Application.DTO;

public record struct CreateProjectOutput(long Id, string Name, string Ticker, DateTime AllocationDate, DateTime? DeallocationDate);
