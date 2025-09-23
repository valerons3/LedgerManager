using LedgerManager.Application.Interfaces.Repositories;
using LedgerManager.Domain.Entities;
using LedgerManager.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LedgerManager.Persistence.Repositories;

public class ResidentRepository : IResidentRepository
{
    private readonly AppDbContext context;

    public ResidentRepository(AppDbContext context)
    {
        this.context = context;
    }
    
    public async Task<Resident?> GetByIdAsync(Guid id)
    {
        return await context.Residents.FindAsync(id);
    }

    public async Task<List<Resident>?> GetAllAsync()
    {
        return await context.Residents.ToListAsync();
    }

    public async Task AddAsync(Resident resident)
    {
        await context.Residents.AddAsync(resident);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Resident resident)
    {
        context.Residents.Update(resident);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Resident resident)
    {
        context.Residents.Remove(resident);
        await context.SaveChangesAsync();
    }

}