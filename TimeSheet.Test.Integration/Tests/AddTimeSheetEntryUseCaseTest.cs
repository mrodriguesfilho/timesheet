using TimeSheet.Application.DTO;
using TimeSheet.Application.Factories;
using TimeSheet.Application.StaticData;
using TimeSheet.Application.UseCases;
using TimeSheet.Test.Integration.Fixtures;

namespace TimeSheet.Test.Integration.Tests;

[Collection(nameof(AddTimeSheetEntryUseCaseTestFixture))]
public class AddTimeSheetEntryUseCaseTest
{
    private readonly AddTimeSheetEntryUseCaseTestFixture _fixture;

    public AddTimeSheetEntryUseCaseTest(AddTimeSheetEntryUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async void It_should_add_a_time_sheet_entry()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var employee = _fixture.GetValidEmployeeInput();
        var createEmployeeResult = await createEmployeeUseCase.Execute(employee);
        var addTimeSheetEntryUseCase = new AddTimeSheetEntryUseCase(employeeFactory, employeeRepository);
        var createTimeSheetEntryInput =
            new CreateTimeSheetEntryInput(createEmployeeResult.Value.GovernmentIdentification,
                new DateTime(2023, 12, 26, 08, 01, 23));
        var addTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(createTimeSheetEntryInput);
        
        Assert.True(createEmployeeResult.Success);
        Assert.True(addTimeSheetEntryResult.Success);
        Assert.NotNull(addTimeSheetEntryResult.Value.TimeSheet);
        Assert.Single(addTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries());
    }

    [Fact]
    public async void It_should_add_two_time_sheet_entries()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var employee = _fixture.GetValidEmployeeInput();
        var createEmployeeResult = await createEmployeeUseCase.Execute(employee);
        var addTimeSheetEntryUseCase = new AddTimeSheetEntryUseCase(employeeFactory, employeeRepository);
        var arriveTime = new DateTime(2023, 12, 26, 08, 01, 23); 
        var arriveTimeSheetEntryInput =
            new CreateTimeSheetEntryInput(createEmployeeResult.Value.GovernmentIdentification,
                arriveTime);
        var lunchLeaveTime = new DateTime(2023, 12, 26, 12, 01, 23); 
        var lunchTimeSheetEntryInput =
            new CreateTimeSheetEntryInput(createEmployeeResult.Value.GovernmentIdentification,
                lunchLeaveTime);
        var arriveTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(arriveTimeSheetEntryInput);
        var lunchTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(lunchTimeSheetEntryInput);
        
        Assert.True(createEmployeeResult.Success);
        Assert.True(arriveTimeSheetEntryResult.Success);
        Assert.True(lunchTimeSheetEntryResult.Success);
        Assert.Single(lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries());
        Assert.True(lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].IsCompleted);
        Assert.Equal(arriveTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].StartDate);
        Assert.Equal(lunchLeaveTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].EndDate);
        Assert.Equal(TimeSpan.FromHours(4), lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].WorkedHours);
    }
    
    [Fact]
    public async void It_should_add_three_time_sheet_entries_on_the_same_day()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var employee = _fixture.GetValidEmployeeInput();
        var createEmployeeResult = await createEmployeeUseCase.Execute(employee);
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

        Assert.True(createEmployeeResult.Success);
        Assert.True(arriveTimeSheetEntryResult.Success);
        Assert.True(lunchTimeSheetEntryResult.Success);
        Assert.True(lunchReturnTimeSheetEntryResult.Success);
        Assert.Equal(2, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries().Count);
        Assert.True(lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].IsCompleted);
        Assert.False(lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[1].IsCompleted);
        Assert.Equal(arriveTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].StartDate);
        Assert.Equal(lunchReturnTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[1].StartDate);
        Assert.Equal(lunchLeaveTime, lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].EndDate);
        Assert.Equal(TimeSpan.FromHours(4), lunchTimeSheetEntryResult.Value.TimeSheet.GetAllTimeSheetEntries()[0].WorkedHours);
    }
    
        [Fact]
    public async void It_should_add_entries_for_a_complete_day_of_work()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var employee = _fixture.GetValidEmployeeInput();
        var createEmployeeResult = await createEmployeeUseCase.Execute(employee);
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
        
        Assert.True(createEmployeeResult.Success);
        Assert.True(arriveTimeSheetEntryResult.Success);
        Assert.True(lunchTimeSheetEntryResult.Success);
        Assert.True(lunchReturnTimeSheetEntryResult.Success);
        Assert.True(officeLeaveTimeSheetEntryResult.Success);
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
    
    
    [Fact]
    public async void It_shouldnt_add_a_time_sheet_entry_to_an_non_existant_employee()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var employee = _fixture.GetValidEmployeeInput();
        var addTimeSheetEntryUseCase = new AddTimeSheetEntryUseCase(employeeFactory, employeeRepository);
        var createTimeSheetEntryInput =
            new CreateTimeSheetEntryInput("111-111-111-00",
                new DateTime(2023, 12, 26, 08, 01, 23));
        var addTimeSheetEntryResult = await addTimeSheetEntryUseCase.Execute(createTimeSheetEntryInput);
        
        Assert.False(addTimeSheetEntryResult.Success);
        Assert.Equal(ErrorMessages.EmployeeNotFound("111-111-111-00"), addTimeSheetEntryResult.Error);
    }
}