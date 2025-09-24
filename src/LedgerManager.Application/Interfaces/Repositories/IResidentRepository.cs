using LedgerManager.Domain.Entities;

namespace LedgerManager.Application.Interfaces.Repositories;

public interface IResidentRepository
{
    Task<Resident?> GetByIdAsync(Guid id);
    Task<List<Resident>> GetAllAsync();
    
    Task AddAsync(Resident resident);
    Task UpdateAsync(Resident resident);
    Task DeleteAsync(Resident resident);
    
}