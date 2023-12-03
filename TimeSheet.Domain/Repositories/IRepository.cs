namespace TimeSheet.Domain.Repositories;

public interface IRepository
{
    Task<bool> SaveChangesAsync();
}