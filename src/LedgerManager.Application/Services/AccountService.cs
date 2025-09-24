using LedgerManager.Application.Common;
using LedgerManager.Application.Contracts.Accounts;
using LedgerManager.Application.Dtos;
using LedgerManager.Application.Interfaces.Repositories;
using LedgerManager.Application.Interfaces.Services;
using LedgerManager.Domain.Entities;

namespace LedgerManager.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository accountRepository;
    private readonly IAccountNumberGenerator accountNumberGenerator;

    public AccountService(IAccountRepository accountRepository, IAccountNumberGenerator accountNumberGenerator)
    {
        this.accountRepository = accountRepository;
        this.accountNumberGenerator = accountNumberGenerator;
    }
    
    public async Task<Result<List<AccountDto>>> GetAllAsync()
    {
        var accounts = await accountRepository.GetAllAsync();
        var accountsDto = accounts
            .Select(a =>
                new AccountDto(
                    a.Id,
                    a.AccountNumber
                    )).ToList();
        
        return Result<List<AccountDto>>.Success(accountsDto);
    }

    public async Task<Result<AccountDto>> GetByAccountNumberAsync(string accountNumber)
    {
        Account? account = await accountRepository.GetByAccountNumberAsync(accountNumber);
        return account is null
            ? Result<AccountDto>.Failure($"Account with number: {accountNumber} not found")
            : Result<AccountDto>.Success(new AccountDto(account.Id, account.AccountNumber));
    }

    public async Task<Result<AccountDetailsDto>> GetAccountWithDetailsAsync(Guid id)
    {
        Account? account = await accountRepository.GetAccountWithDetailsAsync(id);
        
        return account is null
            ? Result<AccountDetailsDto>.Failure($"Account with id: {id} not found")
            : Result<AccountDetailsDto>.Success(MapToDto(account));
    }
    

    public async Task<Result<AccountDto>> CreateAsync(CreateAccountRequest accountDto)
    {
        var accountNumber = accountNumberGenerator.Generate();
        Account account = new Account()
        {
            Id = Guid.NewGuid(),
            AccountNumber = accountNumber,
            StartDate = accountDto.StartDate,
            EndDate = accountDto.EndDate,
            Address = accountDto.Address,
            Area = accountDto.Area
        };
        await accountRepository.AddAsync(account);
        return Result<AccountDto>.Success(new AccountDto(account.Id, account.AccountNumber));
    }

    public async Task<Result<AccountDto>> UpdateAsync(Guid id, UpdateAccountRequest accountDto)
    {
        Account? account = await accountRepository.GetByIdAsync(id);
        if (account is null)
            return Result<AccountDto>.Failure($"Account with id: {id} not found");
        account.StartDate = accountDto.StartDate;
        account.EndDate = accountDto.EndDate;
        account.Address = accountDto.Address;
        account.Area = accountDto.Area;
        await accountRepository.UpdateAsync(account);
        
        return Result<AccountDto>.Success(new AccountDto(account.Id, account.AccountNumber));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        Account? account = await accountRepository.GetByIdAsync(id);
        if (account is null)
            return Result<bool>.Failure($"Account with id: {id} not found");
        await accountRepository.DeleteAsync(account);
        return Result<bool>.Success(true);
    }

    public async Task<Result<List<AccountDto>>> GetAccountsWithFilterAsync(AccountFilter filter)
    {
        var accounts = await accountRepository.GetAccountsAsync(filter);
        var accountsDto = accounts.Select(a => new AccountDto(a.Id, a.AccountNumber)).ToList();
        return Result<List<AccountDto>>.Success(accountsDto);
    }
    
    private static AccountDetailsDto MapToDto(Account account)
    {
        return new AccountDetailsDto(
            account.Id,
            account.AccountNumber,
            account.StartDate,
            account.EndDate,
            account.Address,
            account.Area,
            account.Residents
                .Select(r => new ResidentDto(r.Id, r.FirstName, r.LastName, r.MiddleName))
                .ToList()
        );
    }
}