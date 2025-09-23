using LedgerManager.Domain.Entities;
using LedgerManager.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LedgerManager.Persistence.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Resident> Residents => Set<Resident>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new ResidentConfiguration());
    }
}