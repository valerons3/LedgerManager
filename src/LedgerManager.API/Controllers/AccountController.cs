using LedgerManager.Application.Contracts.Accounts;
using LedgerManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LedgerManager.API.Controllers;

/// <summary>
/// Контроллер для работы с лицевыми счетами (ЛС)
/// </summary>
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }
    
    /// <summary>
    /// Получить все ЛС
    /// </summary>
    /// <returns>Список ЛС</returns>
    /// <response code="200">Возвращает список всех ЛС</response>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var accounts = await accountService.GetAllAsync();
        return Ok(accounts.Value);
    }
    
    /// <summary>
    /// Получить ЛС по номеру
    /// </summary>
    /// <param name="accountNumber">Номер ЛС</param>
    /// <returns>Данные ЛС</returns>
    /// <response code="200">Возвращает ЛС с указанным номером</response>
    /// <response code="404">ЛС с указанным номером не найдено</response>
    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> GetAccountByNumberAsync(string accountNumber)
    {
        var accountResult = await accountService.GetByAccountNumberAsync(accountNumber);
        
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
        
        return Ok(accountResult.Value);
    }
    
    /// <summary>
    /// Получить детальную информацию по ЛС
    /// </summary>
    /// <param name="id">Id ЛС</param>
    /// <returns>Детальная информация о ЛС</returns>
    /// <response code="200">Возвращает детальные данные ЛС</response>
    /// <response code="404">ЛС с указанным Id не найдено</response>
    [HttpGet("details/{id:guid}", Name = "GetAccountDetails")]
    public async Task<IActionResult> GetAccountDetailsAsync(Guid id)
    {
        var accountResult = await accountService.GetAccountWithDetailsAsync(id);
    
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
    
        return Ok(accountResult.Value);
    }

    /// <summary>
    /// Создать новое ЛС
    /// </summary>
    /// <param name="accountDto">Данные ЛС для создания</param>
    /// <returns>Созданное ЛС</returns>
    /// <response code="201">Возвращает созданное ЛС</response>
    /// <response code="400">Ошибка валидации или некорректные данные</response>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest accountDto)
    {
        var accountResult = await accountService.CreateAsync(accountDto);

        if (!accountResult.IsSuccess)
            return BadRequest(new { error = accountResult.Error });

        return CreatedAtRoute(
            routeName: "GetAccountDetails",
            routeValues: new { id = accountResult.Value.Id },
            value: accountResult.Value
        );
    }
    
    /// <summary>
    /// Обновить существующее ЛС
    /// </summary>
    /// <param name="id">Id ЛС</param>
    /// <param name="accountDto">Данные для обновления</param>
    /// <returns>Обновленные данные ЛС</returns>
    /// <response code="200">Возвращает обновленное ЛС</response>
    /// <response code="404">ЛС с указанным Id не найдено</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateAccountRequest accountDto)
    {
        var accountResult = await accountService.UpdateAsync(id, accountDto);
        
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
        
        return Ok(accountResult.Value);
    }
    
    /// <summary>
    /// Удалить ЛС
    /// </summary>
    /// <param name="id">Id ЛС</param>
    /// <response code="204">ЛС успешно удалено</response>
    /// <response code="404">ЛС с указанным Id не найдено</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var accountResult = await accountService.DeleteAsync(id);
        
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
        
        return NoContent();
    }
    
    /// <summary>
    /// Получить ЛС с фильтром
    /// </summary>
    /// <param name="filter">Фильтры поиска ЛС</param>
    /// <returns>Список ЛС, соответствующих фильтру</returns>
    /// <response code="200">Возвращает список ЛС, соответствующих фильтру</response>
    /// <response code="400">Некорректный фильтр</response>
    [HttpGet("filter")]
    public async Task<IActionResult> GetAccountFilterAsync([FromQuery] AccountFilter filter)
    {
        var accountsResult = await accountService.GetAccountsWithFilterAsync(filter);
        
        if (!accountsResult.IsSuccess)
            return BadRequest(new { error = accountsResult.Error });
        
        return Ok(accountsResult.Value);
    }
}