namespace LedgerManager.Application.Contracts.Accounts;

public record UpdateAccountRequest(DateTime StartDate, DateTime EndDate, string Address,
    int Area);