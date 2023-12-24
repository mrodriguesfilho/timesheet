using TimeSheet.Application.DTO;
using TimeSheet.Application.Factories;
using TimeSheet.Application.StaticData;
using TimeSheet.Application.UseCases;
using TimeSheet.Test.Integration.Fixtures;

namespace TimeSheet.Test.Integration.Tests;

[Collection(nameof(CreateEmployeeUseCaseTestFixture))]
public class CreateEmployeeUseCaseTest : CreateEmployeeUseCaseTestFixture
{
    private readonly CreateEmployeeUseCaseTestFixture _fixture;
    
    public CreateEmployeeUseCaseTest(CreateEmployeeUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async void It_should_create_an_employee()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);

        var createEmployeeInput = new CreateEmployeeInput("João Pedro", "000-000-000-11");
        var result = await createEmployeeUseCase.Execute(createEmployeeInput);
        
        Assert.True(result.Success);
        Assert.Equal("000-000-000-11", result.Value.GovernmentIdentification);
        Assert.Equal("João Pedro", result.Value.Name);
    }
    
    [Fact]
    public async void It_shouldnt_create_an_employee_twice()
    {
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);

        var createEmployeeInput = _fixture.GetValidEmployeeInput();
        var sucessfulResult = await createEmployeeUseCase.Execute(createEmployeeInput);
        var unsucessfulResult = await createEmployeeUseCase.Execute(createEmployeeInput);
        
        Assert.True(sucessfulResult.Success);
        Assert.False(unsucessfulResult.Success);
        Assert.Equal(ErrorMessages.EmployeeAlreadyExists("111-111-111-22"), unsucessfulResult.Error);
        Assert.Equal("111-111-111-22", sucessfulResult.Value.GovernmentIdentification);
        Assert.Equal("João Pedro", sucessfulResult.Value.Name);
    }
}