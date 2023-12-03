using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;

namespace TimeSheet.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateEmployeeController : ControllerBase
{
    private IUseCase<CreateEmployeeInput, Result<CreateEmployeeOutput>> _createEmployeeUseCase;

    public CreateEmployeeController(IUseCase<CreateEmployeeInput, Result<CreateEmployeeOutput>> createEmployeeUseCase)
    {
        _createEmployeeUseCase = createEmployeeUseCase;
    }
    
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeInput createEmployeeInput)
    {
        var response = await _createEmployeeUseCase.Execute(createEmployeeInput);
        return Ok(response);
    }
}