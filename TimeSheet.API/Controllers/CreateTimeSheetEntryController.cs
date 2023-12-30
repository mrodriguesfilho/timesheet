using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.DTO;
using TimeSheet.Application.Interfaces;
using TimeSheet.Application.UseCases;

namespace TimeSheet.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateTimeSheetEntryController : ControllerBase
{
    private readonly IUseCase<CreateTimeSheetEntryInput, Result<CreateTimeSheetEntryOutput>>
        _createTimeSheetEntryUseCase;

    public CreateTimeSheetEntryController(
        IUseCase<CreateTimeSheetEntryInput, Result<CreateTimeSheetEntryOutput>> createTimeSheetEntryUseCase)
    {
        _createTimeSheetEntryUseCase = createTimeSheetEntryUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTimeSheetEntry(
        [FromBody] CreateTimeSheetEntryInput createTimeSheetEntryInput)
    {
        var response = await _createTimeSheetEntryUseCase.Execute(createTimeSheetEntryInput);
        return Ok(response);
    }
}