using LedgerManager.Application.Common;
using LedgerManager.Application.Contracts.Residents;
using LedgerManager.Application.Dtos;
using LedgerManager.Application.Interfaces.Repositories;
using LedgerManager.Application.Interfaces.Services;
using LedgerManager.Domain.Entities;

namespace LedgerManager.Application.Services;

public class ResidentService : IResidentService
{
    private readonly IResidentRepository residentRepository;

    public ResidentService(IResidentRepository residentRepository)
    {
        this.residentRepository = residentRepository;
    }
    
    public async Task<Result<ResidentDto>> GetByIdAsync(Guid id)
    {
        var resident = await residentRepository.GetByIdAsync(id);
        if (resident is null)
            return Result<ResidentDto>.Failure($"Resident with id: {id} not found");

        return Result<ResidentDto>.Success(MapToDto(resident));
    }

    public async Task<Result<List<ResidentDto>>> GetAllAsync()
    {
        var residents = await residentRepository.GetAllAsync();
        var dtos = residents.Select(MapToDto).ToList();
        return Result<List<ResidentDto>>.Success(dtos);
    }

    public async Task<Result<ResidentDto>> CreateAsync(CreateResidentRequest request)
    {
        var resident = new Resident
        {
            Id = Guid.NewGuid(),
            FirstName = request.firstName,
            LastName = request.lastName,
            MiddleName = request.middleName,
            BirthDate = request.birthDate,
            AccountId = request.accountId
        };

        await residentRepository.AddAsync(resident);

        return Result<ResidentDto>.Success(MapToDto(resident));
    }

    public async Task<Result<ResidentDto>> UpdateAsync(Guid id, UpdateResidentRequest request)
    {
        var resident = await residentRepository.GetByIdAsync(id);
        if (resident is null)
            return Result<ResidentDto>.Failure($"Resident with id: {id} not found");

        resident.FirstName = request.firstName;
        resident.LastName = request.lastName;
        resident.MiddleName = request.middleName;
        resident.BirthDate = request.birthDate;
        resident.AccountId = request.accountId;

        await residentRepository.UpdateAsync(resident);

        return Result<ResidentDto>.Success(MapToDto(resident));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var resident = await residentRepository.GetByIdAsync(id);
        if (resident is null)
            return Result<bool>.Failure($"Resident with id: {id} not found");

        await residentRepository.DeleteAsync(resident);

        return Result<bool>.Success(true);
    }

    private static ResidentDto MapToDto(Resident r) =>
        new ResidentDto(r.Id, r.FirstName, r.LastName, r.MiddleName);
}
