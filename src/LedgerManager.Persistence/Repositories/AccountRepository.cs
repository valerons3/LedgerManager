using LedgerManager.Application.Contracts.Accounts;
using LedgerManager.Application.Interfaces.Repositories;
using LedgerManager.Domain.Entities;
using LedgerManager.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LedgerManager.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext context;

    public AccountRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Account>> GetAllAsync()
    {
        return await context.Accounts.ToListAsync();
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public async Task<Account?> GetAccountWithDetailsAsync(Guid id)
    {
        return await context.Accounts
            .Include(a => a.Residents)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task UpdateAsync(Account account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public async Task AddAsync(Account account)
    {
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Account account)
    {
        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
    }

    public async Task<List<Account>> GetAccountsAsync(AccountFilter filter)
    {
        IQueryable<Account> query = context.Accounts
            .Include(a => a.Residents)
            .AsQueryable()
            .AsNoTracking();

        if (filter.HasResidents.HasValue && filter.HasResidents.Value)
            query = query.Where(a => a.Residents.Any());

        if (!string.IsNullOrEmpty(filter.Number))
            query = query.Where(a => a.AccountNumber.Contains(filter.Number));

        if (filter.ActiveOnDate.HasValue)
            query = query.Where(a =>
                a.StartDate <= filter.ActiveOnDate.Value &&
                a.EndDate >= filter.ActiveOnDate.Value);

        if (!string.IsNullOrEmpty(filter.ResidentFirstName))
            query = query.Where(a => a.Residents.Any(r => r.FirstName.Contains(filter.ResidentFirstName)));
        if (!string.IsNullOrEmpty(filter.ResidentSecondName))
            query = query.Where(a => a.Residents.Any(r => r.LastName.Contains(filter.ResidentSecondName)));
        if (!string.IsNullOrEmpty(filter.ResidentMiddleName))
            query = query.Where(a => a.Residents.Any(r => r.MiddleName.Contains(filter.ResidentMiddleName)));

        if (!string.IsNullOrEmpty(filter.Address))
            query = query.Where(a => a.Address.Contains(filter.Address));

        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            query = filter.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
        }

        query = query.Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);

        return await query.ToListAsync();
    }
}