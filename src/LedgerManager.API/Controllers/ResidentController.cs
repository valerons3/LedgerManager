using LedgerManager.Application.Contracts.Residents;
using LedgerManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LedgerManager.API.Controllers;

[ApiController]
[Route("api/resident")]
public class ResidentController : ControllerBase
{
    private readonly IResidentService residentService;

    public ResidentController(IResidentService residentService)
    {
        this.residentService = residentService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var residentsResult = await residentService.GetAllAsync();
        return Ok(residentsResult.Value); 
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var residentResult = await residentService.GetByIdAsync(id);

        if (!residentResult.IsSuccess)
            return NotFound(new { error = residentResult.Error });

        return Ok(residentResult.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateResidentRequest request)
    {
        var residentResult = await residentService.CreateAsync(request);

        if (!residentResult.IsSuccess)
            return BadRequest(new { error = residentResult.Error });

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = residentResult.Value.Id },
            residentResult.Value
        );
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateResidentRequest request)
    {
        var residentResult = await residentService.UpdateAsync(id, request);

        if (!residentResult.IsSuccess)
            return NotFound(new { error = residentResult.Error });

        return Ok(residentResult.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var residentResult = await residentService.DeleteAsync(id);

        if (!residentResult.IsSuccess)
            return NotFound(new { error = residentResult.Error });

        return NoContent();
    }
}