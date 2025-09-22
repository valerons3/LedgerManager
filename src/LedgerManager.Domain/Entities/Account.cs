namespace LedgerManager.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public required string AccountNumber { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string Address { get; set; }
    public required int Area { get; set; }
    
    public List<Resident> Residents { get; set; } = new();
}
