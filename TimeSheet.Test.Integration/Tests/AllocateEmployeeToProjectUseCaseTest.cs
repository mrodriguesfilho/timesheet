using TimeSheet.Application.DTO;
using TimeSheet.Application.Factories;
using TimeSheet.Application.StaticMessages;
using TimeSheet.Application.UseCases;
using TimeSheet.Test.Integration.Fixtures;

namespace TimeSheet.Test.Integration.Tests;

[Collection(nameof(AllocateEmployeeToProjectUseCaseTestFixture))]
public class AllocateEmployeeToProjectUseCaseTest : AllocateEmployeeToProjectUseCaseTestFixture
{
    private readonly AllocateEmployeeToProjectUseCaseTestFixture _fixture;

    public AllocateEmployeeToProjectUseCaseTest(AllocateEmployeeToProjectUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async void It_should_allocate_an_employee_to_a_project()
    {
        var projectDao = _fixture.CreateProjectDao();
        var projectFactory = new ProjectFactory(projectDao);
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);

        var createEmployeeInput = _fixture.GetValidEmployeeInput();
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var createEmployeeResult = await createEmployeeUseCase.Execute(createEmployeeInput);

        var createProjectInput = _fixture.GetValidProjectInput();
        var createProjectUseCase = new CreateProjectUseCase(projectDao, projectFactory);
        var createProjectResult = await createProjectUseCase.Execute(createProjectInput);

        var allocateEmployeeToProjectInput = new AllocateEmployeeToProjectInput(
            createEmployeeResult.Value.GovernmentIdentification,
            createProjectResult.Value.Ticker);
        
        var allocateEmployeeToProjectUseCase = new AllocateEmployeeToProjectUseCase(
            projectFactory, 
            employeeFactory,
            employeeRepository);
        
        var allocateEmplyoeeToProjectResult = await allocateEmployeeToProjectUseCase.Execute(allocateEmployeeToProjectInput);
        
        Assert.True(createProjectResult.Success);
        Assert.True(createEmployeeResult.Success);
        Assert.True(allocateEmplyoeeToProjectResult.Success);
        Assert.Equal(createEmployeeResult.Value.GovernmentIdentification, allocateEmplyoeeToProjectResult.Value.GovernmentIdentfication);
        Assert.Single(allocateEmplyoeeToProjectResult.Value.AllocatedProjects);
        Assert.Equal(createProjectResult.Value.Ticker, allocateEmplyoeeToProjectResult.Value.AllocatedProjects[0].Ticker);
    }

    [Fact]
    public async void It_shouldnt_allocate_an_employee_to_a_non_existant_project()
    {
        var projectDao = _fixture.CreateProjectDao();
        var projectFactory = new ProjectFactory(projectDao);
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);

        var createEmployeeInput = _fixture.GetValidEmployeeInput();
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var createEmployeeResult = await createEmployeeUseCase.Execute(createEmployeeInput);

        var mcDonaldsProjectTicker = "MCDP";
        
        var allocateEmployeeToProjectInput = new AllocateEmployeeToProjectInput(
            createEmployeeResult.Value.GovernmentIdentification,
            mcDonaldsProjectTicker);
        
        var allocateEmployeeToProjectUseCase = new AllocateEmployeeToProjectUseCase(
            projectFactory, 
            employeeFactory,
            employeeRepository);
        
        var allocateEmplyoeeToProjectResult = await allocateEmployeeToProjectUseCase.Execute(allocateEmployeeToProjectInput);
        
        Assert.False(allocateEmplyoeeToProjectResult.Success);
        Assert.Equal(ErrorMessages.ProjectNotFound(mcDonaldsProjectTicker), allocateEmplyoeeToProjectResult.Error);
    }
    
    [Fact]
    public async void It_shouldnt_allocate_an_unexistant_employee()
    {
        var projectDao = _fixture.CreateProjectDao();
        var projectFactory = new ProjectFactory(projectDao);
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);
        
        var createProjectInput = _fixture.GetValidProjectInput();
        var createProjectUseCase = new CreateProjectUseCase(projectDao, projectFactory);
        var createProjectResult = await createProjectUseCase.Execute(createProjectInput);

        var governementIdentification = "111-222-333-44";
        var allocateEmployeeToProjectInput = new AllocateEmployeeToProjectInput(
            governementIdentification,
            createProjectResult.Value.Ticker);
        
        var allocateEmployeeToProjectUseCase = new AllocateEmployeeToProjectUseCase(
            projectFactory, 
            employeeFactory,
            employeeRepository);
        
        var allocateEmplyoeeToProjectResult = await allocateEmployeeToProjectUseCase.Execute(allocateEmployeeToProjectInput);
        
        Assert.False(allocateEmplyoeeToProjectResult.Success);
        Assert.Equal(ErrorMessages.EmployeeNotFound(governementIdentification), allocateEmplyoeeToProjectResult.Error);
    }
    
    [Fact]
    public async void It_shoulnd_allocate_an_employee_to_a_project_twice()
    {
        var projectDao = _fixture.CreateProjectDao();
        var projectFactory = new ProjectFactory(projectDao);
        var employeeRepository = _fixture.CreateEmployeeRepository();
        var employeeFactory = new EmployeeFactory(employeeRepository);

        var createEmployeeInput = _fixture.GetValidEmployeeInput();
        var createEmployeeUseCase = new CreateEmployeeUseCase(employeeRepository, employeeFactory);
        var createEmployeeResult = await createEmployeeUseCase.Execute(createEmployeeInput);

        var createProjectInput = _fixture.GetValidProjectInput();
        var createProjectUseCase = new CreateProjectUseCase(projectDao, projectFactory);
        var createProjectResult = await createProjectUseCase.Execute(createProjectInput);

        var allocateEmployeeToProjectInput = new AllocateEmployeeToProjectInput(
            createEmployeeResult.Value.GovernmentIdentification,
            createProjectResult.Value.Ticker);
        
        var allocateEmployeeToProjectUseCase = new AllocateEmployeeToProjectUseCase(
            projectFactory, 
            employeeFactory,
            employeeRepository);
        
        var successfulAllocateEmplyoeeToProjectResult = await allocateEmployeeToProjectUseCase.Execute(allocateEmployeeToProjectInput);
        var unsuccessfulAllocateEmplyoeeToProjectResult = await allocateEmployeeToProjectUseCase.Execute(allocateEmployeeToProjectInput);
        
        Assert.True(createProjectResult.Success);
        Assert.True(createEmployeeResult.Success);
        Assert.True(successfulAllocateEmplyoeeToProjectResult.Success);
        Assert.False(unsuccessfulAllocateEmplyoeeToProjectResult.Success);
        Assert.Equal(ErrorMessages.EmployeeAlreadyAllocatedToProject(createProjectResult.Value.Ticker), unsuccessfulAllocateEmplyoeeToProjectResult.Error);
        Assert.Equal(createEmployeeResult.Value.GovernmentIdentification, successfulAllocateEmplyoeeToProjectResult.Value.GovernmentIdentfication);
        Assert.Single(successfulAllocateEmplyoeeToProjectResult.Value.AllocatedProjects);
        Assert.Equal(createProjectResult.Value.Ticker, successfulAllocateEmplyoeeToProjectResult.Value.AllocatedProjects[0].Ticker);
    }
}