using Microsoft.EntityFrameworkCore;
using TimeSheet.Application.DTO;
using TimeSheet.Application.Factories;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.UseCases;
using TimeSheet.Database.Repositories.EFCore;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Configuration
    .AddJsonFile("appsettings.json", false, true);

var pgSqlConnectionString = builder.Configuration.GetConnectionString("PgSqlConnectionString");
builder.Services.AddDbContext<EmployeeDbContext>(options => options.UseNpgsql(pgSqlConnectionString));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeFactory, EmployeeFactory>();
builder.Services.AddScoped<IUseCase<CreateEmployeeInput, Result<CreateEmployeeOutput>>, CreateEmployeeUseCase>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();