using System.Text;
using TimeSheet.Database.AdoNet.Adapters.Interface;
using TimeSheet.Database.AdoNet.Queries;
using TimeSheet.Database.AdoNet.Utils;
using TimeSheet.Database.Builders;
using TimeSheet.Database.Extensions;
using TimeSheet.Database.ModelMappers;
using TimeSheet.Database.Models;
using TimeSheet.Domain.Entities;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Database.AdoNet.Repositories;

public class EmployeeAdoNetRepository : IEmployeeRepository
{
    private readonly IDatabaseAdapter _databaseAdapter;

    public EmployeeAdoNetRepository(IDatabaseAdapter databaseAdapter)
    {
        _databaseAdapter = databaseAdapter;
    }

    public async Task Create(Employee employee)
    {
        var query = string.Format(EmployeeQueries.INSERT_EMPLOYEE, employee.Name, employee.GovernmentIdentification);
        var dataBaseResult = await _databaseAdapter.ExecuteScalarAsync(query);

        if (!dataBaseResult.Success) return;

        var newId = Convert.ToInt64(dataBaseResult.Value);
        employee.SetId(newId);
    }

    public async Task Update(Employee employee)
    {
        await AddNewTimeSheetEntries(employee);
        await UpdateEmployeeProjects(employee);
    }

    private async Task UpdateEmployeeProjects(Employee employee)
    {
        var stringBuilder = new StringBuilder();
        var dataBaseResult = await _databaseAdapter.ExecuteQueryAsync<EmployeeProjectModel>(
            string.Format(EmployeeProjectsQueries.SELECT_EMPLOYEE_ALLOCATED_PROJECTS_BY_ID_EMPLOYEE, employee.Id), 
            dataRecord => EmployeeProjectMapper.Map(dataRecord));

        if (!dataBaseResult.Success) return;

        var currentProjectsList = dataBaseResult.Value;
        
        var projectsUpdated = employee.AllocatedProjects.ConvertAll(x => ProjectModelMapper.Map(x));

        var removedProjectIds = new List<long>();
        foreach (var currentProject in currentProjectsList)
        {
            var stillOnProject = employee.AllocatedProjects.FirstOrDefault(x => x.Id == currentProject.ProjectId);
            if(stillOnProject is null) removedProjectIds.Add(currentProject.ProjectId);
        }

        if (removedProjectIds.Any())
        {
            foreach (var removedProjectId in removedProjectIds)
            {
                stringBuilder.AppendFormat(EmployeeProjectsQueries.DEALLOCATE_EMPLOYEE_TO_PROJECT, DateTime.Now.ToPT0H0M0S(),
                    employee.Id, removedProjectId);
            }
        }

        var newProjects = new List<Project>();
        foreach (var allocatedProject in employee.AllocatedProjects)
        {
            var alreadyAllocatedToProject = currentProjectsList.FirstOrDefault(x => x.ProjectId == allocatedProject.Id);
            if (alreadyAllocatedToProject is null) newProjects.Add(allocatedProject);
        }
        
        if (newProjects.Any())
        {
            stringBuilder.Append(EmployeeProjectsQueries.ALLOCATE_EMPLOYEE_TO_PROJECT_BASE);
            foreach (var newProject in newProjects)
            {
                stringBuilder.AppendFormat(EmployeeProjectsQueries.ALLOCATE_EMPLOYEE_TO_PROJECT_VALUES, employee.Id, newProject.Id,
                    newProject.AllocationDate);
            }
        }

        var query = stringBuilder.Append(';').ToString();
        await _databaseAdapter.ExecuteNonQueryAsync(query);
    }

    public async Task<Employee?> GetByGovernmentId(string governmentIdentification)
    {
        var selectEmployeeQuery = string.Format(EmployeeQueries.SELECT_EMPLOYEE_BY_GOVERNMENT_CODE, governmentIdentification);
        var selectEmployeeDatabaseResult = await _databaseAdapter.ExecuteQueryAsync(selectEmployeeQuery, dataRecord => EmployeeModelMapper.Map(dataRecord));

        if (!selectEmployeeDatabaseResult.Success) return null;

        var employeeModel = selectEmployeeDatabaseResult.Value.FirstOrDefault();
        
        var allocatedProjectsDatabaseResult =
            await _databaseAdapter.ExecuteQueryAsync<ProjectModel>(string.Format(ProjectQueries.SELECT_ALLOCATED_PROJECTS_BY_EMPLOYEE_ID, employeeModel.Id),
                dataRecord => ProjectModelMapper.MapWithAllocation(dataRecord));

        var employeeBuilder = new EmployeeBuilder();

        if (allocatedProjectsDatabaseResult.Success) employeeBuilder = employeeBuilder.BuildWithAllocatedProjects(allocatedProjectsDatabaseResult.Value);
        
        var employee = employeeBuilder
            .Build(employeeModel);

        return employee;
    }

    public async Task<Employee?> GetByGovernmentIdWithTimeSheetEntries(string governmentIdentification, DateTime startDate, DateTime endDate)
    {
        var selectEmployeeQuery = string.Format(EmployeeQueries.SELECT_EMPLOYEE_BY_GOVERNMENT_CODE, governmentIdentification);
        var selectEmployeeDatabaseResult = await _databaseAdapter.ExecuteQueryAsync(selectEmployeeQuery, dataRecord => EmployeeModelMapper.Map(dataRecord));

        if (!selectEmployeeDatabaseResult.Success) return null;

        var employeeModel = selectEmployeeDatabaseResult.Value.FirstOrDefault();
        
        var allocatedProjectsDatabaseResult =
            await _databaseAdapter.ExecuteQueryAsync<ProjectModel>(string.Format(ProjectQueries.SELECT_ALLOCATED_PROJECTS_BY_EMPLOYEE_ID, employeeModel.Id),
                dataRecord => ProjectModelMapper.MapWithAllocation(dataRecord));

        var timeSheetEntriesDatabaseResult =
            await _databaseAdapter.ExecuteQueryAsync<TimeSheetEntryModel>(string.Format(TimeSheetEntryQueries.SELECT_ENTRIES_BY_DATE, employeeModel.Id, startDate,
                endDate), dataRecord => TimeSheetEntryMapper.Map(dataRecord));
        
        var employeeBuilder = new EmployeeBuilder();

        if (timeSheetEntriesDatabaseResult.Success)
            employeeBuilder = employeeBuilder.BuildWithTimeSheetEntries(timeSheetEntriesDatabaseResult.Value);
        
        if (allocatedProjectsDatabaseResult.Success) 
            employeeBuilder = employeeBuilder.BuildWithAllocatedProjects(allocatedProjectsDatabaseResult.Value);
        
        var employee = employeeBuilder
            .Build(employeeModel);

        return employee;
    }

    public async Task AddNewTimeSheetEntries(Employee employee)
    {
        var timeSheetEntriesNotAdded = employee.TimeSheet?.GetAllTimeSheetEntries().Where(x => x.Id == 0).ToList();

        if (timeSheetEntriesNotAdded is null) return;

        var insertNewTimeSheetEntrySb = new StringBuilder();
        insertNewTimeSheetEntrySb.Append(TimeSheetEntryQueries.INSERT_TIME_ENTRY_BASE);

        for (var index = 0; index < timeSheetEntriesNotAdded.Count; index++)
        {
            insertNewTimeSheetEntrySb.AppendFormat(
                TimeSheetEntryQueries.INSERT_TIME_ENTRY_VALUE,
                employee.Id,
                timeSheetEntriesNotAdded[index].StartDate.ToStringSingleQuoted(),
                timeSheetEntriesNotAdded[index].EndDate.ToStringSingleQuoted(),
                timeSheetEntriesNotAdded[index].WorkedHours.ToStringSingleQuoted(),
                timeSheetEntriesNotAdded[index].HoursAllocated.ToStringSingleQuoted(),
                timeSheetEntriesNotAdded[index].IsCompleted
            );
            
            if(index + 1 != timeSheetEntriesNotAdded.Count)
                insertNewTimeSheetEntrySb.Append(',');
        }
        
        insertNewTimeSheetEntrySb.Append(';');
        var builtInsertNewTimeSheetEntryQuery = insertNewTimeSheetEntrySb.ToString();

        await _databaseAdapter.ExecuteNonQueryAsync(builtInsertNewTimeSheetEntryQuery);
    }
}