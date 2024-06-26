using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.Mappers;
using TimeSheet.Application.StaticMessages;
using TimeSheet.Domain.Factories;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.UseCases;

public class CreateProjectUseCase : IUseCase<CreateProjectInput, Result<CreateProjectOutput>>
{
    private readonly IProjectDao _projectDao;
    private readonly IProjectFactory _projectFactory;
    
    public CreateProjectUseCase(IProjectDao projectDao, IProjectFactory projectFactory)
    {
        _projectDao = projectDao;
        _projectFactory = projectFactory;
    }

    public async Task<Result<CreateProjectOutput>> Execute(CreateProjectInput allocateEmployeeToProjectInput)
    {
        var project = await _projectFactory.Create(allocateEmployeeToProjectInput.Ticker, allocateEmployeeToProjectInput.Name);

        if (project is null)
            return Result
                .Fail(default(CreateProjectOutput),
                    string.Format(ErrorMessages.UnableToCreateProject, allocateEmployeeToProjectInput.Name,
                        allocateEmployeeToProjectInput.Ticker));
        
        if (project.Id != default) 
            return Result
                .Fail(ProjectMapper.MapToCreateProjectOutput(project), 
                    ErrorMessages.ProjectAlreadyExists(allocateEmployeeToProjectInput.Ticker));

        await _projectDao.Create(project);

        var createProjectOutput = ProjectMapper.MapToCreateProjectOutput(project);
        return Result.Ok(createProjectOutput);
    }
}