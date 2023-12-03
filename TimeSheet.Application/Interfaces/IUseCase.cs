namespace TimeSheet.Application.Interfaces;

public interface IUseCase<Tinput, Toutput>
{
    Task<Toutput> Execute(Tinput createTimeSheetEntryInput);
}