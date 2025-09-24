namespace LedgerManager.Application.Contracts.Residents;

public record CreateResidentRequest(
    string firstName,
    string lastName,
    string middleName,
    DateTime? birthDate,
    Guid accountId
    );