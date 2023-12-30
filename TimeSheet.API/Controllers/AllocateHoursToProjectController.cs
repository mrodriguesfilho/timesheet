using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;

namespace TimeSheet.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AllocateHoursToProjectController : ControllerBase
{
    private readonly IUseCase<AllocateHoursToProjectInput, Result<AllocateHoursToProjectOutput>>
        _allocateHoursToProjectUseCase;

    public AllocateHoursToProjectController(
        IUseCase<AllocateHoursToProjectInput, Result<AllocateHoursToProjectOutput>> allocateHoursToProjectUseCase)
    {
        _allocateHoursToProjectUseCase = allocateHoursToProjectUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> AllocateHoursToProject(
        [FromBody] AllocateHoursToProjectInput allocateHoursToProjectInput)
    {
        var response = await _allocateHoursToProjectUseCase.Execute(allocateHoursToProjectInput);
        return Ok(response);
    }
}