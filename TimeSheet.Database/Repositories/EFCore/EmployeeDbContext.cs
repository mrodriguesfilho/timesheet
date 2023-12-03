using Microsoft.EntityFrameworkCore;
using TimeSheet.Database.Mappers;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.Repositories.EFCore;

public class EmployeeDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
   
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Employee>().OwnsOne(employee => employee.TimeSheet);
        builder.ApplyConfiguration(new EmployeeMap()).HasDefaultSchema("mowdb");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}