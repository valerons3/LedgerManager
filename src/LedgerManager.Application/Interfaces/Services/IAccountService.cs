using LedgerManager.Application.Common;
using LedgerManager.Application.Contracts.Accounts;
using LedgerManager.Application.Dtos;
using LedgerManager.Domain.Entities;

namespace LedgerManager.Application.Interfaces.Services;

public interface IAccountService
{
    Task<Result<List<AccountDto>>> GetAllAsync();
    Task<Result<AccountDto>> GetByAccountNumberAsync(string accountNumber);
    Task<Result<AccountDetailsDto>> GetAccountWithDetailsAsync(Guid id);
    
    Task<Result<AccountDto>> CreateAsync(CreateAccountRequest accountDto);
    Task<Result<AccountDto>> UpdateAsync(Guid id, UpdateAccountRequest accountDto);
    Task<Result<bool>> DeleteAsync(Guid id);
    
    Task<Result<List<AccountDto>>> GetAccountsWithFilterAsync(AccountFilter filter);
}