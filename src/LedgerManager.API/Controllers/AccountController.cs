using LedgerManager.Application.Contracts.Accounts;
using LedgerManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LedgerManager.API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var accounts = await accountService.GetAllAsync();
        return Ok(accounts.Value);
    }

    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> GetAccountByNumberAsync(string accountNumber)
    {
        var accountResult = await accountService.GetByAccountNumberAsync(accountNumber);
        
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
        
        return Ok(accountResult.Value);
    }

    [HttpGet("details/{id:guid}")]
    public async Task<IActionResult> GetAccountDetailsAsync(Guid id)
    {
        var accountResult = await accountService.GetAccountWithDetailsAsync(id);
        
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
        
        return Ok(accountResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest accountDto)
    {
        var accountResult = await accountService.CreateAsync(accountDto);
        
        return CreatedAtAction(
            nameof(GetAccountDetailsAsync), 
            new { id = accountResult.Value.Id }, 
            accountResult.Value
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateAccountRequest accountDto)
    {
        var accountResult = await accountService.UpdateAsync(id, accountDto);
        
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
        
        return Ok(accountResult.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var accountResult = await accountService.DeleteAsync(id);
        
        if (!accountResult.IsSuccess)
            return NotFound(new { error = accountResult.Error });
        
        return NoContent();
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetAccountFilterAsync([FromQuery] AccountFilter filter)
    {
        var accountsResult = await accountService.GetAccountsWithFilterAsync(filter);
        
        if (!accountsResult.IsSuccess)
            return BadRequest(new { error = accountsResult.Error });
        
        return Ok(accountsResult.Value);
    }
}