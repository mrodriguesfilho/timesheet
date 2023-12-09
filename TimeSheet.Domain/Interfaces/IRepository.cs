namespace TimeSheet.Domain.Interfaces;

public interface IRepository
{
    Task<bool> SaveChangesAsync();
}