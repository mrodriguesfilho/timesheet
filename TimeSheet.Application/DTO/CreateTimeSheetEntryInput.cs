namespace TimeSheet.Application.DTO;

public record struct CreateTimeSheetEntryInput(string GovernmentIdentification, DateTime timeEntry);
