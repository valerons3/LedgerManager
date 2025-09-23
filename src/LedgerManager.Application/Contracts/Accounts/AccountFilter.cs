namespace LedgerManager.Application.Contracts.Accounts;

public record AccountFilter
(
    bool? HasResidents = null,            
    string? Number = null,           
    DateTime? ActiveOnDate = null,      
    string? ResidentFirstName = null,
    string? ResidentSecondName = null,
    string? ResidentMiddleName = null,   
    string? Address = null,    
    string? SortBy = null,               
    string? SortDirection = "asc",
    int Page = 1,                        
    int PageSize = 10
);