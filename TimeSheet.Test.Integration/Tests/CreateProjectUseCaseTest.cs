using TimeSheet.Application.DTO;
using TimeSheet.Application.Factories;
using TimeSheet.Application.StaticData;
using TimeSheet.Application.UseCases;
using TimeSheet.Test.Integration.Fixtures;

namespace TimeSheet.Test.Integration.Tests;

[Collection(nameof(CreateProjectUseCaseTestFixture))]

public class CreateProjectUseCaseTest : CreateProjectUseCaseTestFixture
{
    private readonly CreateProjectUseCaseTestFixture _fixture;
    
    public CreateProjectUseCaseTest(CreateProjectUseCaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async void It_should_create_a_project()
    {
        var projectDao = _fixture.CreateProjectDao();
        var projectFactory = new ProjectFactory(projectDao);
        var createProjectUseCase = new CreateProjectUseCase(projectDao, projectFactory);
        var createProjectInput = new CreateProjectInput("Burguer King's Project", "BKPJ");
        var result = await createProjectUseCase.Execute(createProjectInput);
        
        Assert.True(result.Success);
        Assert.NotEqual(0, result.Value.Id);
        Assert.Equal("Burguer King's Project", result.Value.Name);
        Assert.Equal("BKPJ", result.Value.Ticker);
    }
    
    [Fact]
    public async void It_shoulnd_create_a_project_twice()
    {
        var projectDao = _fixture.CreateProjectDao();
        var projectFactory = new ProjectFactory(projectDao);
        var createProjectUseCase = new CreateProjectUseCase(projectDao, projectFactory);
        var createProjectInput = _fixture.GetValidProjectInput();
        var successfulResult = await createProjectUseCase.Execute(createProjectInput);
        var unsuccessffulResult  = await createProjectUseCase.Execute(createProjectInput);
        if(!unsuccessffulResult.Success) Console.WriteLine();
        Assert.True(successfulResult.Success);
        Assert.False(unsuccessffulResult.Success);
        Assert.Equal(ErrorMessages.ProjectAlreadyExists(createProjectInput.Ticker), unsuccessffulResult.Error);
        Assert.NotEqual(0, successfulResult.Value.Id);
        Assert.Equal("McDonalds Project", successfulResult.Value.Name);
        Assert.Equal("MCDP", successfulResult.Value.Ticker);
    }
}