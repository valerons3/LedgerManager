using FluentAssertions;
using LedgerManager.Application.Contracts.Accounts;
using LedgerManager.Application.Interfaces.Repositories;
using LedgerManager.Application.Interfaces.Services;
using LedgerManager.Application.Services;
using LedgerManager.Domain.Entities;
using Moq;

namespace LedgerManager.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> accountRepoMock = new();
    private readonly Mock<IAccountNumberGenerator> numberGenMock = new();
    private readonly AccountService service;
    
    public AccountServiceTests()
    {
        service = new AccountService(accountRepoMock.Object, numberGenMock.Object);
    }

    private static Account CreateAccount(string number = "123", string address = "Somewhere", int area = 100)
    {
        return new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = number,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddYears(1),
            Address = address,
            Area = area,
            Residents = new List<Resident>()
        };
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAccounts()
    {
        var accounts = new List<Account>
        {
            CreateAccount("1234567890"),
            CreateAccount("9876543210")
        };
        accountRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(accounts);

        var result = await service.GetAllAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Select(a => a.AccountNumber).Should().Contain("1234567890");
    }

    [Fact]
    public async Task GetByAccountNumberAsync_ShouldReturnAccount_WhenExists()
    {
        var account = CreateAccount("123");
        accountRepoMock.Setup(r => r.GetByAccountNumberAsync("123")).ReturnsAsync(account);

        var result = await service.GetByAccountNumberAsync("123");

        result.IsSuccess.Should().BeTrue();
        result.Value.AccountNumber.Should().Be("123");
    }

    [Fact]
    public async Task GetByAccountNumberAsync_ShouldFail_WhenNotFound()
    {
        accountRepoMock.Setup(r => r.GetByAccountNumberAsync("notfound"))
            .ReturnsAsync((Account?)null);

        var result = await service.GetByAccountNumberAsync("notfound");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task CreateAsync_ShouldGenerateAccountNumber_AndSaveAccount()
    {
        numberGenMock.Setup(g => g.Generate()).Returns("1234567890");

        var request = new CreateAccountRequest(
            DateTime.UtcNow, DateTime.UtcNow.AddYears(1), "Somewhere", 100);

        Account? savedAccount = null;
        accountRepoMock.Setup(r => r.AddAsync(It.IsAny<Account>()))
            .Callback<Account>(a => savedAccount = a)
            .Returns(Task.CompletedTask);

        var result = await service.CreateAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.AccountNumber.Should().Be("1234567890");
        savedAccount.Should().NotBeNull();
        savedAccount!.Address.Should().Be("Somewhere");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenAccountExists()
    {
        var account = CreateAccount("123", "Old", 50);

        accountRepoMock.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var request = new UpdateAccountRequest(
            DateTime.UtcNow, DateTime.UtcNow.AddYears(1), "New", 200);

        var result = await service.UpdateAsync(account.Id, request);

        result.IsSuccess.Should().BeTrue();
        account.Address.Should().Be("New");
        account.Area.Should().Be(200);
    }

    [Fact]
    public async Task UpdateAsync_ShouldFail_WhenNotFound()
    {
        var request = new UpdateAccountRequest(
            DateTime.UtcNow, DateTime.UtcNow.AddYears(1), "New", 200);

        accountRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Account?)null);

        var result = await service.UpdateAsync(Guid.NewGuid(), request);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenExists()
    {
        var account = CreateAccount("123");
        accountRepoMock.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);

        var result = await service.DeleteAsync(account.Id);

        result.IsSuccess.Should().BeTrue();
        accountRepoMock.Verify(r => r.DeleteAsync(account), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenNotFound()
    {
        accountRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Account?)null);

        var result = await service.DeleteAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetAccountsWithFilterAsync_ShouldReturnMappedDtos()
    {
        var accounts = new List<Account>
        {
            CreateAccount("123"),
            CreateAccount("456")
        };
        accountRepoMock.Setup(r => r.GetAccountsAsync(It.IsAny<AccountFilter>()))
            .ReturnsAsync(accounts);

        var result = await service.GetAccountsWithFilterAsync(new AccountFilter());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Select(a => a.AccountNumber).Should().Contain(new[] { "123", "456" });
    }
}