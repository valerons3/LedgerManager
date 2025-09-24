using LedgerManager.Application.Contracts.Residents;
using LedgerManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LedgerManager.API.Controllers;

/// <summary>
/// Контроллер для работы с жильцами
/// </summary>
[ApiController]
[Route("api/resident")]
public class ResidentController : ControllerBase
{
    private readonly IResidentService residentService;

    public ResidentController(IResidentService residentService)
    {
        this.residentService = residentService;
    }
    
    /// <summary>
    /// Получить список всех жильцов
    /// </summary>
    /// <response code="200">Возвращает список жильцов</response>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var residentsResult = await residentService.GetAllAsync();
        return Ok(residentsResult.Value); 
    }
    
    /// <summary>
    /// Получить жильца по Id
    /// </summary>
    /// <param name="id">Идентификатор жильца</param>
    /// <response code="200">Возвращает жильца</response>
    /// <response code="404">Жилец с указанным Id не найден</response>
    [HttpGet("{id:guid}", Name = "GetResidentById")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var residentResult = await residentService.GetByIdAsync(id);

        if (!residentResult.IsSuccess)
            return NotFound(new { error = residentResult.Error });

        return Ok(residentResult.Value);
    }
    
    /// <summary>
    /// Создать нового жильца
    /// </summary>
    /// <param name="request">Данные нового жильца</param>
    /// <response code="201">Жилец успешно создан</response>
    /// <response code="400">Ошибка валидации данных</response>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateResidentRequest request)
    {
        var residentResult = await residentService.CreateAsync(request);

        if (!residentResult.IsSuccess)
            return BadRequest(new { error = residentResult.Error });
        
        return CreatedAtRoute(
            routeName: "GetResidentById",
            routeValues: new { id = residentResult.Value.Id },
            value: residentResult.Value
        );
        
    }
    
    /// <summary>
    /// Обновить данные существующего жильца
    /// </summary>
    /// <param name="id">Идентификатор жильца</param>
    /// <param name="request">Новые данные жильца</param>
    /// <response code="200">Данные жильца успешно обновлены</response>
    /// <response code="404">Жилец с указанным Id не найден</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateResidentRequest request)
    {
        var residentResult = await residentService.UpdateAsync(id, request);

        if (!residentResult.IsSuccess)
            return NotFound(new { error = residentResult.Error });

        return Ok(residentResult.Value);
    }
    
    /// <summary>
    /// Удалить жильца
    /// </summary>
    /// <param name="id">Идентификатор жильца</param>
    /// <response code="204">Жилец успешно удален</response>
    /// <response code="404">Жилец с указанным Id не найден</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var residentResult = await residentService.DeleteAsync(id);

        if (!residentResult.IsSuccess)
            return NotFound(new { error = residentResult.Error });

        return NoContent();
    }
}