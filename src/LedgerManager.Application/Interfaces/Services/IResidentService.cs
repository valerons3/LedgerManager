using LedgerManager.Application.Common;
using LedgerManager.Application.Contracts.Residents;
using LedgerManager.Application.Dtos;

namespace LedgerManager.Application.Interfaces.Services;

public interface IResidentService
{
    Task<Result<ResidentDto>> GetByIdAsync(Guid id);
    Task<Result<List<ResidentDto>>> GetAllAsync();
    
    Task<Result<ResidentDto>> CreateAsync(CreateResidentRequest resident);
    Task<Result<ResidentDto>> UpdateAsync(Guid id, UpdateResidentRequest resident);
    Task<Result<bool>> DeleteAsync(Guid id);
}