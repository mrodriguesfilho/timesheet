using TimeSheet.Domain.Entities;

namespace TimeSheet.Application.DTO;

public record struct CreateTimeSheetEntryOutput(string GovernmentIdentification, string Name, TimeSheetEntity TimeSheet);