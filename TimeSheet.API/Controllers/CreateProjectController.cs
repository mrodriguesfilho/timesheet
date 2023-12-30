using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;

namespace TimeSheet.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateProjectController : ControllerBase
{
    private readonly IUseCase<CreateProjectInput, Result<CreateProjectOutput>> _createProjectUseCase;

    public CreateProjectController(IUseCase<CreateProjectInput, Result<CreateProjectOutput>> createProjectUseCase)
    {
        _createProjectUseCase = createProjectUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectInput createProjectInput)
    {
        var response = await _createProjectUseCase.Execute(createProjectInput);
        return Ok(response);
    }
}