using TimeSheet.Application.DTO;
using TimeSheet.Application.Factories;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.UseCases;
using TimeSheet.Database.AdoNet.Adapters.Implementation;
using TimeSheet.Database.AdoNet.Adapters.Interface;
using TimeSheet.Database.AdoNet.DAO;
using TimeSheet.Database.AdoNet.Repositories;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Configuration
    .AddJsonFile("appsettings.json", false, true);

var pgSqlConnectionString = builder.Configuration.GetConnectionString("PgSqlConnectionString");
builder.Services.AddSingleton<IDatabaseAdapter>(new PostgresSqlAdapter(pgSqlConnectionString, 5));
builder.Services.AddScoped<IEmployeeRepository, EmployeeAdoNetRepository>();
builder.Services.AddScoped<IEmployeeFactory, EmployeeFactory>();
builder.Services.AddScoped<IProjectDao, ProjectAdoNetDao>();
builder.Services.AddScoped<IProjectFactory, ProjectFactory>();
builder.Services.AddScoped<IUseCase<CreateEmployeeInput, Result<CreateEmployeeOutput>>, CreateEmployeeUseCase>();
builder.Services.AddScoped<IUseCase<CreateProjectInput, Result<CreateProjectOutput>>, CreateProjectUseCase>();
builder.Services.AddScoped<IUseCase<AllocateEmployeeToProjectInput, Result<AllocateEmployeeToProjectOutput>>, AllocateEmployeeToProjectUseCase>();
builder.Services.AddScoped<IUseCase<CreateTimeSheetEntryInput, Result<CreateTimeSheetEntryOutput>>, CreateTimeSheetEntryUseCase>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();