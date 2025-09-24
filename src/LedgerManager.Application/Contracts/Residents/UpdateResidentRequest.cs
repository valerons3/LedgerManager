namespace LedgerManager.Application.Contracts.Residents;

public record UpdateResidentRequest(
    string firstName,
    string lastName,
    string middleName,
    DateTime? birthDate,
    Guid accountId
);