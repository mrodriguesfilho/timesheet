using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;

namespace TimeSheet.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AllocateEmployeeToProjectController : ControllerBase
{
    public IUseCase<AllocateEmployeeToProjectInput, Result<AllocateEmployeeToProjectOutput>>
        _allocateEmployeeToProjectUseCase;

    public AllocateEmployeeToProjectController(IUseCase<AllocateEmployeeToProjectInput, Result<AllocateEmployeeToProjectOutput>> allocateEmployeeToProjectUseCase)
    {
        _allocateEmployeeToProjectUseCase = allocateEmployeeToProjectUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> AllocateEmployeeToProjecT(
        [FromBody] AllocateEmployeeToProjectInput allocateEmployeeToProjectInput)
    {
        var response = await _allocateEmployeeToProjectUseCase.Execute(allocateEmployeeToProjectInput);
        return Ok(response);
    }
}