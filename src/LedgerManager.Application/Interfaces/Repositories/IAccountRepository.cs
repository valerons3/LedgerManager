using LedgerManager.Application.Contracts.Accounts;
using LedgerManager.Domain.Entities;

namespace LedgerManager.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<List<Account>> GetAllAsync();
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task<Account?> GetAccountWithDetailsAsync(Guid id);
    Task<Account?> GetByIdAsync(Guid id);
    
    Task UpdateAsync(Account account);
    Task AddAsync(Account account);
    Task DeleteAsync(Account account);
    
    Task<List<Account>> GetAccountsAsync(AccountFilter filter);
}