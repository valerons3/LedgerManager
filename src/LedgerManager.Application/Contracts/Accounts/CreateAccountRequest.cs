namespace LedgerManager.Application.Contracts.Accounts;

public record CreateAccountRequest(DateTime StartDate, DateTime EndDate, string Address,
    int Area);