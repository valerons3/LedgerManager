namespace LedgerManager.Domain.Entities;

public class Resident
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime? BirthDate { get; set; } = null!;
    public required Guid AccountId { get; set; }
    
    
    public Account? Account { get; set; } = null!;
}