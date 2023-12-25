using TimeSheet.Application.DTO;
using TimeSheet.Application.Factories;
using TimeSheet.Application.UseCases;
using TimeSheet.Test.Integration.Fixtures;

namespace TimeSheet.Test.Integration.Tests;

[Collection(nameof(AllocateHoursToProjectUseCaseTestFixture))]
public class AllocateHoursToProjectUseCaseTest
{
    public readonly AllocateHoursToProjectUseCaseTestFixture _fixture;

    public AllocateHoursToProjectUseCaseTest(AllocateHoursToProjectUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async void It_should_allocate_hours_to_a_project()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var employee = _fixture.GetValidEmployeeInput();
        var createEmployeeResult = await createEmployeeUseCase.Execute(employee);
        var projectDao = _fixture.CreateProjectDao();
        var projectFactory = new ProjectFactory(projectDao);
        var createProjectUseCase = new CreateProjectUseCase(projectDao, projectFactory);
        var createProjectInput = new CreateProjectInput("Burguer King's Project", "BKPJ");
        var createProjectResult = await createProjectUseCase.Execute(createProjectInput);
        var allocateEmployeeToProjectInput = new AllocateEmployeeToProjectInput(
            createEmployeeResult.Value.GovernmentIdentification,
            createProjectResult.Value.Ticker);
        var allocateEmployeeToProjectUseCase = new AllocateEmployeeToProjectUseCase(
            projectFactory, 
            employeeFactory,
            employeeRepository);
        var allocateEmplyoeeToProjectResult = await allocateEmployeeToProjectUseCase.Execute(allocateEmployeeToProjectInput);
        
        var addTimeSheetEntryUseCase = new AddTimeSheetEntryUseCase(employeeFactory, employeeRepository);
        var arriveTime = new DateTime(2023, 12, 26, 08, 01, 23); 
        var arriveTimeSheetEntryInput =
            new CreateTimeSheetEntryInput(createEmployeeResult.Value.GovernmentIdentification,
                arriveTime);
        var arriveTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(arriveTimeSheetEntryInput);
        var lunchLeaveTime = new DateTime(2023, 12, 26, 12, 01, 23); 
        var lunchTimeSheetEntryInput =
            new CreateTimeSheetEntryInput(createEmployeeResult.Value.GovernmentIdentification,
                lunchLeaveTime);
        var lunchTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(lunchTimeSheetEntryInput);
        var lunchReturnTime = new DateTime(2023, 12, 26, 13, 01, 23); 
        var lunchReturnTimeSheetEntryInput =
            new CreateTimeSheetEntryInput(createEmployeeResult.Value.GovernmentIdentification,
                lunchReturnTime);
        var lunchReturnTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(lunchReturnTimeSheetEntryInput);
        var officeLeaveTime = new DateTime(2023, 12, 26, 17, 01, 23); 
        var officeLeaveTimeSheetEntryInput =
            new CreateTimeSheetEntryInput(createEmployeeResult.Value.GovernmentIdentification,
                officeLeaveTime);
        var officeLeaveTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(officeLeaveTimeSheetEntryInput);
        var allocateHoursToProjectInput = new AllocateHoursToProjectInput(
            createEmployeeResult.Value.GovernmentIdentification,
            createProjectResult.Value.Ticker,
            officeLeaveTime.Date,
            TimeSpan.FromHours(8)
            );
        var allocateHoursToProjectUseCase = new AllocateHoursToProjectUseCase(employeeFactory, employeeRepository);
        var alloacteHoursToProjectResult = await allocateHoursToProjectUseCase.Execute(allocateHoursToProjectInput);
        
        Assert.True(createEmployeeResult.Success);
        Assert.True(createProjectResult.Success);
        Assert.True(allocateEmplyoeeToProjectResult.Success);
        Assert.True(arriveTimeSheetEntryResult.Success);
        Assert.True(lunchTimeSheetEntryResult.Success);
        Assert.True(lunchReturnTimeSheetEntryResult.Success);
        Assert.True(officeLeaveTimeSheetEntryResult.Success);
        Assert.True(alloacteHoursToProjectResult.Success);
        Assert.Equal(2, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries().Count);
        Assert.True(lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].IsCompleted);
        Assert.True(lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[1].IsCompleted);
        Assert.Equal(arriveTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].StartDate);
        Assert.Equal(lunchReturnTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[1].StartDate);
        Assert.Equal(lunchLeaveTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].EndDate);
        Assert.Equal(officeLeaveTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[1].EndDate);
        Assert.Equal(TimeSpan.FromHours(4), lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].WorkedHours);
        Assert.Equal(TimeSpan.FromHours(4), officeLeaveTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[1].WorkedHours);
        var workedTimeDictionary = officeLeaveTimeSheetEntryResult.Value.TimeSheet.CalculateWorkedTime();
        var workDayFound = workedTimeDictionary.TryGetValue(officeLeaveTime.Date, out var workedDay);
        Assert.True(workDayFound);
        Assert.Equal(8, workedDay.Hours);
    }
}

