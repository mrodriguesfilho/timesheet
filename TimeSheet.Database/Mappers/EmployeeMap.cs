using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.Mappers;

public class EmployeeMap : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("tb_employee").HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id_employee");
        builder.Property(x => x.Name).HasColumnName("nm_employee");
        builder.Property(x => x.GovernmentIdentification).HasColumnName("cd_employee_government");
    }
}