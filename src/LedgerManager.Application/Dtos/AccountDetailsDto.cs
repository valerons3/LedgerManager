namespace LedgerManager.Application.Dtos;

public record AccountDetailsDto(
    Guid Id,
    string AccountNumber,
    DateTime StartDate,
    DateTime? EndDate,
    string Address,
    int Area,
    IReadOnlyList<ResidentDto> Residents
);